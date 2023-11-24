using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    
    [SerializeField] private float CurHP;
    [SerializeField] private float MaxHP;
    [SerializeField] private float MinAttackDMG;
    [SerializeField] private float MaxAttackDMG;
    private Animator anim;
    private Animator hpBar_anim;
    Transform HP_Bar;

    bool isHit;
    [SerializeField] bool isDead;
    public bool IsDead {  get { return isDead; } }

    private void OnEnable()
    {
        CurHP = MaxHP;
        
    }
    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        HP_Bar = transform.Find("HpBar").GetComponent<Transform>();
        hpBar_anim= HP_Bar.GetComponent<Animator>();
    }

    private void Update()
    {
        DeadHPCehaker();
    }

    
    public void F_OnHit(float DMG)
    {
        if(!isHit && !isDead)
        {
            if (CurHP > 0)
            {
                isHit = true;
                CurHP -= DMG;
                hpBar_anim.SetTrigger("Hit");
                Debug.Log(DMG);

                if (CurHP <= 0)
                {
                    isDead = true;
                    anim.SetTrigger("Dead");
                    HP_Bar.gameObject.SetActive(false);
                }
            }
           else if(CurHP <= 0)
            {
                isDead = true;
                anim.SetTrigger("Dead");
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
}
