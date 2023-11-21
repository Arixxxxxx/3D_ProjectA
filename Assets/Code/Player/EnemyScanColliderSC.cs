using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanColliderSC : MonoBehaviour
{
    private BoxCollider scanBoxColl;
    [Header("# Collider Size Setup")]
    [SerializeField, Range(0f, 20f)] float boxSizeX;
    [SerializeField, Range(0f, 20f)] float boxSizeY;
    [SerializeField, Range(0f, 20f)] float boxSizeZ;
    [Space(2)]
    [Header("# Collider Center Setup")]
    [SerializeField, Range(0f, 20f)] float boxCenterX;
    [SerializeField, Range(0f, 20f)] float boxCenterY;
    [SerializeField, Range(0f, 20f)] float boxCenterZ;

    [Header("# Targeting System")]
    [SerializeField] GameObject nearTargetObj;
    [SerializeField] List<GameObject> scanTargets = new List<GameObject>();

    Vector3 boxSizeVec;
    Vector3 boxCenVec;

    private void Awake()
    {
        scanBoxColl = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        SetupBoxCollSize();
        UpdatingNearTarget();
    }

    private void SetupBoxCollSize()
    {
        boxSizeVec = new Vector3(boxSizeX, boxSizeY, boxSizeZ);
        scanBoxColl.size = boxSizeVec;

        boxCenVec = new Vector3(boxCenterX, boxCenterY, boxCenterZ);
        scanBoxColl.center = boxCenVec;
    }

    float nearDis;
    float nextTargetDis;
    

    private void UpdatingNearTarget()
    {
        int count = scanTargets.Count;

        if (count == 1)
        {
            nearTargetObj = scanTargets[0];
        }
        else if (count == 0)
        {
            nearTargetObj = null;
        }

        else if (count > 1)
        {
            for (int i = 0; i < count; i++)
            {

                if (scanTargets[i] == null || nearTargetObj == null || i == (count-1)) { return; }

                nearDis = Vector3.Distance(transform.position, scanTargets[i].transform.position);
                nextTargetDis = Vector3.Distance(transform.position, scanTargets[i+1].transform.position);
                
                if(nearDis < nextTargetDis)
                {
                    nearTargetObj = scanTargets[i + 1].gameObject;
                }
            }
        }
    }

    public GameObject F_GetNearTarget()
    {
        return nearTargetObj;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && scanTargets.Contains(other.gameObject) == false)
        {
            scanTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && scanTargets.Contains(other.gameObject) == true)
        {
            scanTargets.Remove(other.gameObject);
        }
    }
}
