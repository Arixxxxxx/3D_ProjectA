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

    private void SwimOnOff()
    {
        if (Player != null)
        {
            MoveSc = Player.GetComponent<PlayerMoveController>();
            MoveSc.IsinWater = true;
        }
        else if(Player == null)
       {
            MoveSc.IsinWater = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = other.gameObject;
            SwimOnOff();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = null;
            SwimOnOff();
        }
    }
}
