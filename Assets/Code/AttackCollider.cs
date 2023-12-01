using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public enum CharacterType { Player,Enemy}
    public CharacterType type;
    [SerializeField] float MinDMG;
    [SerializeField] float MaxDMG;
    float[] DMG;
    float CriticalChance;
    float CriDice;

    bool once;

    PlayerStatsManager playerStatsManager;

    private void Start()
    {
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
    }
    private void Update()
    {
        if(type == CharacterType.Player) 
        {
            DMG = playerStatsManager.F_GetPlayerDMG(0);
            MinDMG = DMG[0];
            MaxDMG = DMG[1];
            CriticalChance = playerStatsManager.Criti;
        }
       
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (once == false)
        {
            once = true;

            switch (type)
            {
                case CharacterType.Player:

                    if (other.gameObject.CompareTag("Enemy"))
                    {
                        EnemyStats sc = other.GetComponent<EnemyStats>();

                        CriDice = Random.Range(0, 100);
                        Debug.Log(CriDice);
                        if (CriDice <= CriticalChance)
                        {
                            sc.F_OnHit(Random.Range(MinDMG, MaxDMG), true);
                        }
                        else if (CriDice > CriticalChance)
                        {
                            sc.F_OnHit(Random.Range(MinDMG, MaxDMG), false);
                        }

                        CriDice = 0;
                    }
                    break;

                case CharacterType.Enemy:

                    if (other.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("11");
                        PlayerStatsManager sc = other.GetComponent<PlayerStatsManager>();
                        sc.F_Player_On_Hit(Random.Range(MinDMG, MaxDMG));
                       
                    }
                    break;
            }

        }



        once = false;
    }
}
