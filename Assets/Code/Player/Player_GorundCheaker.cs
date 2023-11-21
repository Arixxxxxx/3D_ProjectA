using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_GorundCheaker : MonoBehaviour
{
    [SerializeField] LayerMask CheakGround;
    PlayerMoveController player;
    
    bool doJump;
    
    private void Start()
    {
        player = GetComponentInParent<PlayerMoveController>();
    }



    private void Update()
    {
        doJump = Input.GetButtonDown("Jump");
      
    }
    bool once;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground") && !once)
        {
            once = true;
            //player.F_GroundColliderCheak(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && once)
        {
            once = false;
            //player.F_GroundColliderCheak(false);
        }
    }
}
