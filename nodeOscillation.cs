using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeOscillation : MonoBehaviour
{
    private Vector3 spherePosition;
    private float xzPosition, yPosition;
    private float increaseXZPosition, increaseYPosition;
    private bool isPlaying = false;
    private bool spaceIsDown = false;
    private float startTime = 0.0f;

    // Get velocity of striking object
    // Use velocity to adjust amplitude

    // Determine amplitude of oscilattion using distance from origin

    private float t = 0.0f;
    private float x_init = 0.0f;
    private float y_init = 0.0f;
    private float z_init = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Three seconds is period.
        increaseXZPosition = (2.0f * Mathf.PI) / 0.1f;
        x_init = transform.position.x;
        y_init = transform.position.y;
        z_init = transform.position.z;
    }

    void StartAgain()
    {
        increaseXZPosition = (2.0f * Mathf.PI) / 0.1f;
        //x_init = transform.position.x;
       //y_init = transform.position.y;
        //z_init = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) //check this from armMovement.cs
        {
            t = Time.time - startTime; //check start time from armMove....cs
            if (1.0f - (t / 2.0f) > 0.0f)
            {
                spherePosition = new Vector3(x_init + (1.0f - t / 2.0f) * (2.0f * Mathf.Sin(xzPosition)) * Mathf.Cos(Mathf.PI * (Mathf.Abs(transform.position.z)) / 20.0f), y_init, z_init);
                transform.position = spherePosition;

                // Update the rotating position.
                xzPosition += increaseXZPosition * Time.deltaTime;
                if (xzPosition > 2.0f * Mathf.PI)
                {
                    xzPosition = (xzPosition - 2.0f * Mathf.PI);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !spaceIsDown)
        {
            startTime = Time.time;
            isPlaying = true;
            //Start();
            increaseXZPosition = (2.0f * Mathf.PI) / 0.1f;

            spaceIsDown = true;
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceIsDown = false;
        }



    }
}