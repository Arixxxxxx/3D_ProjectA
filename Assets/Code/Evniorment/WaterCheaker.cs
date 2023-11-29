using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterCheaker : MonoBehaviour
{
    
    public enum CheakerType { Body, Head }
    public CheakerType type;
    [SerializeField] WaterScreenSC UiManager_Obj;
    [SerializeField] GameObject Water_Obj;
    PlayerMoveController MoveSc;
    GameManager Gm;
    bool isOnce;

    void Start()
    {
        MoveSc = GetComponentInParent<PlayerMoveController>();
        Gm = GameManager.Inst;
    }


    void Update()
    {
        WaterScreenOnOff();
    }

    private void SwimOnOff()
    {
        MoveSc.IsinWater = !MoveSc.IsinWater;
    }
    private void WaterScreenOnOff()
    {
        if (Gm.Water_Obj != null && isOnce == false)
        {
            isOnce = true;
            UiManager_Obj.F_ScreenOnOffSetter(true);
        }
        else if(Gm.Water_Obj == null && isOnce == true)
        {
            isOnce = false;
            UiManager_Obj.F_ScreenOnOffSetter(false);
        }
    } 

    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case CheakerType.Body:

                if (other.gameObject.CompareTag("Water"))
                {
                    SwimOnOff();
                }
                break;

            case CheakerType.Head:


                if (other.gameObject.CompareTag("Water") && Gm.Water_Obj == null)
                {
                    Gm.Water_Obj = other.gameObject;
                }

                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (type)
        {
            case CheakerType.Body:

                if (other.gameObject.CompareTag("Water"))
                {
                    SwimOnOff();
                }
                break;

            case CheakerType.Head:


                if (other.gameObject.CompareTag("Water") && Gm.Water_Obj != null)
                {
                    Gm.Water_Obj = null;
                }

                break;
        }
    }

}
