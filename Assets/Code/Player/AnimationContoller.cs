using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContoller : MonoBehaviour
{
    Animator anim;

    PlayerMoveController player;
    PlayerBattleController battleSc;
    GameManager Gm;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform BulletStartPoint;
    [Header("#Particle System & AavarterMask")]
    [Space]
    [SerializeField] List<ParticleSystem> comboAttackPs = new List<ParticleSystem>();
    [SerializeField] List<AvatarMask> avaterList = new List<AvatarMask>();
    [SerializeField] Animator[] con;
    [Space]
    [Header("#Cheking Stats")]
    [Space]
    [SerializeField] float verSpeedValue;
    [SerializeField] float horiSpeedValue;
    [SerializeField] float SprintValue;
    [SerializeField] float parameter_VerticalValue;
    [SerializeField] float parameter_HorizontalValue;
    bool _PushDownSpacebar;
    [SerializeField] bool _PushVDown;
    [SerializeField] bool _PushHDown;
    [SerializeField] bool _PushLshiftDown;
    [SerializeField] bool _Push1KeyDown;
    [SerializeField] bool isLeftClick;
    [SerializeField] int curModeValue;
    [SerializeField] bool isDodge;
    [Header("#Shooting Effect")]
    [Space]
    [SerializeField] Light gunShotLight;
    [SerializeField] ParticleSystem[] gunShotPs;
    [Header("#Shooting Stats")]
    [Space]
    [SerializeField] float shotRayDistance;
    [SerializeField] float shotPower;
    bool isAttackStart;
    [SerializeField] bool aimOn;
    bool isInWater;
    [Header("# Window Cheak")]
    [Space]
    [SerializeField] bool isWindowPopUp;

    public void SetWindowPopUp(bool _value)
    {
        isWindowPopUp = _value;
    }

    public bool IsAttackStart { get { return isAttackStart; } }
    public bool Isdodge { get { return isDodge; } set { isDodge = value; } }
    Camera mainCam;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMoveController>();
        battleSc = GetComponent<PlayerBattleController>();
    }
    private void Start()
    {
        mainCam = Camera.main;
        Gm = GameManager.Inst;
        Gm.SetAction((bool value) =>
        {
            SetWindowPopUp(value);
        });
    }
    private void Update()
    {
        if (isWindowPopUp == true) return;
        
        CheakInput();
        AimOnAnimation();
        MathfValueFuntion();
        ApllyAnimator();
        AnimateDodge();
        curModeValue = player.F_GetPlayerAttackModeNum();
        isInWater = player.IsinWater;
        aimOn = battleSc.isAimOn();

        CheakAttackPahse();
        UpperAvatarMaskWeightChanger();
        Swim_AnimationConllor();
    }

    bool layerWeightOnce;

    /// <summary>
    /// animator layer Weight Changer
    /// </summary>
    /// <param name="value"> 레이어 번호 1/2</param>
    /// <param name="_value">true / false</param>
    public void F_Set_LayerWeight(int value, bool _value)
    {
        switch (value)
        {
            case 1:
                if (_value == true && !layerWeightOnce)
                {
                    StartCoroutine(AniLayer_Weight(1, true));
                }
                else if (_value == false && !layerWeightOnce)
                {
                    StartCoroutine(AniLayer_Weight(1, false));
                }
                break;

            case 2:
                if (_value == true && !layerWeightOnce)
                {
                    StartCoroutine(AniLayer_Weight(2, true));
                }
                else if (_value == false && !layerWeightOnce)
                {
                    StartCoroutine(AniLayer_Weight(2, false));
                }
                break;
        }

    }

    float weightFloat;
    IEnumerator AniLayer_Weight(int int_value, bool _value)
    {
        layerWeightOnce = true;

        switch (int_value)
        {
            case 1:

                switch (_value)

                {
                    case true:
                        weightFloat = 0;

                        while (anim.GetLayerWeight(1) < 1)
                        {
                            weightFloat += Time.deltaTime * 3.0f;
                            anim.SetLayerWeight(1, Mathf.Lerp(0, 1, weightFloat));
                            yield return null;
                        }

                        anim.SetLayerWeight(1, 1);
                        layerWeightOnce = false;
                        break;

                    case false:

                        weightFloat = 0;

                        while (anim.GetLayerWeight(1) > 0)
                        {
                            weightFloat += Time.deltaTime * 3.0f;
                            anim.SetLayerWeight(1, Mathf.Lerp(1, 0, weightFloat));
                            yield return null;
                        }
                        anim.SetLayerWeight(1, 0);
                        layerWeightOnce = false;
                        break;
                }

                break;

            case 2:
                switch (_value)

                {
                    case true:
                        weightFloat = 0;

                        while (anim.GetLayerWeight(2) < 1)
                        {
                            weightFloat += Time.deltaTime * 3.0f;
                            anim.SetLayerWeight(2, Mathf.Lerp(0, 1, weightFloat));
                            yield return null;
                        }

                        anim.SetLayerWeight(2, 1);
                        layerWeightOnce = false;
                        break;

                    case false:

                        weightFloat = 0;

                        while (anim.GetLayerWeight(2) > 0)
                        {
                            weightFloat += Time.deltaTime * 3.0f;
                            anim.SetLayerWeight(2, Mathf.Lerp(1, 0, weightFloat));
                            yield return null;
                        }
                        anim.SetLayerWeight(2, 0);
                        layerWeightOnce = false;
                        break;
                }

                break;


        }


    }



    [SerializeField] float inputMouseVertical;
    [SerializeField] float mouseVerticalSensevity;
    float aaaa;
    float _animVerfloat;
    private void CheakInput()
    {

        _Push1KeyDown = Input.GetKeyDown(KeyCode.Alpha1);

        verSpeedValue = Input.GetAxis("Vertical") * 0.5f;
        _PushVDown = Input.GetButton("Vertical");

        horiSpeedValue = Input.GetAxis("Horizontal") * 0.5f;
        _PushHDown = Input.GetButton("Horizontal");

        SprintValue = Input.GetAxis("Sprint") * 0.5f;
        _PushLshiftDown = Input.GetButton("Sprint");

        _PushDownSpacebar = Input.GetButtonDown("Jump");
        isLeftClick = Input.GetMouseButtonDown(0);

        //inputMouseVertical += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseVerticalSensevity

        inputMouseVertical = CameraManager.inst.F_1rd_Cam_VerticalValue();
        inputMouseVertical *= -0.01f;
        //inputMouseVertical = Mathf.Clamp(inputMouseVertical, -0.5f, 0.5f);

    }
    [SerializeField] bool isCharacterMove;
    [SerializeField] float CheakVelocity;
    bool ononon;
    private void UpperAvatarMaskWeightChanger()
    {
        CheakVelocity = Mathf.Abs(horiSpeedValue) + Mathf.Abs(verSpeedValue);

        if (CheakVelocity > 0)
        {
            isCharacterMove = true;
            ononon = false;
        }
        else
        {
            isCharacterMove = false;

        }

        if (isCharacterMove == true && curModeValue != 0)
        {
            anim.SetLayerWeight(2, 0);
        }
        else if (isCharacterMove == false && curModeValue != 0 && !ononon)
        {
            ononon = true;
            anim.SetLayerWeight(2, 1);
        }

        if (curModeValue == 0 && anim.GetLayerWeight(2) != 0)
        {
            anim.SetLayerWeight(2, 0);
        }
    }

    bool inputMouseVerInit;
    bool isboomAttackDleay;
    private void AimOnAnimation()
    {
        if (aimOn == true && curModeValue == 1 && anim.GetBool("AimMode") == true)
        {
            battleSc.F_OffAimMode(1);
        }

        if (aimOn == false && anim.GetLayerWeight(1) != 0 && curModeValue == 2)
        {
            anim.SetLayerWeight(1, 0);
        }
        if (aimOn == true)
        {
            if (anim.GetLayerWeight(1) != 1 && CheakVelocity > 0)
            {
                anim.SetLayerWeight(1, 1);
            }
            else if (anim.GetLayerWeight(1) != 0 && CheakVelocity == 0)
            {
                anim.SetLayerWeight(1, 0);
            }
            anim.SetFloat("InputMouseVertical", inputMouseVertical);

            if (isLeftClick && Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, shotRayDistance) && !isboomAttackDleay)
            {
                isboomAttackDleay = true;

                anim.SetTrigger("Shoot");
                CameraManager.inst.F_FireCameraZoonOutIn();
                StartCoroutine(ShotLightON());
                gunShotPs[0].Play();
                gunShotPs[1].Play();
                battleSc.F_useBullet();

                GameObject bullet_obj = Instantiate(Bullet, BulletStartPoint.position, transform.rotation, PoolManager.Inst.transform);
                bullet_obj.GetComponent<Rigidbody>().AddForce(mainCam.transform.forward * shotPower, ForceMode.Impulse);

                Invoke("BoomAttackDleay_False_Funtion", battleSc.RangeBoomAttackDleay);

            }
        }
    }
    private void BoomAttackDleay_False_Funtion()
    {
        isboomAttackDleay = false;
    }

    float shotLightIntensityValue;
    WaitForSeconds LightIntervalTime = new WaitForSeconds(0.3f);
    IEnumerator ShotLightON()
    {
        shotLightIntensityValue = 0;

        while (shotLightIntensityValue <= 10)
        {
            shotLightIntensityValue += Time.deltaTime * 20;

            gunShotLight.intensity = shotLightIntensityValue;

            yield return null;
        }
        gunShotLight.intensity = 0;
    }

    private void MathfValueFuntion()
    {

        if (_PushLshiftDown && _PushVDown && verSpeedValue > 0)
        {
            verSpeedValue += SprintValue;
        }
        else if (_PushLshiftDown && _PushHDown && verSpeedValue < 0)
        {
            verSpeedValue -= SprintValue;
        }

        if (_PushLshiftDown && _PushHDown && horiSpeedValue > 0)
        {
            horiSpeedValue += SprintValue;
        }
        else if (_PushLshiftDown && _PushHDown && horiSpeedValue < 0)
        {
            horiSpeedValue -= SprintValue;
        }

        parameter_VerticalValue = verSpeedValue;
        parameter_HorizontalValue = horiSpeedValue;



    }

    private void AnimateDodge()
    {
        if (curModeValue != 0)
        {
            if (_PushLshiftDown && (Mathf.Abs(horiSpeedValue) > 0.1f || Mathf.Abs(verSpeedValue) > 0.1f) && !isDodge)
            {
                isDodge = true;
                anim.SetTrigger("Dodge");
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f))
        {

            isDodge = false;
        }
    }

    private void ApllyAnimator()
    {
        if (isWindowPopUp)
        {
            parameter_VerticalValue = 0;
            parameter_HorizontalValue = 0;
            Debug.Log("11");
        }

        anim.SetFloat("Horizontal", parameter_HorizontalValue);
        anim.SetFloat("Vertical", parameter_VerticalValue);

        if (_PushDownSpacebar && player.IsGround == true && isInWater == false) { anim.SetTrigger("Jump"); }
    }

    public void F_PlayerCurMode(int value)
    {
        switch (value)
        {
            case 0:
                anim.SetBool("NormalMode", true);
                anim.SetBool("MeleeMode", false);
                anim.SetBool("RangeMode", false);
                break;

            case 1:
                anim.SetBool("MeleeMode", true);
                anim.SetBool("NormalMode", false);
                anim.SetBool("RangeMode", false);
                break;

            case 2:
                anim.SetBool("MeleeMode", false);
                anim.SetBool("NormalMode", false);
                anim.SetBool("RangeMode", true);
                break;
        }

    }
    [SerializeField] int meleeAttackNum;
    public int MeleeAttackNum { get { return meleeAttackNum; } }
    [SerializeField, Range(0f, 3f)] float meleeAttackResetTime;
    [SerializeField] float maxComboDelay;


    public static int noClick;
    bool attack1Once;
    public void F_MeleeAttack()
    {


        if (isDodge) { return; }


        meleeAttackNum++;

        if (CheakVelocity > 0 && meleeAttackNum > 0)
        {
            anim.SetLayerWeight(1, 1);
        }

        if (meleeAttackNum == 1 && attack1Once == false)
        {
            attack1Once = true;
            anim.SetInteger("MeleeAttackNum", 1);
            StartCoroutine(battleSc.AttackColliderActive());
            CameraManager.inst.F_FireCameraZoonOutIn();
            player.IsAttacking = true;
            StartCoroutine(ComboAttackParticle(0));




        }
    }
    [SerializeField] float particle_1_Delay;
    [SerializeField] float particle_2_Delay;
    [SerializeField] float Attack1EndTime;
    [SerializeField] float Attack2EndTime;
    /// <summary>
    /// 근접공격 파티클 실행
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerator ComboAttackParticle(int value)
    {
        switch (value)
        {
            case 0:
                comboAttackPs[0].gameObject.SetActive(true);

                yield return new WaitForSeconds(particle_1_Delay);

                comboAttackPs[0].Play();

                yield return new WaitForSeconds(Attack1EndTime);

                comboAttackPs[0].Stop();
                comboAttackPs[0].gameObject.SetActive(false);
                attack1Once = false;

                break;

            case 1:
                comboAttackPs[1].gameObject.SetActive(true);

                yield return new WaitForSeconds(particle_2_Delay);

                comboAttackPs[1].Play();

                yield return new WaitForSeconds(Attack2EndTime);

                comboAttackPs[1].Stop();
                comboAttackPs[1].gameObject.SetActive(false);
                attack1Once = false;

                break;

            case 2:
                comboAttackPs[2].gameObject.SetActive(true);

                yield return new WaitForSeconds(particle_2_Delay);

                comboAttackPs[2].Play();

                yield return new WaitForSeconds(Attack2EndTime);

                comboAttackPs[2].Stop();
                comboAttackPs[2].gameObject.SetActive(false);
                attack1Once = false;

                break;

            case 3:
                comboAttackPs[3].gameObject.SetActive(true);

                yield return new WaitForSeconds(particle_2_Delay);

                comboAttackPs[3].Play();

                yield return new WaitForSeconds(Attack2EndTime);

                comboAttackPs[3].Stop();
                comboAttackPs[3].gameObject.SetActive(false);
                attack1Once = false;

                break;
        }

    }


    [SerializeField] float nextAttackTyming;
    private void CheakAttackPahse()
    {
        if (curModeValue == 0 || curModeValue == 2) { return; }

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack1") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > nextAttackTyming)
        {
            if (meleeAttackNum >= 2)
            {
                anim.SetInteger("MeleeAttackNum", 2);
                StartCoroutine(ComboAttackParticle(1));
                StartCoroutine(battleSc.AttackColliderActive());
                CameraManager.inst.F_FireCameraZoonOutIn();

            }
            else if ((anim.GetCurrentAnimatorStateInfo(1).IsName("Attack1") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1f))
            {
                ResetMeleeAttackNum();
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack2") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > nextAttackTyming)
        {
            if (meleeAttackNum >= 3)
            {
                anim.SetInteger("MeleeAttackNum", 3);
                StartCoroutine(ComboAttackParticle(2));
                StartCoroutine(battleSc.AttackColliderActive());
                CameraManager.inst.F_FireCameraZoonOutIn();
            }
            else if ((anim.GetCurrentAnimatorStateInfo(1).IsName("Attack2") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1f))
            {
                ResetMeleeAttackNum();
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack3") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > nextAttackTyming)
        {
            if (meleeAttackNum >= 4)
            {
                anim.SetInteger("MeleeAttackNum", 4);
                StartCoroutine(ComboAttackParticle(3));
                StartCoroutine(battleSc.AttackColliderActive());
                CameraManager.inst.F_FireCameraZoonOutIn();
            }
            else if ((anim.GetCurrentAnimatorStateInfo(1).IsName("Attack3") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1f))
            {
                ResetMeleeAttackNum();
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack4") && anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1f)
        {
            ResetMeleeAttackNum();
        }
    }

    private void ResetMeleeAttackNum()
    {
        anim.SetLayerWeight(1, 0);
        meleeAttackNum = 0;
        anim.SetInteger("MeleeAttackNum", 0);
        battleSc.IsAttackColliderEnagle = false;
        player.IsAttacking = false;
        attack1Once = false;
    }

    bool doAimModeOnOff = false;
    public void F_AimModeAnimationOnOFF()
    {
        doAimModeOnOff = !doAimModeOnOff;
        anim.SetBool("AimMode", doAimModeOnOff);
    }



    [SerializeField] float _IkDownDis;
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down, out RaycastHit hit, _IkDownDis + 1.0f, LayerMask.GetMask("Ground")))
        {
            Vector3 HitPos = hit.point;
            HitPos.y += _IkDownDis;
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, HitPos);
        }

        if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down, out RaycastHit hit2, _IkDownDis + 1.0f, LayerMask.GetMask("Ground")))
        {
            Vector3 HitPosR = hit2.point;
            HitPosR.y += _IkDownDis;
            anim.SetIKPosition(AvatarIKGoal.RightFoot, HitPosR);
        }


        //if (aimOn == true)
        //{
        //    anim.SetLookAtWeight(1.0f);

        //    화면 중앙을 메인 카메라의 월드 좌표로 변환
        //   Vector3 screenCenter = Camera.main.transform.position;
        //    screenCenter.y *= -1;
        //    screenCenter.z *= -1;

        //    anim.SetLookAtPosition(screenCenter);
        //}
        //else
        //{
        //    anim.SetLookAtWeight(0f);
        //}
    }

    private void Swim_AnimationConllor()
    {
        if (isInWater == true && anim.GetBool("Swim") == false)
        {
            anim.SetBool("Swim", isInWater);
        }
        else if (isInWater == false && anim.GetBool("Swim") == true)
        {
            anim.SetBool("Swim", isInWater);
        }
    }

}
