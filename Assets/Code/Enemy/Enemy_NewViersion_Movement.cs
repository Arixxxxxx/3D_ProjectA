using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_NewViersion_Movement : MonoBehaviour
{
    public enum MoveType { Patrol, Attaker}
    public MoveType type;

    PlayerMoveController player;
    NavMeshAgent nav;
    Animator anim;
    [Header("# Insert Object or Component")]
    [Space]
    [SerializeField] BoxCollider Enemy_AttackCollider;
    [Header("# View Info")]
    [Space]
    [SerializeField] float distance;
    [SerializeField] float notAuto_Distance;
    [SerializeField] float startPos_Distance;
    [Space]
    [SerializeField] bool comback_StartPos;
    [SerializeField] bool doWait;
    [SerializeField] bool doFollow;
    [SerializeField] bool isCanAttack;
    [SerializeField] bool doAttack;
    [SerializeField] bool doScream;
    [SerializeField] bool Auto;
    [Space]
    [SerializeField] bool isDead;

    [Space]
    [Header("# Animation Info")]
    [Space]
    [SerializeField] float animation_Speed_Value;
    Vector3 startPos;
    Vector3 playerPos;
    Vector3 LookatPos;
    Vector3 lookAtSavePos;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        player = PlayerMoveController.Inst;
        startPos = transform.position;
        Auto = true;
        if (isDead == true) { isDead = false; }
    }

    private void Start()
    {
        player = PlayerMoveController.Inst;
        startPos = transform.position;
        Auto = true;
        if(isDead == true) { isDead = false; }
    }

    private void Update()
    {
        if (isDead) { return; }

        Target_Enemy_Distance_Update(); // 거리계산
        StartPosToCurPos_DistanceUpdate(); // 시작지점 계산
        Enemy_Move_Type_Controll(); // 움직임 판단

        Enemy_Move(); // 행동

        //애니메이션
        Animation_SpeedValue_Controll(); // 속도 Float값 계산
        Animation_Controll_Funtion(); // Animator Parameter 제어
    }

    
    //거리측정
    private void Target_Enemy_Distance_Update()
    {
        playerPos = player.F_Get_PlayerCurPos();
        if (Auto)
        {
            distance = Vector3.Distance(playerPos, transform.position);
        }
        else
        {
            notAuto_Distance = Vector3.Distance(playerPos, transform.position);

            if (notAuto_Distance <= 5)
            {
                Auto = true;
                return;
            }
            else if (notAuto_Distance > 5)
            {
                distance = 3;
            }

        }
    }

    private void StartPosToCurPos_DistanceUpdate()
    {
        startPos_Distance = Vector3.Distance(startPos, transform.position);
    }

    bool startOk;
    // private 거리에 따른 bool 변수 조절
    private void Enemy_Move_Type_Controll()
    {
        if (doWait && startPos_Distance > 0.1f && Auto)
        {
            comback_StartPos = true;
            doWait = false;
        }
        else if (comback_StartPos && startPos_Distance <= 0.1f && Auto)
        {
            comback_StartPos = false;
        }

        if (distance > 10)
        {
            doWait = true;
            doFollow = false;
            isCanAttack = false;
            doScream = false;
            startOk = false;
            nav.speed = 1.5f;
        }
        else if (distance < 10 && distance >= 6)
        {
            if (doScream == false)
            {
                StartCoroutine(ZombieScream_AttackStart());
            }

            if (startOk == true)
            {
                doWait = false;
                doFollow = true;
                isCanAttack = false;
                nav.speed = 1.5f;
            }

        }
        else if (distance < 6 && distance >= 2 && nav.speed != 2.5f) // 거리가까워지면 속도증가
        {
            nav.speed = 2.5f;
        }
        else if (distance < 2)
        {
            doWait = false;
            doFollow = false;
            isCanAttack = true;

        }
    }

    IEnumerator ZombieScream_AttackStart()
    {
        doScream = true;
        transform.LookAt(playerPos);
        anim.SetTrigger("scream");
        yield return new WaitForSeconds(2.25f);
        startOk = true;
    }
    //움직임
    private void Enemy_Move()
    {
        if (doFollow && !doAttack) // 타겟선정시
        {
            nav.SetDestination(playerPos);
        }

        if (isCanAttack == true && doAttack == false) // 공격가능거리도달시
        {
            nav.velocity = Vector3.zero;
            LookatPos = playerPos;

            transform.LookAt(LookatPos);
        }

        if(doAttack == true)
        {
            nav.velocity = Vector3.zero;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Z_Scream!"))
        {
            transform.LookAt(playerPos);
        }

        if (comback_StartPos == true) // 스타트포인트로 귀환
        {
            nav.SetDestination(startPos);
        }
    }

    //애니메이션
    private void Animation_SpeedValue_Controll()
    {
        if (distance >= 6 && animation_Speed_Value < 0.5f) // 거리가 6보다 멀때
        {
            animation_Speed_Value = 0.5f;
        }

        else if (distance > 6 && animation_Speed_Value > 0.5f) // 가까워졋다가 멀어질때
        {
            animation_Speed_Value -= Time.deltaTime * 0.5f;
        }

        else if (distance < 6 && animation_Speed_Value < 1) // 가까워졌을때
        {
            animation_Speed_Value += Time.deltaTime * 0.5f;
        }
    }

    private void Animation_Controll_Funtion()
    {
        if (nav.velocity == Vector3.zero)
        {
            anim.SetBool("Run", false);
        }
        else if (nav.velocity != Vector3.zero)
        {
            anim.SetBool("Run", true);
            anim.SetFloat("Velo", animation_Speed_Value);
        }

        if (isCanAttack == true && doAttack == false)
        {
            StartCoroutine(Attack_Anim_And_Motion());
        }
    }
    IEnumerator Attack_Anim_And_Motion()
    {
        doAttack = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(3f);
        Enemy_AttackCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        Enemy_AttackCollider.enabled = false;
        yield return new WaitForSeconds(1.2f);
        doAttack = false;
        isCanAttack = false;
        doFollow = true;


    }

    //원거리 공격받앗을시 강제 공격 선택
    public void F_isAutoAttackFalse()
    {
        if (distance > 10 && Auto == true)
        {
            Auto = false;
            doFollow = true;
            doWait = false;
        }
    }

    public void F_Dead()
    {
        isDead = true;
    }
}
