using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public enum EnemyType { Normal, Zombie}
    public EnemyType type;
    Enemy_NewViersion_Movement moveSc;


    [SerializeField] private float CurHP;
    [SerializeField] private float MaxHP;
    [SerializeField] private float MinAttackDMG;
    [SerializeField] private float MaxAttackDMG;
    [SerializeField] ParticleSystem[] hitPs;
    private Animator anim;
    private Animator hpBar_anim;
    Transform HP_Bar;
    Transform _DMGBOX;

    bool isHit;
    [SerializeField] int hitCount;
    [SerializeField] bool isDead;
    public bool IsDead {  get { return isDead; } }

    private void OnEnable()
    {
        Init();
    }

    private void Start()
    {
        if (type == EnemyType.Zombie)
        {
            moveSc = GetComponent<Enemy_NewViersion_Movement>();
        }
        anim = GetComponentInParent<Animator>();
        HP_Bar = transform.Find("HpBar").GetComponent<Transform>();
        hpBar_anim= HP_Bar.GetComponent<Animator>();
        _DMGBOX = transform.Find("DmgFontPoint").GetComponent<Transform>() ;

    }

    private void Update()
    {
        DeadHPCehaker();

        hitCount = (int)Mathf.Repeat(hitCount, 4);
    }

    private void Init()
    {
        CurHP = MaxHP;
        isDead = false;
    }
    
    public void F_OnHit(float DMG, bool isCri)
    {
        if(!isHit && !isDead)
        {
            isHit = true;

            if(type == EnemyType.Zombie) 
            {
                moveSc.F_isAutoAttackFalse();
            }
            
            if (CurHP > 0)
            {
                if(isCri == true) // 크리터짐
                {
                    CurHP -= DMG * 2f;
                }
                else if(isCri == false)
                {
                    CurHP -= DMG;
                }

                if (hitPs.Length != 0)
                {
                    StartCoroutine(HitPsPlay());
                }
                

                //대미지 폰트 소환
                GameObject obj = PoolManager.Inst.F_GetObj(0);
                obj.GetComponent<DMGFontMoveUp>().F_SetFont(DMG, _DMGBOX, isCri);
                if (CurHP <= 0)
                {
                    isDead = true;
                    anim.SetTrigger("Dead");
                    HP_Bar.gameObject.SetActive(false);

                    switch (type)
                    {
                        case EnemyType.Zombie:
                            moveSc.F_Dead();
                            break;
                    }
                }

            }
        
        }
        
        

        isHit = false;
    }
    private void DeadHPCehaker()
    {
        if(CurHP < 0)
        {
            CurHP = 0;
        }
    }
    public float[] F_ReturnParentHP()
    {
        float[] hpSet = new float[2];
        hpSet[0] = CurHP; 
        hpSet[1] = MaxHP;
        return hpSet;
    }

    WaitForSeconds HitPsDelay = new WaitForSeconds(0.25f);
    IEnumerator HitPsPlay()
    {
        hitPs[hitCount].gameObject.SetActive(true);
        hitPs[hitCount].Play();
        int Yebi = hitCount;
        hitCount++;

        yield return HitPsDelay;

        //while (hitPs[Yebi].isPlaying) // hit4 Ps가 지속시간이 살짝 더 길어서 고정시간으로 바꿈
        //{
        //    yield return null;
        //}

        hitPs[Yebi].Stop();
        hitPs[Yebi].gameObject.SetActive(false);
      
    }
}
