using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void playPrecomposed()
    {
        Application.LoadLevel(0);

    }

    public void playImprov()
    {
        Application.LoadLevel(2);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
