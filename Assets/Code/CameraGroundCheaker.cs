using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGroundCheaker : MonoBehaviour
{
    [SerializeField] GameObject Ground;
    [SerializeField] CinemachineVirtualCamera Cam;
    [SerializeField] float upValue;
    [SerializeField] float cheakVerticalValue;
    [SerializeField] Vector3 cheakHitPos;
    SphereCollider coll;
    CinemachinePOV camsAim;
    
    [SerializeField] bool IsGround;
    private void Start()
    {
        camsAim = Cam.GetCinemachineComponent<CinemachinePOV>();
        coll = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        Chekaer();
        CamPositionUp();

    }

    private void Chekaer()
    {
        if(Ground != null)
        {
            IsGround = true;
        }
        else
        {
            IsGround = false;
        }
    }

    private void CamPositionUp()
    {
        if (IsGround)
        {
            if(camsAim.m_VerticalAxis.Value <= cheakVerticalValue)
            {
                camsAim.m_VerticalAxis.Value = cheakVerticalValue;
            }
            camsAim.m_VerticalAxis.Value += upValue;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") && Ground == null)
        {
            Ground = other.gameObject;
            cheakVerticalValue = camsAim.m_VerticalAxis.Value;
            
            //if(coll != null)
            //{
            //    Vector3 center = coll.bounds.center;
            //    float radius = coll.radius;

            //    float leftBound = center.x - radius;
            //    float rightBound = center.x + radius;

            //    if (transform.position.x < leftBound)
            //    {
            //        float camHorizontalValue = camsAim.m_HorizontalAxis.Value;
            //        if(camsAim.m_HorizontalAxis.Value < camHorizontalValue)
            //        {
            //            Debug.Log("aa");
            //            camsAim.m_HorizontalAxis.Value = camHorizontalValue;
            //        }
            //    }
            //    else if (transform.position.x > rightBound)
            //    {
            //        float camHorizontalValue = camsAim.m_HorizontalAxis.Value;
            //        if (camsAim.m_HorizontalAxis.Value > camHorizontalValue)
            //        {
            //            camsAim.m_HorizontalAxis.Value = camHorizontalValue;
            //        }
                //}

            

            cheakHitPos = other.gameObject.transform.position - transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") && Ground == null)
        {
            Ground = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") && Ground != null)
        {
            Ground = null;
        }
    }
}
