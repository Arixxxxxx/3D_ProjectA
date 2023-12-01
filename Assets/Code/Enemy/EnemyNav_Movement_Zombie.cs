using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyNavMovement;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyNav_Movement_Zombie : MonoBehaviour
{
    EnemyStats stats;
    protected NavMeshAgent nav;
    protected ScanPlayers scanColl;
    protected Animator anim;
    protected Vector3 StartVec;
    [Header("# Enemy Cur Stats")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float curSpeed;

    public List<ParticleSystem> enemyAttackPs = new List<ParticleSystem>();
    [Header("# Enemy Cur What do?")]
    [SerializeField] protected float comeBackDis;
    [SerializeField] protected bool doReturnStartPos;
    [SerializeField] protected bool waitEnemy;
    [SerializeField] protected bool doFollowTarget;
    [SerializeField] protected bool doAttack;
    [SerializeField] protected float targetDis;
    bool once1;
    bool MoveStop;
    bool isCanAttack;
    [SerializeField] bool doScream;

    [SerializeField] protected GameObject Target;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        maxSpeed = nav.speed;

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

        if (isCanAttack)
        {
            transform.LookAt(Target.transform.position);
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
        if (doScream)
        {
            transform.LookAt(Target.transform.position);
            return;
        }


        if (doFollowTarget && !doAttack && doScream == false)
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
        if (doScream) { waitTimeRandomVec = Vector3.zero; }
        nav.SetDestination(waitTimeRandomVec);


        while (nav.remainingDistance < 0.1f)
        {
            yield return null;
        }
        waitTimeRandomVec = Vector3.zero;
        once = false;
    }

    bool once2;
    private void TargetOnOff()
    {
        if (Target != null && once2 == false)
        {
            once2 = true;
            StartCoroutine(TargetSurche_Doing());
        }
        else if (Target == null)
        {
            doFollowTarget = false;
            ComeBackStartComplete();
            once2 = false;
        }


    }

    WaitForSeconds screamTime = new WaitForSeconds(2f);
    private IEnumerator TargetSurche_Doing()
    {

        doScream = true;
        anim.SetTrigger("scream");

        yield return screamTime;

        doScream = false;
        doFollowTarget = true;
        doReturnStartPos = false;

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
        if (nav.velocity == Vector3.zero)
        {
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
            if (nav.speed == 1)
            {
                curSpeed = 0.5f;
                anim.SetFloat("Velo", curSpeed);
            }
            else if (nav.speed == 3)
            {
                curSpeed = 1f;
                anim.SetFloat("Velo", curSpeed);

            }
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
            isCanAttack = true;
            //여기서 공격신호
        }
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
