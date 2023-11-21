using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour
{
    PlayerMoveController playerMoveCon;
    AnimationContoller anim;
    EnemyScanColliderSC scanSc;



    [SerializeField] GameObject selectTarget;
    [SerializeField] int curModeValue;
    [SerializeField] GameObject[] MeleeModeWeapenSet;
    [SerializeField] bool leftClick;
    [SerializeField] bool rightClick;
    [SerializeField] bool tapClick;
    [SerializeField] bool isTargetingOnOff;
      
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
        SelectTarget();
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

    private void MeleeAttack()
    {
        if ((curModeValue == 1 || curModeValue == 3) && leftClick)
        {
            anim.F_MeleeAttack();
        }
    }

    
    private int beforeModeNum;
    private void TargetingMode()
    {
        if(tapClick && curModeValue == 1 ) 
        {
            isTargetingOnOff = true; 
        }
        else if( tapClick && curModeValue == 3 ) 
        {
            isTargetingOnOff = false; 
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

    private void SelectTarget()
    {
        selectTarget = scanSc.F_GetNearTarget();
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
