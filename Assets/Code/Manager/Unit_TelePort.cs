using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_TelePort : MonoBehaviour
{
    public static Unit_TelePort inst;

    [SerializeField] Transform[] TelPoint;


    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }


    }
    void Start()
    {
        
    }

    // 위치이동
    public void F_Teleport(GameObject obj,int value)
    {
            obj.transform.position = TelPoint[value].transform.position;
    }
}
