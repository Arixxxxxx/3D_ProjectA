using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtPlayer : MonoBehaviour
{
    public enum LookPositionType { Outdoor, Downtown }
    public LookPositionType type;
    [SerializeField] Camera townCam;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    Vector3 Pos;
    private void Update()
    {
        switch (type)
        {
            case LookPositionType.Outdoor:
                Pos = cam.transform.eulerAngles;
                Pos.z = 0;
                cam.transform.eulerAngles = Pos;
                transform.LookAt(cam.transform);
                break;

            case LookPositionType.Downtown:
                Pos = townCam.transform.eulerAngles;
                Pos.z = 0;
                townCam.transform.eulerAngles = Pos;
                transform.LookAt(townCam.transform);
                break;

        }


     
        //Vector3 dir = target_Player.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(dir);
    }
}
