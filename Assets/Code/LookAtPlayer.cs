using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtPlayer : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
       transform.LookAt(cam.transform);
        //Vector3 dir = target_Player.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(dir);
    }
}
