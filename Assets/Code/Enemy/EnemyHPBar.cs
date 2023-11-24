using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{

    [SerializeField] float _BackFillAmountSpeed;

    EnemyStats Stats;
    private Image _HP_F;
    private Image _HP_M;

    private float[] HpSet = new float[2];

    private void Start()
    {
        Stats = GetComponentInParent<EnemyStats>();
        _HP_F = transform.Find("HP_F").GetComponent<Image>();
        _HP_M = transform.Find("HP_M").GetComponent<Image>();
    }

    private void Update()
    {
        HpSet = Stats.F_ReturnParentHP();    

        if( HpSet.Length > 0 )
        {
            Set_FillAmount();
        }
    }
    private void Set_FillAmount()
    {
        _HP_F.fillAmount = HpSet[0] / HpSet[1];

        if(_HP_M.fillAmount >= _HP_F.fillAmount)
        {
            _HP_M.fillAmount -= _BackFillAmountSpeed * Time.deltaTime;
        }
        else if(_HP_M.fillAmount < _HP_F.fillAmount)
        {
            _HP_M.fillAmount = _HP_F.fillAmount;
        }

    }

    
}
