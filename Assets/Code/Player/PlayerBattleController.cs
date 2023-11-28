using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleController : MonoBehaviour
{
    PlayerMoveController playerMoveCon;
    AnimationContoller anim;
    EnemyScanColliderSC scanSc;


    [Header("# Attack Dleay Settings")]
    [Space]
    [SerializeField] float rangeBoomAttackDleay;
    public float RangeBoomAttackDleay { get { return rangeBoomAttackDleay; } }
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
    [SerializeField] GameObject[] RangeWeapenSet;
    [SerializeField] int bulletEA;
    [SerializeField] Image CrossHair;
    [SerializeField] Image[] bulletBublle;



    bool leftClick;
    bool rightClick;
    bool tapClick;
    bool isTargetingOnOff;
    bool isAttackColliderEnable;


    public bool IsAttackColliderEnagle { set { isAttackColliderEnable = value; } }

    bool isAttatking;


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
        RangeModeAimModeOnOff();
        ScrrenUI_BulletEA_Image_Updater();
        ScrrenUI_BulletEA_Image_ColorA_Updater();
        GameManager.Inst.F_ModeChangeNo(isAttackColliderEnable);
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
        rightClick = Input.GetMouseButtonDown(1);
        tapClick = Input.GetKeyDown(KeyCode.Tab);

    }
    bool attackonce;

    private void MeleeAttack()
    {
        if (anim.MeleeAttackNum >= 4) { return; }

        if ((curModeValue == 1 || curModeValue == 3) && leftClick)
        {
            
            anim.F_MeleeAttack();
        }
    }


    // 공격레이어 활성화
    public IEnumerator AttackColliderActive()
    {

        yield return new WaitForSeconds(FirstAttackDelayTime);

        attackCollider.enabled = true;

        yield return new WaitForSeconds(AttackActiveDurationTime);

        attackCollider.enabled = false;

    }

    private void ScrrenUI_BulletEA_Image_Updater()
    {
        if (CrossHair.gameObject.activeSelf == true)
        {
            for (int i = 0; i < bulletBublle.Length; i++)
            {
                bulletBublle[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < bulletBublle.Length; i++)
            {
                bulletBublle[i].gameObject.SetActive(false);
            }
        }
    }

    private void ScrrenUI_BulletEA_Image_ColorA_Updater()
    {
        if (CrossHair.gameObject.activeSelf == true)
        {
            switch (bulletEA)
            {
                case 0:
                    for (int i = 0; i < bulletBublle.Length; i++)
                    {
                        bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 0.5f);
                    }
                    break;

                case 1:
                    for (int i = 0; i < bulletBublle.Length; i++)
                    {
                        if (i < 1)
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 1f);
                        }
                        else
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 0.5f);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < bulletBublle.Length; i++)
                    {
                        if (i < 2)
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 1f);
                        }
                        else
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 0.5f);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < bulletBublle.Length; i++)
                    {
                        if (i < 3)
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 1f);
                        }
                        else
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 0.5f);
                        }
                    }
                    break;
                case 4:
                    for (int i = 0; i < bulletBublle.Length; i++)
                    {
                        if (i < 4)
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 1f);
                        }
                        else
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 0.5f);
                        }
                    }
                    break;
                case 5:
                    for (int i = 0; i < bulletBublle.Length; i++)
                    {
                        {
                            bulletBublle[i].color = new Color(bulletBublle[i].color.r, bulletBublle[i].color.g, bulletBublle[i].color.b, 1f);
                        }
                    }
                    break;
            }
        }
        else
        {

        }
    }
    private void RangeModeAimModeOnOff()
    {
        if (rightClick == true && curModeValue == 2)
        {
            anim.F_AimModeAnimationOnOFF();
            playerMoveCon.F_ModeSelect("aim");
            CameraManager.inst.F_ChangeCam(1);
            CrossHair.gameObject.SetActive(true);

        }
        else if (rightClick == true && curModeValue == 4)
        {
            F_OffAimMode(0);
        }
    }

    public void F_OffAimMode(int value)
    {
        switch (value)
        {
            case 0:
                playerMoveCon.F_AimModeOff_NoParticle_Funtion();
                break;

            case 1:
                playerMoveCon.F_ModeSelect("melee");
                break;
        }

        anim.F_AimModeAnimationOnOFF();

        CameraManager.inst.F_ChangeCam(0);
        CrossHair.gameObject.SetActive(false);
    }
    private int beforeModeNum;
    private void TargetingMode()
    {
        if (tapClick && curModeValue == 1 && nearTarget != null)
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
        else if (tapClick && curModeValue == 3)
        {
            isTargetingOnOff = false;
            if (selectTarget.transform.Find("HpBar/Target") != null)
            {
                StartCoroutine(LockOnOff());
            }
            selectTarget = null;
        }



        if (isTargetingOnOff && tapClick)
        {
            beforeModeNum = curModeValue;
            playerMoveCon.F_ModeSelect("targeting");
        }

        else if (!isTargetingOnOff && tapClick)
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
        if (selectTarget != null)
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
    /// <param name="value"> 밀리 0= 끄기 / 1 = 켜기 / 2 = 원거리 켜기 </param>
    public void F_Set_CurModeWeapon(int value)
    {
        switch (value)
        {
            case 0:
                for (int i = 0; i < MeleeModeWeapenSet.Length; i++)
                {
                    MeleeModeWeapenSet[i].SetActive(false);
                }

                RangeWeapenSet[0].SetActive(false);

                break;
            case 1:
                for (int i = 0; i < MeleeModeWeapenSet.Length; i++)
                {
                    MeleeModeWeapenSet[i].SetActive(true);
                }
                RangeWeapenSet[0].SetActive(false);

                break;

            case 2:
                for (int i = 0; i < MeleeModeWeapenSet.Length; i++)
                {
                    MeleeModeWeapenSet[i].SetActive(false);
                }

                RangeWeapenSet[0].SetActive(true);

                break;

            case 3:
                for (int i = 0; i < MeleeModeWeapenSet.Length; i++)
                {
                    MeleeModeWeapenSet[i].SetActive(false);
                }

                RangeWeapenSet[0].SetActive(false);

                break;
        }
    }
    public bool isAimOn()
    {
        return CrossHair.gameObject.activeSelf;
    }

    public void F_useBullet()
    {
        bulletEA--;
    }
}
