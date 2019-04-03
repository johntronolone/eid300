using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{

    private float t = 0.0f;
    private float oscStartTime = 0.0f;    

    public bool isActivated = false;
    private bool isAlreadyOscillating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 1 && !isAlreadyOscillating)
        {
            isActivated = true;
            isAlreadyOscillating = true;
            oscStartTime = Time.time;
        }

        if (oscStartTime > Time.time - oscStartTime)
        {
            isAlreadyOscillating = false;
        }


    }
}
