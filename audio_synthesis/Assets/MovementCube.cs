using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCube : MonoBehaviour
{

    public bool isPlaying = false;
    public float moveSpeed;

    private float initXpos;
    private float deltaX;
    private float targetPos1;
    private float targetPos2;
    private float targetPos3;
    private float targetPos4;
    private float targetPos5;
    private float targetPos6;
    private float targetPos7;
    private float targetPos8;
    private float targetPos9;
    private float targetPos10;

    private float direction;

    public bool[] nodePlaying = new bool[10];


    // Start is called before the first frame update
    void Start()
    {
        nodePlaying = new bool[10];

        initXpos = gameObject.transform.position.x;
        //targetPos1 = GameObject.Find("Node 1").gameObject.transform.position.x;
        moveSpeed = 5f;
    }



    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < 10; i++)
        {
            nodePlaying[i] = false;
        }

        // Make arrow keys into a pos or neg value only
        if (Input.GetAxis("Horizontal") > 0)
        {
            deltaX = moveSpeed * Time.deltaTime;

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            deltaX = -moveSpeed * Time.deltaTime;
        }
        else
        {
            deltaX = 0;
        }

        //move player object by a delta X
        transform.Translate(deltaX, 0f, 0f);

        //oscilate node only if player object has moved past the node
       /* if (transform.position.x <= targetPos1)
        {
            if (deltaX + transform.position.x >= targetPos1)
            {
                nodePlaying[1] = true;
                nodePlaying[8] = true;
            }

        }

        else if (transform.position.x >= targetPos1)
        {
            if (deltaX + transform.position.x <= targetPos1)
            {
                nodePlaying[1] = true;
                nodePlaying[8] = true;
            }
        }

        if (transform.position.x <= targetPos2 && nodePlaying[2] == false)
        {
            if (deltaX + transform.position.x >= targetPos2 && nodePlaying[2] == false)
            {
                isPlaying = true;
                nodePlaying[2] = true;
                nodePlaying[7] = true;

                //GameObject.Find("Node 2").gameObject.GetComponent<nodeOscillation>().nodeNum = 1;
            }

        }

        else if (transform.position.x >= targetPos2 && nodePlaying[2] == false)
        {
            if (deltaX + transform.position.x <= targetPos2 && nodePlaying[2] == false)
            {
                isPlaying = true;
                nodePlaying[2] = true;
                nodePlaying[7] = true;

                //GameObject.Find("Node 2").gameObject.GetComponent<nodeOscillation>().nodeNum = 1;

            }
        }

        if (transform.position.x <= targetPos3 && nodePlaying[3] == false)
        {
            if (deltaX + transform.position.x >= targetPos3 && nodePlaying[3] == false)
            {
                isPlaying = true;
                nodePlaying[3] = true;
                nodePlaying[6] = true;

                //GameObject.Find("Node 3").gameObject.GetComponent<nodeOscillation>().nodeNum = 2;

            }

        }

        else if (transform.position.x >= targetPos3 && nodePlaying[3] == false)
        {
            if (deltaX + transform.position.x <= targetPos3 && nodePlaying[3] == false)
            {
                isPlaying = true;
                nodePlaying[3] = true;
                nodePlaying[6] = true;

                //GameObject.Find("Node 3").gameObject.GetComponent<nodeOscillation>().nodeNum = 2;

            }
        }

        if (transform.position.x <= targetPos4 && nodePlaying[4] == false)
        {
            if (deltaX + transform.position.x >= targetPos4 && nodePlaying[4] == false)
            {
                isPlaying = true;
                nodePlaying[4] = true;
                nodePlaying[9] = true;

                //GameObject.Find("Node 4").gameObject.GetComponent<nodeOscillation>().nodeNum = 3;

            }

        }

        else if (transform.position.x >= targetPos4 && nodePlaying[4] == false)
        {
            if (deltaX + transform.position.x <= targetPos4 && nodePlaying[4] == false)
            {
                isPlaying = true;
                nodePlaying[4] = true;
                nodePlaying[9] = true;

                //GameObject.Find("Node 4").gameObject.GetComponent<nodeOscillation>().nodeNum = 3;

            }
        }

        if (transform.position.x <= targetPos5 && nodePlaying[5] == false)
        {
            if (deltaX + transform.position.x >= targetPos5 && nodePlaying[5] == false)
            {
                isPlaying = true;
                nodePlaying[5] = true;
                nodePlaying[0] = true;

                //GameObject.Find("Node 5").gameObject.GetComponent<nodeOscillation>().nodeNum = 4;

            }

        }

        else if (transform.position.x >= targetPos5 && nodePlaying[5] == false)
        {
            if (deltaX + transform.position.x <= targetPos5 && nodePlaying[5] == false)
            {
                isPlaying = true;
                nodePlaying[5] = true;
                nodePlaying[0] = true;
                //GameObject.Find("Node 5").gameObject.GetComponent<nodeOscillation>().nodeNum = 4;

            }
        }*/

    }

    public float getDeltaX()
    {
        return deltaX;
    }



}

