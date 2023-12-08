using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager inst;

    [SerializeField] Animator Scrren_Blood;

    [Header("# Player Helth & Mp Info")]
    [Space]
    [SerializeField] float CurHP;
    [SerializeField] float MaxHP;
    [Space]
    [SerializeField] float CurMP;
    [SerializeField] float MaxMP;
    [Space]
    [SerializeField] float CurSP;
    [SerializeField] float MaxSP;
    [Space(2)]
    [Header("# Player Attack Info")]
    [Space]
    [SerializeField] float MeleeMinDMG;
    [SerializeField] float MeleeMaxDMG;
    [SerializeField] float RangeMinDMG;
    [SerializeField] float RangeMaxDMG;
    [SerializeField] float CriticalChance;
    public float Criti { get { return CriticalChance; } }
    [SerializeField] bool onHit;

    [SerializeField] float Def;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }
    }


    private void Start()
    {
        
    }

    public void F_Player_On_Hit(float DMG)
    {
        if(onHit == false && CurHP > 0)
        {
            onHit = true;
            F_SetPlayerHP(DMG);

            if (CurHP <= 0)
            {
                //죽음
                return;
            }

            Scrren_Blood.SetTrigger("Hit");

            onHit = false;
        }
        
    }

    private void F_SetPlayerHP(float DMG)
    {

        CurHP -= DMG;
    }



    /// <summary>
    /// 플레이어 대미지 리턴 0 = Melee / 1 Range
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    public float[] F_GetPlayerDMG(int Type)
    {
        float[] Dmg = new float[2];
        switch (Type)
        {
            case 0:
                Dmg[0] = MeleeMinDMG;
                Dmg[1] = MeleeMaxDMG;
                return Dmg;


            case 1:
                Dmg[0] = RangeMinDMG;
                Dmg[1] = RangeMaxDMG;
                return Dmg;

        }

        return default;

    }

    public void F_Recovery_HP()
    {
        CurHP = MaxHP;
    }
}
