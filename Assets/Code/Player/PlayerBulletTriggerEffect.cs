using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerBulletTriggerEffect : MonoBehaviour
{
    [SerializeField] GameObject boom;
    

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            GameObject obj = Instantiate(boom, transform.position, Quaternion.identity, PoolManager.Inst.transform);
            obj.GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
            
        }
    }
}
