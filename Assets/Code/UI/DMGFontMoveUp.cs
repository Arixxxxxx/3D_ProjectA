using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class DMGFontMoveUp : MonoBehaviour
{
    TMP_Text DmgText;
    Vector3 TargetPoint;
    float Timer;
    [SerializeField] Color normalDmgColor;
    [SerializeField] float normalDmgFontSize;
    [SerializeField] Color CriticalDmgColor;
    [SerializeField] float CriticalDmgFontSize;
    Animator anim;

    [SerializeField] float duration;
    [SerializeField] float fontMoveSpeed;
    [SerializeField, Range(0f,1f) ] float heightVolume;

    bool doStartMove;
    bool isCritical;
    Vector3 orizin_Text_Pos;

    private void Awake()
    {
        DmgText = GetComponentInChildren<TMP_Text>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(doStartMove)
        {
            MoveFont();
        }
        
    }

    /// <summary>
    /// 폰트 초기화 
    /// </summary>
    /// <param name="DMG">대미지수치</param>
    /// <param name="ParentTransform">부모 트랜스</param>
    public void F_SetFont(float DMG, Transform ParentTransform, bool isCri)
    {
        if (DmgText == null || anim == null)
        {
            DmgText = GetComponentInChildren<TMP_Text>();
        }

        transform.SetParent(ParentTransform);
        transform.position = transform.parent.position;
        DmgText.text = string.Empty;
        orizin_Text_Pos = DmgText.transform.position;

        if (isCri == true)
        {
            DmgText.color = CriticalDmgColor;
            DmgText.fontSize = CriticalDmgFontSize;

            DmgText.fontStyle = FontStyles.Bold | FontStyles.Italic;

            isCritical = true;
            TargetPoint = DmgText.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

            DmgText.text = $"Hit! {DMG.ToString("0")}";
        }
        else if (isCri == false)
        {
            DmgText.color = normalDmgColor;
            DmgText.fontSize = normalDmgFontSize;

            DmgText.fontStyle = FontStyles.Normal | FontStyles.Italic;

            isCritical = false;
            TargetPoint = transform.position + new Vector3(Random.Range(-0.6f, 0.6f), heightVolume);
            DmgText.text = DMG.ToString("0");
        }



        
       

        doStartMove = true;
    }

    bool once;
    private void MoveFont()
    {
        if(!doStartMove || DmgText.text == string.Empty )  { return; }

        Timer += Time.deltaTime;

        switch (isCritical)
        {
            case true:

                if (Timer < duration)
                {

                    if (!once)
                    {
                        once = true;
                        anim.SetTrigger("Cri");
                    }

                    transform.position = TargetPoint;
                }

                else if (Timer > duration)
                {
                    Timer = 0;
                    DmgText.transform.position = orizin_Text_Pos;
                    doStartMove = false;
                    once = false;
                    isCritical = false;
                    PoolManager.Inst.F_ReturnObj(this.gameObject, 0);

                }
                break;

                case false:

                if (Timer < duration)
                {
                    DmgText.transform.position = Vector3.Lerp(DmgText.transform.position, TargetPoint, fontMoveSpeed * Time.deltaTime);
                }

                else if (Timer > duration)
                {
                    Timer = 0;
                    DmgText.transform.position = orizin_Text_Pos;
                    doStartMove = false;

                    PoolManager.Inst.F_ReturnObj(this.gameObject, 0);

                }
                break;

        }
               
    }
}
