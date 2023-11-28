using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBullet : MonoBehaviour
{

    [Header("# Boom DMG Setting")]
    [Space]
    [SerializeField] float MinDMG;
    [SerializeField] float MaxDMG;
    
    
    float currentTime;
    float totalDuration;
    float playbackRatio;
    SphereCollider coll;
    ParticleSystem Ps;
  
    void Start()
    {
        coll = GetComponent<SphereCollider>();
        Ps  = GetComponent<ParticleSystem>();
        //StartCoroutine(OnColl()); // ������� ������ ������ �޾ƾ��Ҷ�
        StartCoroutine(KeepDMG());
    }
    private void Update()
    {
        if (Ps.isPlaying)
        {
            currentTime = Ps.time;
            totalDuration = Ps.duration;
            playbackRatio = currentTime / totalDuration;
        }
    }
   // ��ô����
         IEnumerator KeepDMG()
    {
        coll.enabled = true;
        while (playbackRatio < 0.2f)
        {
            yield return null;
        }
        coll.enabled = false;
    }
    //������ �����
    WaitForSeconds colliderDur = new WaitForSeconds(0.1f);
    IEnumerator OnColl()
    {
        
       while (playbackRatio < 0.5f)
        {
            yield return null;
        }
        
        coll.enabled = true;
        yield return colliderDur;

        coll.enabled = false;
    }

    float Dice;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && other.GetComponent<EnemyStats>() != null)
        {
            Dice = Random.Range(0f,100f);
      
            other.GetComponent<EnemyStats>().F_OnHit(Random.Range(MinDMG, MaxDMG), Dice < PlayerStatsManager.inst.Criti ? true : false);
        }
    }
}
