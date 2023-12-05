using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Npc_Cam_Lookat : MonoBehaviour
{
    [SerializeField]  Transform Target_Obj;
    void Start()
    {
        if(transform.parent != null)
        {
            Target_Obj = transform.parent;
        }
        else
        {
            Debug.LogError("Npc_Cam_Lookat: This script requires a parent transform. Make sure the camera has a parent object.");
        }
    
        
    }
    [SerializeField] Vector3 parentRot;
    [SerializeField] Vector3 meRot;
    // Update is called once per frame
    void Update()
    {
        parentRot = Target_Obj.transform.localEulerAngles;
        meRot = parentRot * -1;
        transform.localEulerAngles = meRot;
    }
}
