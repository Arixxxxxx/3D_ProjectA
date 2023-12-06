using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Npc_Cam_Lookat : MonoBehaviour
{
    [SerializeField]  Transform Target_Obj;
    
    void Start()
    {
       
        
    }
  
    void Update()
    {
        transform.LookAt(Target_Obj);
      
    }
}
