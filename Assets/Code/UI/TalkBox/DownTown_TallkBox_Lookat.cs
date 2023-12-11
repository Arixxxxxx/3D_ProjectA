using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTown_TallkBox_Lookat : MonoBehaviour
{
    [SerializeField] Camera townCam;
    Vector3 camPos;
    Vector3 myRot;
    void Start()
    {
        myRot = transform.eulerAngles;
    }

    
    void Update()
    {
        camPos = townCam.transform.position;
        transform.LookAt(camPos);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, 0);

        
    }
}
