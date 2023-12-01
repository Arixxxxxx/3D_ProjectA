using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InWater_Air_Bar_Updater : MonoBehaviour
{
    [SerializeField] GameObject airBarUiObject;
    [SerializeField] Image FrontBar;
    [SerializeField] TMP_Text airBartext;
    [Header("# Insert Value => Air Time")]
    [Space]
    [SerializeField] float curAirTime;
    [SerializeField] float MaxAirTime;

    bool charHeadOutOfWater;
    bool inwater;

    PlayerMoveController player;
    PlayerStatsManager playerStatsManager;
    Animator anim;

    private void Start()
    {
        player = PlayerMoveController.Inst;
        curAirTime = MaxAirTime;
        playerStatsManager = player.GetComponent<PlayerStatsManager>();
        anim = airBarUiObject.GetComponent<Animator>();
    }

    private void Update()
    {
        WaterChaker();
        CurAirTimer();
        CurAir_Max_Updater();
        AirTime_FrontBar_Fillamount_Updater();
        AirTime_Text_Updater();
    }

    private void WaterChaker() // ĳ���� ���� üũ
    {
        inwater = player.F_IsInWater();
        charHeadOutOfWater = player.IsNoUpInWater;
    }
    
    private void CurAirTimer()
    {
        // ���ӿ� �ְ� �Ӹ��� �������� ��������
        if (inwater == true && charHeadOutOfWater == true) 
        {
            if(airBarUiObject.gameObject.activeSelf == false)
            {
                airBarUiObject.gameObject.SetActive(true);
            }

            curAirTime -= Time.deltaTime;
        }

        // ���ӿ� ���� �ְ� �Ӹ��� �������� ����������
        else if (inwater == true && charHeadOutOfWater == false) 
        {
            curAirTime+= Time.deltaTime;
            
            if(curAirTime >= MaxAirTime)
            {
                airBarUiObject.gameObject.SetActive(false);
            }

            if (anim.GetBool("Danger") == true)
            {
                anim.SetBool("Danger", false);
            }
        }
        
        // ���ۿ�������
        if(inwater == false)
        {
            curAirTime += Time.deltaTime;

            if (curAirTime >= MaxAirTime)
            {
                airBarUiObject.gameObject.SetActive(false);
            }
        }
    }
    bool hit;

    //���� ����ġ ����
    private void CurAir_Max_Updater()
    {
        if(curAirTime >= MaxAirTime)
        {
            curAirTime = MaxAirTime;
        }
        if(curAirTime <= 0)
        {
            curAirTime = 0;

            if(hit == false)
            {
                hit = true;
                StartCoroutine(NoAirDMG());
            }
        }
    }

    WaitForSeconds airDMGDealy = new WaitForSeconds(2f);
    IEnumerator NoAirDMG() // ���� �������� ����� (������ 2��)
    {
        yield return null;
        playerStatsManager.F_Player_On_Hit(50);
        yield return airDMGDealy;
        hit = false;
    }


    // AirTime ����Ʈ�� �� Fillamount ����
    float CheakTime;
    private void AirTime_FrontBar_Fillamount_Updater()
    {
        FrontBar.fillAmount = curAirTime / MaxAirTime;

         CheakTime = FrontBar.fillAmount;
        
        if(CheakTime < 0.3f)
        {
            anim.SetBool("Danger", true);
        }
    }
    

    // �ؽ�Ʈ ������Ʈ
    private void AirTime_Text_Updater()
    {
        airBartext.text = $"���� ȣ��  ( ���� �ð� : { curAirTime.ToString("0")} �� )";
    }
}
