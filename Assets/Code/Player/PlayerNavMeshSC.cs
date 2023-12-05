using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMeshSC : MonoBehaviour
{
    GameManager Gm;
    NavMeshAgent nav;
    Animator anim;
    [SerializeField] GameObject questBox_Obj;
    [SerializeField] LayerMask Ground;
    [SerializeField] LayerMask QuestBoard;
    [SerializeField] Camera TownCam;
    [SerializeField] ParticleSystem clickPointer;
    [SerializeField] float Dis;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Gm = GameManager.Inst;
    }

    Vector3 veloVec;
    private void Update()
    {
        if (questBox_Obj.activeSelf)
        {
            nav.SetDestination(transform.position);
        }
       
        InputDestiynationPos();
        animControl();
        NPC_Outline_Enable_On_Off();
    }

    [SerializeField] bool select_Object;
    [SerializeField] GameObject MouseOver_Obj;
    [SerializeField] GameObject Before_MouseOver_Obj;

    //마우스 포지션 및 버튼클릭 관리함수
    private void InputDestiynationPos()
    {
        if(Gm.IsWindowOpen == true) { return; }
        
        Ray ray = TownCam.ScreenPointToRay(Input.mousePosition);
        select_Object = Ray_Cheak_Interaction_Object_OutLine();

        // 무브 조건
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, Dis, Ground) && select_Object == false)
        {
           nav.SetDestination(hit.point);
           clickPointer.transform.position = hit.point + Vector3.up * 0.05f;
            clickPointer.Play();
        }
        // 상호작용 가능 npc 클릭시
        if (Physics.Raycast(ray, out RaycastHit hit2, Dis, QuestBoard) && hit2.transform.GetComponent<Outline>() != null && select_Object == true)
        {
            if(MouseOver_Obj == null) 
            {
                MouseOver_Obj = hit2.transform.gameObject;
            }

            float DisTance = DisTance_Check(hit2);

            if (Input.GetMouseButtonDown(0) && DisTance > 3)
            {
                nav.SetDestination(hit2.point);
                StartCoroutine(PopupWindow(DisTance, hit2));

            }
            else if(Input.GetMouseButtonDown(0) && DisTance < 3 && questBox_Obj.gameObject.activeSelf == false)
            {
                questBox_Obj.gameObject.SetActive(true);
            }

            
        }
        else if(select_Object == false)
        {
            MouseOver_Obj = null;
        }
    }

    
    IEnumerator PopupWindow(float DisTance , RaycastHit hit)
    {
        float dis = DisTance;
        Debug.Log("11");
        while (dis > 3)
        {
            dis = DisTance_Check(hit);
            Debug.Log("2");
            yield return null;
        }
        
        questBox_Obj.gameObject.SetActive(true); ;
    }
    // 오브젝트 마우스오버 아웃라인 기능
    private void NPC_Outline_Enable_On_Off()
    {
        if(MouseOver_Obj != null && MouseOver_Obj.GetComponent<Outline>() != null)
        {
            MouseOver_Obj.GetComponent<Outline>().enabled = true;
            Before_MouseOver_Obj = MouseOver_Obj;
        }

        if (MouseOver_Obj == null && Before_MouseOver_Obj != null)
        {
            Before_MouseOver_Obj.GetComponent<Outline>().enabled = false;
            Before_MouseOver_Obj = null;
        }
    }
    private bool Ray_Cheak_Interaction_Object_OutLine()
    {
        Ray ray = TownCam.ScreenPointToRay(Input.mousePosition);
               
        return Physics.Raycast(ray, out RaycastHit hit, Dis, QuestBoard);
        
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

    private float DisTance_Check(RaycastHit Hit)
    {
        float Dis = Vector3.Distance(transform.position, Hit.point);
        return Dis;
    }
}
