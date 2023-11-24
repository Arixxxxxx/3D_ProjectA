using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform target_Player;
    [SerializeField] GameObject _LockOnIMG;

    private void Update()
    {
        //_LockOnIMG.transform.LookAt(target_Player);
        Vector3 dir = target_Player.position - transform.position;
        _LockOnIMG.transform.rotation = Quaternion.LookRotation(dir);
    }
}
