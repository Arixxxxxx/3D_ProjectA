using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterCheaker : MonoBehaviour
{
    
    public enum CheakerType { Body, Head,Camera }
    public CheakerType type;
    [SerializeField] WaterScreenSC _UiManager_Obj;
    [SerializeField] GameObject water_Obj;
    [SerializeField] Animator waterScrren;
    [SerializeField] ParticleSystem waterUpPs;
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
        if(type == CheakerType.Camera)
        {
            WaterScreenOnOff();
        }
      
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
            _UiManager_Obj.F_ScreenOnOffSetter(true);
        }
        else if(Gm.Water_Obj == null && isOnce == true)
        {
            isOnce = false;
            _UiManager_Obj.F_ScreenOnOffSetter(false);
            waterScrren.SetTrigger("On");
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
                    waterUpPs.Play();
                }
                break;

            case CheakerType.Head:
                
                if (other.gameObject.CompareTag("Water"))
                {
                    MoveSc.IsNoUpInWater = true;
                  
                }
             
                break;

            case CheakerType.Camera:


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
                if (other.gameObject.CompareTag("Water"))
                {
                    MoveSc.IsNoUpInWater = false;
                }
                break;

            case CheakerType.Camera:


                if (other.gameObject.CompareTag("Water") && Gm.Water_Obj != null)
                {
                    Gm.Water_Obj = null;
                }

                break;
        }
    }

}
