using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
   

    [SerializeField] GameObject Player;
    PlayerMoveController MoveSc;
    private void Update()
    {

    }

  

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Cheaker"))
    //    {
    //        Player = other.transform.parent.gameObject;
    //    }

    //    if (other.gameObject.CompareTag("Cheaker"))
    //    {
    //        SwimOnOff();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Cheaker") )
    //    {
    //        Player = null;
    //    }

    //    if (other.gameObject.CompareTag("Cheaker"))
    //    {
    //        SwimOnOff();
    //    }
    //}
}
