using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour
{
    PlayerMoveController playerMoveCon;
    AnimationContoller anim;
    EnemyScanColliderSC scanSc;



    [Header("# Targeting Stat")]
    [Space]
    [SerializeField] GameObject nearTarget;
    [SerializeField] GameObject selectTarget;
    [Space]
    [Header("# CurMode Value")]
    [Space]
    [SerializeField] int curModeValue;
    [Space]
    [Header("# Weapone GameObject in Hierarchy")]
    [Space]
    [SerializeField] BoxCollider attackCollider;
    [SerializeField] float FirstAttackDelayTime;
    [SerializeField] float AttackActiveDurationTime;
    [Space]
    [SerializeField] GameObject[] MeleeModeWeapenSet;
   
    
    
    bool leftClick;
    bool rightClick;
    bool tapClick;
    bool isTargetingOnOff;
      
    private void Start()
    {
        playerMoveCon = GetComponent<PlayerMoveController>();
        anim = GetComponent<AnimationContoller>();
        scanSc = transform.GetComponentInChildren<EnemyScanColliderSC>();
    }

    private void Update()
    {
        curModeValue = playerMoveCon.F_GetPlayerAttackModeNum();
        inputMode();
        MeleeAttack();
        WeapneBugCheak();
        Get_NearTargetInScanCollider();
        TargetingMode();
    }

    private void WeapneBugCheak()
    {
        if (curModeValue == 0 && MeleeModeWeapenSet[0].gameObject.activeSelf)
        {
            F_Set_CurModeWeapon(0);
        }
    }

    private void inputMode()
    {
        leftClick = Input.GetMouseButtonDown(0);
        tapClick = Input.GetKeyDown(KeyCode.Tab);

    }
    bool attackonce;

    private void MeleeAttack()
    {
        if ((curModeValue == 1 || curModeValue == 3) && leftClick)
        {
            anim.F_MeleeAttack();
           
            if (attackonce == false)
            {
                StartCoroutine(AttackColliderActive());  
            }
        }
    }

    // 공격레이어 활성화
    IEnumerator AttackColliderActive()
    {
        attackonce = true;
        yield return new WaitForSeconds(FirstAttackDelayTime);

        attackCollider.enabled = true;

        yield return new WaitForSeconds(AttackActiveDurationTime);

        attackCollider.enabled = false;
        attackonce = false;
    }
    
    private int beforeModeNum;
    private void TargetingMode()
    {
        if(tapClick && curModeValue == 1  && nearTarget != null) 
        {
            isTargetingOnOff = true;

            if (nearTarget.gameObject.activeSelf == false) { return; }
            
                selectTarget = nearTarget;

                if (selectTarget.transform.Find("HpBar/Target") != null)
                {
                    selectTarget.transform.Find("HpBar/Target").gameObject.SetActive(true);
                    selectTarget.transform.Find("HpBar/Target").GetComponent<Animator>().SetTrigger("In");
                }
            
           
                
        }
        else if( tapClick && curModeValue == 3 ) 
        {
            isTargetingOnOff = false;
            if(selectTarget.transform.Find("HpBar/Target") != null)
            {
                StartCoroutine(LockOnOff());
            }
            selectTarget = null;
        }

        
        
        if(isTargetingOnOff && tapClick)
        {
            beforeModeNum = curModeValue;
            playerMoveCon.F_ModeSelect("targeting");
        }

        else if(!isTargetingOnOff && tapClick)
        {
            switch (beforeModeNum)
            {
                case 0:
                    playerMoveCon.F_ModeSelect("normal");
                    break;
                    case 1:
                    playerMoveCon.F_ModeSelect("melee");
                    break;
                    case 2:
                    playerMoveCon.F_ModeSelect("range");
                    break;
                    
            }

            beforeModeNum = 0;
        }
        
    }
    IEnumerator LockOnOff()
    {
        Animator an = selectTarget.transform.Find("HpBar/Target").GetComponent<Animator>();
        an.SetTrigger("Out");

        yield return null;

        while (an.GetCurrentAnimatorStateInfo(0).IsName("AimOut") && an.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        an.gameObject.SetActive(false);
        
    }

    private void Get_NearTargetInScanCollider()
    {
        nearTarget = scanSc.F_GetNearTarget();
    }

    public GameObject F_Get_SelectTarget()
    {
        if(selectTarget != null)
        {
            return selectTarget;
        }
        else
        {
            return null;
        }
        
    }


    /// <summary>
    /// 무기 형상 끄기 / 키기
    /// </summary>
    /// <param name="value"> 0= 끄기 / 1 = 켜기</param>
    public void F_Set_CurModeWeapon(int value)
    {
        switch (value)
        {
            case 0:
                for (int i = 0; i < MeleeModeWeapenSet.Length; i++)
                {
                    MeleeModeWeapenSet[i].SetActive(false);
                }
                break;
            case 1:
                for (int i = 0; i < MeleeModeWeapenSet.Length; i++)
                {
                    MeleeModeWeapenSet[i].SetActive(true);
                }
                break;
        }
    }

}
