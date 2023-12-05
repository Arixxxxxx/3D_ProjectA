using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Object : MonoBehaviour
{
    [SerializeField] float popupDistance;
    [SerializeField] float playerAndMeDistance;
    private void Start()
    {
        
    }

    private void Update()
    {
        Cheack_Distance();
    }
    private void Cheack_Distance()
    {
        playerAndMeDistance = PlayerMoveController.Inst.F_Get_PlayerAndObject_Distance(1,transform.position);
    }
}
