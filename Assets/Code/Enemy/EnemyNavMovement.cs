using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMovement : MonoBehaviour
{
    public enum EnemyType
    {
        A,B,C
    }
    public EnemyType enemyType;

    protected NavMeshAgent nav;
    protected ScanPlayers scanColl;
    protected Animator anim;
    protected Vector3 StartVec;
    [Header("# Enemy Cur Stats")]

    [SerializeField] protected float comeBackDis;
    [SerializeField] protected bool doReturnStartPos;
    [SerializeField] protected bool waitEnemy;
    [SerializeField] protected bool doFollowTarget;
    [SerializeField] protected bool doAttack;
    [SerializeField] protected float targetDis;
    bool once1;

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
        waitEnemy = true;
    }
    private void Update()
    {
        TargetOnOff();
        UpdatingTarget();
        EnemyMove();
        ComeBackStartComplete();
        TargetDis();
        AnimationCenter();

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
            Debug.Log("1");
        }
    }
    private void ThinkEnemy()
    {
        int Pattan = Random.Range(0, 1);

        switch (Pattan)
        {
            case 0:
                StartCoroutine(Taunt());
                Debug.Log("2");
                break;
        }
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

}
