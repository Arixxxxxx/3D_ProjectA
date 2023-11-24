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
    
    Vector3 Pos;
    private void Update()
    {
        Pos = cam.transform.eulerAngles;
        Pos.z = 0;
        cam.transform.eulerAngles = Pos;

        transform.LookAt(cam.transform);
        //Vector3 dir = target_Player.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(dir);
    }
}
