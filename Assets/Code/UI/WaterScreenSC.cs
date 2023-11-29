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
        
    }

    public void F_ScreenOnOffSetter(bool Value)
    {
       if(Value == true)
        {
            ScreenAnim.SetBool("Water", true);
        }
        else
        {
            ScreenAnim.SetBool("Water", false);
        }
    }
}
