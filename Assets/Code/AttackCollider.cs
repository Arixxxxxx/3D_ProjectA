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

    PlayerStatsManager playerStatsManager;

    private void Start()
    {
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
    }
    private void Update()
    {
        DMG = playerStatsManager.F_GetPlayerDMG(0);
        MinDMG = DMG[0];
        MaxDMG = DMG[1];
    }

    
    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case CharacterType.Player:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    EnemyStats sc = other.GetComponent<EnemyStats>();
                    sc.F_OnHit(Random.Range(MinDMG, MaxDMG));
                    Debug.Log("11");
                }
                break;
        }

    }
}
