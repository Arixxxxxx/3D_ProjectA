using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScreenSC : MonoBehaviour
{
    [SerializeField]  Animator ScreenAnim;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScreenOnOffSetter();
    }

    private void ScreenOnOffSetter()
    {
        if(PlayerMoveController.Inst.IsinWater == true && ScreenAnim.GetBool("Water") == false)
        {
            ScreenAnim.SetBool("Water", true);
        }
        else if(PlayerMoveController.Inst.IsinWater == false && ScreenAnim.GetBool("Water") == true)
        {
            ScreenAnim.SetBool("Water", false);
        }
       
    }
}
