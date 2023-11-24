using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
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

    [SerializeField] float Def;


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
}
