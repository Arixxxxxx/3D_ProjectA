using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Town_Npc_Animation : MonoBehaviour
{
    public enum NPC_Num
    {
        Food, Weapon
    }
    public NPC_Num type;

    [SerializeField] GameObject Player_Obj;
    [Header("# Insert Obj !!!")]
    [Space]
    [SerializeField] GameObject TalkingBox;
    [SerializeField] GameObject RightHand_Obj;
    [Header("# Insert Camera !!!")]
    [Space]
    [SerializeField] GameObject TownCam;
    [SerializeField] GameObject NpcCam;
    [SerializeField] Transform LookPoint;
    Npc_Talk talking;
    Vector3 OriginPos;
    Vector3 OriginRot;

    Animator anim;
    bool canTalking;
    bool doTalking;
    void Start()
    {
        anim = GetComponent<Animator>();
        talking = GetComponent<Npc_Talk>();
        switch (type) 
        {
          case NPC_Num.Weapon:
                OriginPos = transform.position;
                OriginRot = transform.eulerAngles;

                break;
        }
    }

    void Update()
    {
        Interaction();
        LookAtPlayer();
        RightHand_ActiveSelf();


    }

    private void RightHand_ActiveSelf()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Work") && RightHand_Obj.activeSelf == false)
        {
            RightHand_Obj.SetActive(true);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Work") == false && RightHand_Obj.activeSelf == true)
        {
            RightHand_Obj.SetActive(false);
        }
    }

    //상점 열기
    private void Interaction()
    {
        if (canTalking == true && doTalking == false && Input.GetKeyDown(KeyCode.F))
        {
            TalkingBox.SetActive(false);
            doTalking = true;
            anim.SetBool("Look", true);
            NPC_Camera_Swap(0);

            switch (type)
            {
                case NPC_Num.Food:
                    ShopManager.inst.F_Open_Shop(0, true);
                    break;

                case NPC_Num.Weapon:
                    ShopManager.inst.F_Open_Shop(1, true);
                    break;

            }
            //상점창 열기
        }
    }

    public void F_CloseShop()
    {
        TalkingBox.SetActive(true);
        doTalking = false;
        anim.SetBool("Look", false);
        NPC_Camera_Swap(1);
        
        switch (type)
        {
            case NPC_Num.Food:
                talking.F_Talk_NPC("또 오세요!");
                break;

            case NPC_Num.Weapon:
                transform.position = OriginPos;
                transform.eulerAngles = OriginRot;
                talking.F_Talk_NPC("감사합니다!");
                break;
        }
    }
    private void LookAtPlayer()
    {
        if (doTalking == true && gameObject.name == "Town_NPC_1")
        {
            LookPoint.rotation = Quaternion.Euler(0, LookPoint.rotation.y, 0);
            
            transform.LookAt(LookPoint);
        }

    }
    /// <summary>
    /// 0 상점 / 1 상점 끄기
    /// </summary>
    /// <param name="value"></param>
    private void NPC_Camera_Swap(int value)
    {
        switch (value)
        {
            case 0:
                TownCam.gameObject.SetActive(false);
                NpcCam.gameObject.SetActive(true);
                break;

            case 1:
                TownCam.gameObject.SetActive(true);
                NpcCam.gameObject.SetActive(false);
                break;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Player_Obj == null)
        {
            Player_Obj = other.gameObject;
            TalkingBox.SetActive(true);
            canTalking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Player_Obj != null)
        {
            Player_Obj = null;
            TalkingBox.SetActive(false);
            canTalking = false;
        }
    }
}
