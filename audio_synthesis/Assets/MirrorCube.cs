using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCube : MonoBehaviour
{

    private float xPos;
    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(-(Input.mousePosition.x / Screen.width * 24.0f - 12), -0.5f, -0.2f);

        //transform.Translate((-1.999f) * GameObject.Find("Movement Cube").GetComponent<MovementCube>().getDeltaX(), 0f, 0f);



    }
}

