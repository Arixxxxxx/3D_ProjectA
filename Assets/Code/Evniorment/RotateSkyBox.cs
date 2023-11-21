using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    [SerializeField, Range(-0f, -30f)] float filterSpeed; 
    [SerializeField, Range(0f, 30f)] float skyBoxSpeed; 
    float skyboxSpinCount, filterSpinCount;
    float startBoxRoationValue, startFilterValue;

    private void Start()
    {
        filterSpinCount = transform.eulerAngles.y;
        startFilterValue = transform.eulerAngles.y;
        
    }
    void Update()
    {
        skyboxSpinCount += Time.deltaTime * skyBoxSpeed;
        filterSpinCount += Time.deltaTime * filterSpeed;
        filterSpinCount = Mathf.Repeat(filterSpinCount, startFilterValue);
        skyboxSpinCount = Mathf.Repeat(skyboxSpinCount, 360);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, filterSpinCount, transform.eulerAngles.z);
        RenderSettings.skybox.SetFloat("_Rotation", skyboxSpinCount);
    }
}
