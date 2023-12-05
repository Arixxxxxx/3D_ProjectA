using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheakNPC : MonoBehaviour
{
    
    [SerializeField] GameObject NearNpc;

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public void F_Set_Player_Near_Npc(GameObject obj, bool value)
    {
        if(value == true)
        {
            NearNpc = obj;
        }
        else
        {
            NearNpc = null;
        }
    }

}
