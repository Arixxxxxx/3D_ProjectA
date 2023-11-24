using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContoller : MonoBehaviour
{
    Animator anim;
    PlayerMoveController player;

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
    [SerializeField] int curModeValue;
    [SerializeField] bool isDodge;

    public bool Isdodge { get { return isDodge; } set { isDodge = value; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMoveController>();
    }

    private void Update()
    {
       
        CheakInput();
        MathfValueFuntion();
        ApllyAnimator();
        AnimateDodge();
        curModeValue = player.F_GetPlayerAttackModeNum();

        CheakAttackPahse();
        UpperAvatarMaskWeightChanger();
    }

    bool layerWeightOnce;
    public void F_Set_LayerWeight(int value, bool _value)
    {
        switch (value)
        {
            case 1:
                if (_value == true && !layerWeightOnce)
                {
                    StartCoroutine(AniLayer1_Weight(true));
                }
                else if (_value == false && !layerWeightOnce)
                {
                    StartCoroutine(AniLayer1_Weight(false));
                }
                break;
        }

    }

    float weightFloat;
    IEnumerator AniLayer1_Weight(bool _value)
    {
        layerWeightOnce = true;

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


    }
    private void CheakInput()
    {
        verSpeedValue = Input.GetAxis("Vertical") * 0.5f;
        _PushVDown = Input.GetButton("Vertical");

        horiSpeedValue = Input.GetAxis("Horizontal") * 0.5f;
        _PushHDown = Input.GetButton("Horizontal");

        SprintValue = Input.GetAxis("Sprint") * 0.5f;
        _PushLshiftDown = Input.GetButton("Sprint");

        _PushDownSpacebar = Input.GetButtonDown("Jump");
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

        if(isCharacterMove == true && (curModeValue == 1 || curModeValue == 3))
        {
            anim.SetLayerWeight(2, 0);
        }
        else if(isCharacterMove == false && (curModeValue == 1 || curModeValue == 3) && !ononon)
        {
            ononon = true;
            anim.SetLayerWeight(2, 1);
        }
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
        anim.SetFloat("Horizontal", parameter_HorizontalValue);
        anim.SetFloat("Vertical", parameter_VerticalValue);

        if (_PushDownSpacebar && player.IsGround == true) { anim.SetTrigger("Jump"); }
    }

    public void F_PlayerCurMode(int value)
    {
        switch (value)
        {
            case 0:
                anim.SetBool("NormalMode", true);
                anim.SetBool("MeleeMode", false);
                break;

            case 1:
                anim.SetBool("MeleeMode", true);
                anim.SetBool("NormalMode", false);
                break;
        }

    }
    [SerializeField] int meleeAttackNum;
    [SerializeField, Range(0f, 3f)] float meleeAttackResetTime;
    [SerializeField] float maxComboDelay;


    public static int noClick;
    bool attack1Once;
    public void F_MeleeAttack()
    {
        if (isDodge) { return; }

        meleeAttackNum++;

        if(CheakVelocity > 0 && meleeAttackNum > 0)
        {
            anim.SetLayerWeight(1, 1);
        }
        
        if (meleeAttackNum == 1)
        {
            anim.SetInteger("MeleeAttackNum", 1);
            if (attack1Once == false)
            {
                attack1Once = true;
                StartCoroutine(ComboAttackParticle(0));
            }



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
    }
}
