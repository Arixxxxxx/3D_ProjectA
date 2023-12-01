using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMovement : MonoBehaviour
{
    public enum EnemyType
    {
        A, Zombie, C
    }
    public EnemyType enemyType;

    EnemyStats stats;
    protected NavMeshAgent nav;
    protected ScanPlayers scanColl;
    protected Animator anim;
    protected Vector3 StartVec;
    [Header("# Enemy Cur Stats")]

    public List<ParticleSystem> enemyAttackPs = new List<ParticleSystem>();
    [SerializeField] protected float comeBackDis;
    [SerializeField] protected bool doReturnStartPos;
    [SerializeField] protected bool waitEnemy;
    [SerializeField] protected bool doFollowTarget;
    [SerializeField] protected bool doAttack;
    [SerializeField] protected float targetDis;
    bool once1;
    bool MoveStop;

    [SerializeField] protected GameObject Target;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
    }
    private void Start()
    {
        scanColl = GetComponentInChildren<ScanPlayers>();
        StartVec = transform.position;
        stats = GetComponent<EnemyStats>();
        waitEnemy = true;
    }
    private void Update()
    {
        MoveStop = stats.IsDead;

        if (!MoveStop) 
        {
            TargetOnOff();
            UpdatingTarget();
            EnemyMove();
            ComeBackStartComplete();
            TargetDis();
            AnimationCenter();
        }
        

    }

    Vector3 waitTimeRandomVec;
    bool once;
    private void UpdatingTarget()
    {
        Target = scanColl.F_GetFindOBj();
    }
    private void EnemyMove()
    {
        if (doFollowTarget && !doAttack)
        {
            nav.speed = 3;
            nav.stoppingDistance = 3;
            nav.SetDestination(Target.transform.position);
        }

        if (doReturnStartPos && !doAttack)
        {
            nav.SetDestination(StartVec);//saa
        }

        if (waitEnemy && nav.velocity == Vector3.zero && waitTimeRandomVec == Vector3.zero && !once)
        {
            once = true;
            nav.stoppingDistance = 0;
            StartCoroutine(WaitMove());

        }
    }

    IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(3);
        nav.speed = 1;

        waitTimeRandomVec = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        nav.SetDestination(waitTimeRandomVec);


        while (nav.remainingDistance < 0.1f)
        {
            yield return null;
        }
        waitTimeRandomVec = Vector3.zero;
        once = false;
    }
    private void TargetOnOff()
    {
        if (Target != null)
        {
            doFollowTarget = true;
            doReturnStartPos = false;
        }
        else if (Target == null)
        {
            doFollowTarget = false;
            ComeBackStartComplete();
        }
    }


    private void ComeBackStartComplete()
    {
        if (doFollowTarget == false)
        {
            comeBackDis = Vector3.Distance(StartVec, transform.position);

            if (comeBackDis < 3f)
            {
                doReturnStartPos = false;
                waitEnemy = true;
            }
            else if (comeBackDis > 3f)
            {
                doReturnStartPos = true;
                waitEnemy = false;
            }
        }

    }
    private void AnimationCenter()
    {
        if (nav.velocity != Vector3.zero)
        {
            waitEnemy = false;
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

    }

    //공격 관련

    private void TargetDis()
    {
        if (!doFollowTarget) { return; }
        if (Target != null)
        {
            targetDis = Vector3.Distance(transform.position, Target.transform.position);
        }


        if (nav.remainingDistance <= nav.stoppingDistance && !once1)
        {
            once1 = true;
            ThinkEnemy();
        }
    }
    private void ThinkEnemy()
    {
        int Pattan = Random.Range(0, 1);
        switch (enemyType)
        {


            case EnemyType.A:

                switch (Pattan)
                {
                    case 0:
                        StartCoroutine(Taunt());
                        StartCoroutine(AttackPSOn());
                        break;


                }
           break;



            case EnemyType.Zombie:
                Debug.Log("공격");

                break;

        }
        
    }
    [SerializeField] float PsTyming_1;
    IEnumerator AttackPSOn()
    {
        yield return new WaitForSeconds(PsTyming_1);


        enemyAttackPs[0].gameObject.SetActive(true);
        enemyAttackPs[0].Play();

        while (enemyAttackPs[0].isPlaying)
        {
            yield return null;
        }

        enemyAttackPs[0].Stop();
        enemyAttackPs[0].gameObject.SetActive(false);
    }
    IEnumerator Taunt()
    {
        doAttack = true;
        anim.SetBool("Attack", true);

        yield return new WaitForSeconds(0.5f);

        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.98f)
        {
            yield return null;
        }

        anim.SetBool("Attack", false);
        anim.SetBool("Combat_Idle", true);

        yield return new WaitForSeconds(1);

        anim.SetBool("Combat_Idle", false);
        doAttack = false;
        once1 = false;
    }

   private void A_GameObjectFalse()
    {
        Invoke("off", 2);
    }
    private void off()
    {
        gameObject.SetActive(false);
    }
}
