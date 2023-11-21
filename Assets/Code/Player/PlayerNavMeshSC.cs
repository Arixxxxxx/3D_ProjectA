using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMeshSC : MonoBehaviour
{

    NavMeshAgent nav;
    Animator anim;
    [SerializeField] Camera TownCam;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    Vector3 veloVec;
    private void Update()
    {
        InputDestiynationPos();
        animControl();
    }
    private void InputDestiynationPos()
    {
        Ray ray = TownCam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit))
        {
            nav.SetDestination(hit.point);
        }
    }

    float animVerticalValue;
    private void animControl()
    {
        if(nav.remainingDistance > 0.5f)
        {
            animVerticalValue += Time.deltaTime;
            animVerticalValue = Mathf.Clamp(animVerticalValue, 0f, 0.5f);
            anim.SetFloat("Vertical", animVerticalValue);
        }
        else if(nav.remainingDistance < 0.5f)
        {
            animVerticalValue -= Time.deltaTime;
            animVerticalValue = Mathf.Clamp(animVerticalValue, 0f, 0.5f);
            anim.SetFloat("Vertical", animVerticalValue);
        }
        else if(nav.velocity == Vector3.zero)
        {
            animVerticalValue = 0f;
        }
    }
    public Vector3 F_Out_Player_Pos()
    {
        Vector3 Pos = transform.position;
        return Pos;
    }
}
