//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using System;
//using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using System.Linq;
using System.Collections.Generic;
using System;




public class improvNoteInfo
{
    public int noteNum { get; set; } //ranges 0-127
    public float startTime { get; set; } //in seconds
    public float duration { get; set; } //in seconds
    public int velocity { get; set; } //ranges 0-127
}

public class SynthImprov : MonoBehaviour
{

    public bool[] nodePlaying = new bool[10];
    private int precomposedScore = 0;
    private int totalNotes = 0;

    //to update, 
    //place scene object in Assets folder 
    //this creates a prefab
    //drag the prefab into the synth script attached to the game object
    public GameObject NoteBar;

    //this array stores whether or not a note should be playing
    private bool[] isPlaying = new bool[128]; //notes 0-127
    private int[] timeIndex = new int[128];
    private float[] noteStartTime = new float[128];
    private float[] noteDurs = new float[128];
    private float[] freqs = new float[128];

    private float ticksPerQuarterNote = 0;

    private List<improvNoteInfo> MelodyList = new List<improvNoteInfo>();
    private List<improvNoteInfo> BackingList = new List<improvNoteInfo>();
    private List<improvNoteInfo> NoteBarList = new List<improvNoteInfo>();

    private List<GameObject> noteBarList1 = new List<GameObject>();
    private List<GameObject> noteBarList2 = new List<GameObject>();
    private List<GameObject> noteBarList3 = new List<GameObject>();
    private List<GameObject> noteBarList4 = new List<GameObject>();
    private List<GameObject> noteBarList5 = new List<GameObject>();

    private float timeOffset = 5.0f;

    private float currentNoteFreq = 440;

    private float fs = 0;

    private float waveLengthInSeconds1 = 1.0f;
    private float waveLengthInSeconds2 = 1.0f;
    private float waveLengthInSeconds3 = 1.0f;
    private float waveLengthInSeconds4 = 1.0f;
    private float waveLengthInSeconds5 = 1.0f;

    private bool Zplaying = false;
    private bool Xplaying = false;
    private bool Cplaying = false;
    private bool Vplaying = false;
    private bool Bplaying = false;

    private bool actHasLeftZregion = true;
    private bool actHasLeftXregion = true;
    private bool actHasLeftCregion = true;
    private bool actHasLeftVregion = true;
    private bool actHasLeftBregion = true;

   
    private float NoteDur = 1.0f;
    private float Ztime = 0;
    private float Xtime = 0;
    private float Ctime = 0;
    private float Vtime = 0;
    private float Btime = 0;

    private int timeIndex1 = 0;
    private int timeIndex2 = 0;
    private int timeIndex3 = 0;
    private int timeIndex4 = 0;
    private int timeIndex5 = 0;


    private float f1;
    private float f2;
    private float f3;
    private float f4;
    private float f5;


    private float bpm;
    private float bps;
    private float secondsPerQuarterNote;

    //private static double f0 = 220;
    //private static double f1 = f0;
    //private static double f2 = f0 * Math.Pow(2, 2 / 12);
    //private static double f3 = f0 * Math.Pow(2, 4 / 12);
    //private static double f4 = f0 * Math.Pow(2, 7 / 12);
    //private static double f5 = f0 * Math.Pow(2, 9 / 12);

    //var noteBarList;


    AudioSource audioSource;

    private GameObject node;

    void Start()
    {

        for (int i = 0; i < 10; i++)
        {
            nodePlaying[i] = false;
        }

        //create right side spheres
        //for each string create nodes along z-axis
        for (int i = 1; i <= 5; i++) // x positions at 2,4,6,8,10
        {
            for (int j = -25; j <= 25; j++) // z positions range from -5 to 5
            {
                //initialize each sphere
                node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                node.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                node.GetComponent<Collider>().enabled = !node.GetComponent<Collider>().enabled;
                node.AddComponent<improvNodeOscillation>();

                node.transform.position = new Vector3(i * 2.0f, 0.1f, j / 5.0f);
                node.name = "Node " + (6 - i) + " zPos " + j / 10.0f;
            }
        }

        //create left side spheres
        //for each string create nodes along z-axis
        for (int i = 1; i <= 5; i++) // x positions at 2,4,6,8,10
        {
            for (int j = -25; j <= 25; j++) // z positions range from -5 to 5
            {
                //initialize each sphere
                node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                node.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                node.GetComponent<Collider>().enabled = !node.GetComponent<Collider>().enabled;
                node.AddComponent<improvNodeOscillation>();

                node.transform.position = new Vector3(-i * 2.0f, 0.1f, j / 5.0f);
                node.name = "Node " + (6 - i) + " zPos " + j / 10.0f + " Mirror";
            }
        }


        //populate frequency table
        for (double i = 1; i <= 127; i++)
        {
            double f = 8.1757989156f * Math.Pow(2.0f, (i / 12.0f));
            freqs[(int)i] = (float)f;
        }

        //start audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        fs = AudioSettings.outputSampleRate;
        if (!audioSource.isPlaying)
        {
            //timeIndex = 0;  //resets timer before playing sound
            audioSource.Play();
        }

        // beats per minute = 120;
        // beats per second = 2;

       
        //read in midi file
        var midiFile = MidiFile.Read("Assets/pink_panther.mid");
        TempoMap tempoMap = midiFile.GetTempoMap();
        ticksPerQuarterNote = tempoMap.TimeDivision.ToInt16() * 2.0f;

        bpm = 120;
        bps = bpm / 60;
        secondsPerQuarterNote = 1 / bps;

        //f1 = 220.0f;
        //f2 = f1 * Mathf.Pow(2.0f, (2.0f / 12.0f));
        //f3 = f1 * Mathf.Pow(2.0f, (4.0f / 12.0f));
        //f4 = f1 * Mathf.Pow(2.0f, (7.0f / 12.0f));
        //f5 = f1 * Mathf.Pow(2.0f, (9.0f / 12.0f));

        f1 = freqs[64];
        f2 = freqs[67];
        f3 = freqs[69];
        f4 = freqs[71];
        f5 = freqs[74];




        improvNoteInfo lastNoteRead = new improvNoteInfo
        {
            noteNum = 0,
            startTime = 0.0f,
            duration = 0.0f,
            velocity = 0
        };


       
        //parse and extract backing track
        using (var notesManager = midiFile.GetTrackChunks().ElementAtOrDefault(2).ManageNotes())
        {
            var notes = notesManager.Notes;
            foreach (Note note in notes)
            {
                improvNoteInfo currentNoteRead = new improvNoteInfo
                {
                    noteNum = note.NoteNumber,
                    startTime = note.Time / ticksPerQuarterNote,
                    duration = note.Length / ticksPerQuarterNote,
                    velocity = note.Velocity,
                };
                //add this object to the melody notes list
                BackingList.Add(currentNoteRead);
                lastNoteRead = currentNoteRead;
                print(note.Time / ticksPerQuarterNote);
                print(freqs[note.NoteNumber]);
            }
        }

        //TODO: create note bars for each quarter note

        //print(lastNoteRead.startTime);
        //print(lastNoteRead.duration);

        // beatsPerSecond = 120;
        // secondsPerBeat = 1 / beatsPerSecond;

        // note bars are instantiated at (startTime + offset)*2, + duration
        //  with scale of duration * 2

        // start times are 

        /*int quarterNotesPerSong = (int)((lastNoteRead.startTime + lastNoteRead.duration) / secondsPerQuarterNote);
        //print(quarterNotesPerSong);
        for (int i = 1; i <= quarterNotesPerSong; i++)
        {
        
            NoteBar.gameObject.name = "NoteBar " + i + " ";
            NoteBar.gameObject.transform.localScale = new Vector3(10, 0.1f, 0.1f);
            NoteBar.gameObject.GetComponent<Collider>().isTrigger = true;

            Instantiate(NoteBar, new Vector3(5, 0.01f, i * secondsPerQuarterNote * 2.0f + 0.1f), Quaternion.identity);
            Instantiate(NoteBar, new Vector3(-5, 0.01f, i * secondsPerQuarterNote * 2.0f + 0.1f), Quaternion.identity);

        }*/

        // quarterNotesPerSecond = ticksPerQuarterNote
        // numQuaterNotes = (timeOfSong (s)) * (quarterNotesPerSecond (qN / s)) 
        //numQuarterNotes = (currentNoteRead.startTime + currentNoteRead.duration);

        //NoteBar.gameObject.name = "NoteBar " + (currentNoteRead.noteNum - 63) + " ";
        //NoteBar.gameObject.transform.localScale = new Vector3(2, 0.1f, currentNoteRead.duration * 2.0f);
        //NoteBar.gameObject.GetComponent<Collider>().isTrigger = true;


        //Instantiate(NoteBar, new Vector3(currentNoteRead.noteNum - 63, 0.01f, (currentNoteRead.startTime + timeOffset) * 2.0f + currentNoteRead.duration), Quaternion.identity);
        //Instantiate(NoteBar, new Vector3((-1) * (currentNoteRead.noteNum - 63), 0.01f, (currentNoteRead.startTime + timeOffset) * 2.0f + currentNoteRead.duration), Quaternion.identity);







    }


    void Update()
    {

        //TODO: make note play on quarter notes (not arbitrarily)

        //TODO: add checks for if activator left interval
        //  (i.e. string won't play unless you leave the interval)


        //iterate through notes that haven't been played
        //print(BackingList.ToList().First());
        foreach (improvNoteInfo note in BackingList.ToList())
        {
            //print(note.startTime);
            //check if note should start playig
            if (note.startTime + timeOffset <= Time.time)
            {
                isPlaying[note.noteNum] = true;
                timeIndex[note.noteNum] = 0;
                noteStartTime[note.noteNum] = note.startTime;
                noteDurs[note.noteNum] = note.duration;
                BackingList.Remove(note);
            }
        }

        //lowest frequency string (center strings)
        if (actHasLeftZregion && GameObject.Find("Movement Cube").transform.position.x <= 2.1 & GameObject.Find("Movement Cube").transform.position.x >= 1.9)
        {
            Zplaying = true;
            nodePlaying[4] = true;
            Ztime = Time.time;
            timeIndex1 = 0;
            actHasLeftZregion = false;
            //check if noteBar has left interval
        }
        else 
        {
            if (Ztime + NoteDur <= Time.time) //note is over
            {
                Zplaying = false;
                nodePlaying[4] = false;
            }
            if (GameObject.Find("Movement Cube").transform.position.x > 2.1 || GameObject.Find("Movement Cube").transform.position.x < 1.9)
            {
                actHasLeftZregion = true;
            }
        }


        //second to lowest freq
        if (actHasLeftXregion && GameObject.Find("Movement Cube").transform.position.x <= 4.1 & GameObject.Find("Movement Cube").transform.position.x >= 3.9)
        {
            Xplaying = true;
            nodePlaying[3] = true;
            Xtime = Time.time;
            timeIndex2 = 0;
            actHasLeftXregion = false;
        }
        else 
        {
            if (Xtime + NoteDur <= Time.time)
            {
                Xplaying = false;
                nodePlaying[3] = false;
            }
            if (GameObject.Find("Movement Cube").transform.position.x > 4.1 || GameObject.Find("Movement Cube").transform.position.x < 3.9)
            {
                actHasLeftXregion = true;
            }
        }


        //middle freq
        if (actHasLeftCregion && GameObject.Find("Movement Cube").transform.position.x <= 6.1 & GameObject.Find("Movement Cube").transform.position.x >= 5.9)
        {
            Cplaying = true;
            nodePlaying[2] = true;
            Ctime = Time.time;
            timeIndex3 = 0;
            actHasLeftCregion = false;
        }
        else 
        {
            if (Ctime + NoteDur <= Time.time)
            {
                Cplaying = false;
                nodePlaying[2] = false;
            }
            if (GameObject.Find("Movement Cube").transform.position.x > 6.1 || GameObject.Find("Movement Cube").transform.position.x < 5.9)
            {
                actHasLeftCregion = true;
            }
        }

        //second to highest freq
        if (actHasLeftVregion && GameObject.Find("Movement Cube").transform.position.x <= 8.1 & GameObject.Find("Movement Cube").transform.position.x >= 7.9)
        {
            Vplaying = true;
            nodePlaying[1] = true;
            Vtime = Time.time;
            timeIndex4 = 0;
            actHasLeftVregion = false;
        }
        else 
        {
            if (Vtime + NoteDur <= Time.time)
            {
                Vplaying = false;
                nodePlaying[1] = false;
            }
            if (GameObject.Find("Movement Cube").transform.position.x > 8.1 || GameObject.Find("Movement Cube").transform.position.x < 7.9)
            {
                actHasLeftVregion = true;
            }
        }

        //highest freq
        if (actHasLeftBregion && GameObject.Find("Movement Cube").transform.position.x <= 10.1 & GameObject.Find("Movement Cube").transform.position.x >= 9.9)
        {
            Bplaying = true;
            nodePlaying[0] = true;
            Btime = Time.time;
            timeIndex5 = 0;
            actHasLeftBregion = false;
        }
        else 
        {
            if (Btime + NoteDur <= Time.time)
            {
                Bplaying = false;
                nodePlaying[0] = false;
            }
            if (GameObject.Find("Movement Cube").transform.position.x > 10.1 || GameObject.Find("Movement Cube").transform.position.x < 9.9)
            {
                actHasLeftBregion = true;
            }
        }



        for (int i = 0; i <= 127; i++)
        {
            if (isPlaying[i])
            {
                if (Time.time - (noteStartTime[i] + timeOffset) > noteDurs[i])
                {
                    isPlaying[i] = false;
                }
            }
        }


        if (Time.time == 20)
        {
            endGame(precomposedScore);
        }

    }

    void OnAudioFilterRead(float[] data, int channels)
    {

        //run this for loop for each of the frequencies

        //figure out how to fill data[]

        for (var i = 0; i < data.Length; i += channels)
        {

            //clear data
            data[i] = 0;

            for (int j = 1; j <= 127; j++)
            {
                if (isPlaying[j])
                {
                    //data[i] += (1.0f - (timeIndex[j]) * noteDurs[j] / fs) * CreateSine(timeIndex[j], freqs[j], fs);
                    data[i] += CreateSine(timeIndex[j], freqs[j], fs) / 4;
                    timeIndex[j]++;
                    //if (timeIndex[j] >= (fs * waveLengthInSeconds1))
                    //{
                    //    timeIndex[j] = 0;
                    //}
                }
            }

            if (Zplaying) //TODO: ZshouldBePlaying
            {
                //( 1.0f - (timeIndex1) * NoteDur / fs) *  <-- envelope
                data[i] += (1.0f - timeIndex1 * NoteDur / fs) * CreateSine(timeIndex1, f1, fs);
                timeIndex1++;
                /*if (timeIndex1 >= (fs * waveLengthInSeconds1))
                {
                    timeIndex1 = 0;
                }*/
            }
            if (Xplaying)
            {
                data[i] += (1.0f - timeIndex2 * NoteDur / fs) * CreateSine(timeIndex2, f2, fs);
                timeIndex2++;
                //if (timeIndex2 >= (fs * waveLengthInSeconds2))
                //{
                //    timeIndex2 = 0;
                //}
            }
            if (Cplaying)
            {
                data[i] += (1.0f - timeIndex3 * NoteDur / fs) * CreateSine(timeIndex3, f3, fs);
                timeIndex3++;
                //if (timeIndex3 >= (fs * waveLengthInSeconds3))
                //{
                //    timeIndex3 = 0;
                //}
            }
            if (Vplaying)
            {
                data[i] += (1.0f - timeIndex4 * NoteDur / fs) * CreateSine(timeIndex4, f4, fs);
                timeIndex4++;
                //if (timeIndex4 >= (fs * waveLengthInSeconds4))
                //{
                //    timeIndex4 = 0;
                //}
            }
            if (Bplaying)
            {
                data[i] += (1.0f - timeIndex5 * NoteDur / fs) * CreateSine(timeIndex5, f5, fs);
                timeIndex5++;
                //if (timeIndex5 >= (fs * waveLengthInSeconds5))
                //{
                //    timeIndex5 = 0;
                //}
            }


            data[i] = data[i] / 10.0f;

            //data[i] = CreateSine(timeIndex, f, fs);

            //timeIndex++;
            //if (timeIndex >= (fs * waveLengthInSeconds))
            //{
            //    timeIndex = 0;
            //}

        }



    }


    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }

    private void endGame(int notes)
    {
        print(totalNotes);
    }




}
