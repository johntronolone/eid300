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




public class NoteInfo
{
    public int noteNum { get; set; } //ranges 0-127
    public float startTime { get; set; } //in seconds
    public float duration { get; set; } //in seconds
    public int velocity { get; set; } //ranges 0-127
}

public class synth : MonoBehaviour
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

    private List<NoteInfo> MelodyList = new List<NoteInfo>();
    private List<NoteInfo> BackingList = new List<NoteInfo>();
    private List<NoteInfo> NoteBarList = new List<NoteInfo>();

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

    private float NoteDur = 0.2f;
    private float Ztime = 0;
    private float Xtime = 0;
    private float Ctime = 0;
    private float Vtime = 0;
    private float Btime = 0;

    int timeIndex1 = 0;
    int timeIndex2 = 0;
    int timeIndex3 = 0;
    int timeIndex4 = 0;
    int timeIndex5 = 0;

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
                node.AddComponent<nodeOscillation>();

                node.transform.position = new Vector3(i*2.0f, 0.1f,j/5.0f);
                node.name = "Node " + (6-i) + " zPos " + j/10.0f;
            }
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
                node.AddComponent<nodeOscillation>();

                node.transform.position = new Vector3(-i * 2.0f, 0.1f, j / 5.0f);
                node.name = "Node " + (6 - i) + " zPos " + j / 10.0f + " Mirror";
            }
        }

        //for (float i = 0; i < 105; i++)
        //{
        //    node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    node.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //    node.GetComponent<Collider>().enabled = !node.GetComponent<Collider>().enabled;
        //    node.AddComponent<nodeOscillation>();

        //    if (i < 21)
        //    {
        //        node.transform.position = new Vector3(10f, 0.05f, (10 - i) / 2);
        //        node.name = "Node 1 " + i;
        //    }

        //    if (i >= 21 & i < 42)
        //    {
        //        node.transform.position = new Vector3(8f, 0.05f, (31 - i) / 2);
        //        node.name = "Node 2 " + i;

        //    }

        //    if (i >= 42 & i < 63)
        //    {
        //        node.transform.position = new Vector3(6f, 0.05f, (52 - i) / 2);
        //        node.name = "Node 3 " + i;

        //    }

        //    if (i >= 63 & i < 84)
        //    {
        //        node.transform.position = new Vector3(4f, 0.05f, (73 - i) / 2);
        //        node.name = "Node 4 " + i;

        //    }

        //    if (i >= 84 & i < 105)
        //    {
        //        node.transform.position = new Vector3(2f, 0.05f, (94 - i) / 2);
        //        node.name = "Node 5 " + i;

        //    }
        //}

        //for (float i = 0; i < 105; i++)
        //{
        //    node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    node.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //    node.GetComponent<Collider>().enabled = !node.GetComponent<Collider>().enabled;
        //    node.AddComponent<nodeOscillation>();

        //    if (i < 21)
        //    {
        //        node.transform.position = new Vector3(-10f, 0.05f, (10 - i) / 2);
        //        node.name = "Node 1 Mirror" + i;
        //    }

        //    if (i >= 21 & i < 42)
        //    {
        //        node.transform.position = new Vector3(-8f, 0.05f, (31 - i) / 2);
        //        node.name = "Node 2 Mirror" + i;

        //    }

        //    if (i >= 42 & i < 63)
        //    {
        //        node.transform.position = new Vector3(-6f, 0.05f, (52 - i) / 2);
        //        node.name = "Node 3 Mirror" + i;

        //    }

        //    if (i >= 63 & i < 84)
        //    {
        //        node.transform.position = new Vector3(-4f, 0.05f, (73 - i) / 2);
        //        node.name = "Node 4 Mirror" + i;

        //    }

        //    if (i >= 84 & i < 105)
        //    {
        //        node.transform.position = new Vector3(-2f, 0.05f, (94 - i) / 2);
        //        node.name = "Node 5 Mirror" + i;

        //    }
        //}


        //populate frequency table
        for (double i = 1; i <= 127; i++)
        {
            double f = 8.1757989156f * Math.Pow(2.0f, (i / 12.0f));
            freqs[(int)i] = (float) f;
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

        //read in midi file
        var midiFile = MidiFile.Read("Assets/pink_panther.mid");
        TempoMap tempoMap = midiFile.GetTempoMap();
        ticksPerQuarterNote = tempoMap.TimeDivision.ToInt16() * 2.0f;

        // parse and extract melody track
        // melody track determines frequency of (monophonic) sound
        using (var notesManager = midiFile.GetTrackChunks().First().ManageNotes())
        {
            var notes = notesManager.Notes;
            foreach (Note note in notes)
            {
                NoteInfo currentNoteRead = new NoteInfo
                {
                    noteNum = note.NoteNumber,
                    startTime = note.Time / ticksPerQuarterNote,
                    duration = note.Length / ticksPerQuarterNote,
                    velocity = note.Velocity,
                };
                //add this object to the melody notes list
                MelodyList.Add(currentNoteRead);
            }
        }


        //parse and extract track of noteBars
        using (var notesManager = midiFile.GetTrackChunks().ElementAtOrDefault(1).ManageNotes())
        {
            var notes = notesManager.Notes;
            //var numNoteBar = 1;
            foreach (Note note in notes)
            {
                NoteInfo currentNoteRead = new NoteInfo
                {
                    noteNum = note.NoteNumber,
                    startTime = note.Time / ticksPerQuarterNote,
                    duration = note.Length / ticksPerQuarterNote,
                    velocity = note.Velocity,
                };
                //add this object to the melody notes list
                NoteBarList.Add(currentNoteRead);


                NoteBar.gameObject.name = "NoteBar " + (currentNoteRead.noteNum - 63) + " ";
                NoteBar.gameObject.transform.localScale = new Vector3(2, 0.1f, currentNoteRead.duration*2.0f);
                NoteBar.gameObject.GetComponent<Collider>().isTrigger = true;
                Instantiate(NoteBar, new Vector3(currentNoteRead.noteNum - 63, 0.01f, (currentNoteRead.startTime+timeOffset) * 2.0f + currentNoteRead.duration), Quaternion.identity);
                Instantiate(NoteBar, new Vector3((-1) * (currentNoteRead.noteNum - 63), 0.01f, (currentNoteRead.startTime + timeOffset) * 2.0f + currentNoteRead.duration), Quaternion.identity);

                totalNotes++; //gets total number of notes for scoring later
            }
        }

        //parse and extract backing track
        using (var notesManager = midiFile.GetTrackChunks().ElementAtOrDefault(2).ManageNotes())
        {
            var notes = notesManager.Notes;
            foreach (Note note in notes)
            {
                NoteInfo currentNoteRead = new NoteInfo
                {
                    noteNum = note.NoteNumber,
                    startTime = note.Time / ticksPerQuarterNote,
                    duration = note.Length / ticksPerQuarterNote,
                    velocity = note.Velocity,
                };
                //add this object to the backing notes list
                BackingList.Add(currentNoteRead);
            }
        }   


        ////parse and extract backing track
        //using (var notesManager = midiFile.GetTrackChunks().ElementAtOrDefault(1).ManageNotes())
        //{
        //    var notes = notesManager.Notes;
        //    foreach (Note note in notes)
        //    {
        //        NoteInfo currentNoteRead = new NoteInfo
        //        {
        //            noteNum = note.NoteNumber,
        //            startTime = note.Time / ticksPerQuarterNote,
        //            duration = note.Length / ticksPerQuarterNote,
        //            velocity = note.Velocity,
        //        };
        //        //add this object to the backing notes list
        //        BackingList.Add(currentNoteRead);
        //    }
        //}


    }


    void Update()
    {


        //noteBarList1 = ListResources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "NoteBar 10 (Clone)");
        //var noteBarList2 = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "NoteBar 8 (Clone)");
        //var noteBarList3 = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "NoteBar 6 (Clone)");
        //var noteBarList4 = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "NoteBar 4 (Clone)");
        //var noteBarList5 = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "NoteBar 2 (Clone)");

       //print(noteBarList1.First());


        //iterate through notes that haven't been played
        foreach (NoteInfo note in BackingList.ToList())
        {
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

        //iterate through melody list to set note frequency
        foreach(NoteInfo note in MelodyList.ToList())
        {
            //check if note should be playing
            if (note.startTime + timeOffset <= Time.time)
            {
                currentNoteFreq = freqs[note.noteNum];
                MelodyList.Remove(note);
                timeIndex1 = 0;
            }
        }

        // TODO: iterate through track of noteBars and determine ZshouldBePlaying etc
        foreach (NoteInfo note in NoteBarList.ToList())
        {
            //check if noteBar has entered interval
            if (note.noteNum == 65)
            {
                if (!Zplaying && note.startTime + timeOffset <= Time.time && GameObject.Find("Movement Cube").transform.position.x <= 3 & GameObject.Find("Movement Cube").transform.position.x >= 1)
                {
                    Zplaying = true;
                    nodePlaying[4] = true;

                    //check if noteBar has left interval
                } else if (note.startTime + timeOffset + note.duration <= Time.time)
                {
                    Zplaying = false;
                    nodePlaying[4] = false;
                    NoteBarList.Remove(note);
                }
            }

            if (note.noteNum == 67)
            {
                if (!Xplaying && note.startTime + timeOffset <= Time.time && GameObject.Find("Movement Cube").transform.position.x <= 5 & GameObject.Find("Movement Cube").transform.position.x >= 3)
                {
                    Xplaying = true;
                    nodePlaying[3] = true;

                    //check if noteBar has left interval
                }
                else if (note.startTime + timeOffset + note.duration <= Time.time)
                {
                    Xplaying = false;
                    nodePlaying[3] = false;
                    NoteBarList.Remove(note);
                }
            }

            if (note.noteNum == 69)
            {
                if (!Cplaying && note.startTime + timeOffset <= Time.time && GameObject.Find("Movement Cube").transform.position.x <= 7 & GameObject.Find("Movement Cube").transform.position.x >= 5)
                {
                    Cplaying = true;
                    nodePlaying[2] = true;

                    //check if noteBar has left interval
                }
                else if (note.startTime + timeOffset + note.duration <= Time.time)
                {
                    Cplaying = false;
                    nodePlaying[2] = false;
                    NoteBarList.Remove(note);
                }
            }

            if (note.noteNum == 71)
            {
                if (!Vplaying && note.startTime + timeOffset <= Time.time && GameObject.Find("Movement Cube").transform.position.x <= 9 & GameObject.Find("Movement Cube").transform.position.x >= 7)
                {
                    Vplaying = true;
                    nodePlaying[1] = true;

                    //check if noteBar has left interval
                }
                else if (note.startTime + timeOffset + note.duration <= Time.time)
                {
                    Vplaying = false;
                    nodePlaying[1] = false;
                    NoteBarList.Remove(note);
                }
            }

            if (note.noteNum == 73)
            {
                if (!Bplaying && note.startTime + timeOffset <= Time.time && GameObject.Find("Movement Cube").transform.position.x <= 11 & GameObject.Find("Movement Cube").transform.position.x >= 9)
                {
                    Bplaying = true;
                    nodePlaying[0] = true;

                    //check if noteBar has left interval
                }
                else if (note.startTime + timeOffset + note.duration <= Time.time)
                {
                    Bplaying = false;
                    nodePlaying[0] = false;
                    NoteBarList.Remove(note);
                }
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (!audioSource.isPlaying)
        //    {
        //        //timeIndex = 0;  //resets timer before playing sound
        //        audioSource.Play();
        //    }
        //    else
        //    {
        //        audioSource.Stop();
        //    }
        //}
        //GameObject synth  = GameObject.FindGameObjectWithTag("NoteBar").transform.position.x == 4;
    


        //foreach (GameObject noteBar1 in noteBarList1.ToList())
        //{
        //    //if activator is interval and note is in interval
        //    print(noteBar1.ToString());
        //    print(noteBar1.transform.position.z);
        //    print(noteBar1.transform.localScale.z);
        //    if (GameObject.Find("Movement Cube").transform.position.x <= 10.5 & GameObject.Find("Movement Cube").transform.position.x >= 9.5 & (noteBar1.transform.position.z <= noteBar1.transform.localScale.z/2.0f)  & noteBar1.transform.position.z >= (-noteBar1.transform.localScale.z / 2.0f))
        //    {
        //        Zplaying = true;    //makes sound play
        //        nodePlaying[0] = true; //makes strings oscillate
        //        precomposedScore++;
        //        Ztime = Time.time;
        //        timeIndex1 = 0;
        //        //print("Z was pressed");
        //    }
        //    else// if (Time.time - Ztime > noteBar1.transform.localScale.z)
        //    {
        //        Zplaying = false;
        //        nodePlaying[0] = false;
        //        timeIndex1 = 0;
        //        //noteBarList1.
        //    }
        //    //else if (Time.time - Ztime > NoteDur)
        //    //{
        //    //    Zplaying = false;
        //     //   nodePlaying[0] = false;
        //     //   timeIndex1 = 0;
        //        //print("Z was released");
        //    //}
        //}

        //foreach (GameObject noteBar2 in noteBarList2)
        //{
        //    if (GameObject.Find("Movement Cube").transform.position.x <= 8.5 & GameObject.Find("Movement Cube").transform.position.x >= 7.5 & (noteBar2.transform.position.z <= noteBar2.transform.localScale.z / 2.0f) & noteBar2.transform.position.z >= (-noteBar2.transform.localScale.z/2.0f))//Mathf.Abs(noteBar2.transform.position.z) <= 0.05)
        //    {
        //        Xplaying = true;
        //        nodePlaying[1] = true;
        //        precomposedScore++;
        //        Xtime = Time.time;
        //        timeIndex2 = 0;
        //        //print("X was pressed");
        //    }
        //    else //if (Time.time - Xtime > NoteDur)
        //    {
        //        Xplaying = false;
        //        nodePlaying[1] = false;
        //        timeIndex2 = 0;
        //        //print("X was released");
        //    }
        //}

        //foreach (GameObject noteBar3 in noteBarList3)
        //{
        //    if (GameObject.Find("Movement Cube").transform.position.x <= 6.5 & GameObject.Find("Movement Cube").transform.position.x >= 5.5 & (noteBar3.transform.position.z <= noteBar3.transform.localScale.z / 2.0f) & noteBar3.transform.position.z >= (-noteBar3.transform.localScale.z / 2.0f))//Mathf.Abs(noteBar3.transform.position.z) <= 0.05)
        //    {
        //        Cplaying = true;
        //        nodePlaying[2] = true;
        //        precomposedScore++;
        //        Ctime = Time.time;
        //        timeIndex3 = 0;
        //        //print("C was pressed");
        //    }
        //    else //if (Time.time - Ctime > NoteDur)
        //    {
        //        Cplaying = false;
        //        nodePlaying[2] = false;
        //        timeIndex3 = 0;
        //        //print("C was released");
        //    }
        //}
        //int j = 0;
        //foreach (GameObject noteBar4 in noteBarList4)
        //{
        //    if (GameObject.Find("Movement Cube").transform.position.x <= 4.5 & GameObject.Find("Movement Cube").transform.position.x >= 3.5 & (noteBar4.transform.position.z <= noteBar4.transform.localScale.z / 2.0f) & noteBar4.transform.position.z >= (-noteBar4.transform.localScale.z / 2.0f))//Mathf.Abs(noteBar4.transform.position.z) <= 0.05)
        //    {
        //        Vplaying = true;
        //        nodePlaying[3] = true;
        //        //j++;
        //        precomposedScore++;
        //        Vtime = Time.time;
        //        timeIndex4 = 0;
        //        //print("V was pressed");
        //    }
        //    else //if (Time.time - Vtime > NoteDur)
        //    {
        //        Vplaying = false;
        //        nodePlaying[3] = false;
        //        timeIndex4 = 0;
        //        //print("V was released");
        //    }
        //}
        //print("m:" + m);
        //print("j: "+j);
        //foreach (GameObject noteBar5 in noteBarList5)
        //{
            ////print(noteBar5.GetInstanceID());
            ////print(noteBar5.transform.position.z);
            ////print(noteBar5.transform.localScale.z);
            //if (GameObject.Find("Movement Cube").transform.position.x <= 2.5 & GameObject.Find("Movement Cube").transform.position.x >= 1.5 & Mathf.Abs(noteBar5.transform.position.z) <= noteBar5.transform.localScale.z / 2.0f)// & noteBar5.transform.position.z = (noteBar5.transform.localScale.z / 2.0f))//Mathf.Abs(noteBar5.transform.position.z) <= 0.05)
            //{
            //    Bplaying = true;
            //    nodePlaying[4] = true;
            //    precomposedScore++;
            //    Btime = Time.time;
            //    timeIndex5 = 0;
            //    //print("B was pressed");
            //    //resetBtimer

            //}
            //else //if (Time.time - Btime > NoteDur)
            //{
            //    Bplaying = false;
            //    nodePlaying[4] = false;
            //    timeIndex5 = 0;
            //    //print("B was released");
            //}

        //}
        //print(Zplaying);
        //print(Xplaying);
        //print(Cplaying);
        //print(Vplaying);
        //print(Bplaying);

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
                    data[i] += CreateSine(timeIndex[j], freqs[j], fs)/4;
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
                data[i] += CreateSine(timeIndex1, currentNoteFreq, fs);
                timeIndex1++;
                /*if (timeIndex1 >= (fs * waveLengthInSeconds1))
                {
                    timeIndex1 = 0;
                }*/
            }
            if (Xplaying)
            {
                data[i] += CreateSine(timeIndex2, currentNoteFreq, fs);
                timeIndex2++;
                //if (timeIndex2 >= (fs * waveLengthInSeconds2))
                //{
                //    timeIndex2 = 0;
                //}
            }
            if (Cplaying)
            {
                data[i] += CreateSine(timeIndex3, currentNoteFreq, fs);
                timeIndex3++;
                //if (timeIndex3 >= (fs * waveLengthInSeconds3))
                //{
                //    timeIndex3 = 0;
                //}
            }
            if (Vplaying)
            {
                data[i] += CreateSine(timeIndex4, currentNoteFreq, fs);
                timeIndex4++;
                //if (timeIndex4 >= (fs * waveLengthInSeconds4))
                //{
                //    timeIndex4 = 0;
                //}
            }
            if (Bplaying)
            {
                data[i] += CreateSine(timeIndex5, currentNoteFreq, fs);
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
