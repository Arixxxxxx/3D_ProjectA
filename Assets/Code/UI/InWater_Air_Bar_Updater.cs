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

    private void WaterChaker() // 캐릭터 물속 체크
    {
        inwater = player.F_IsInWater();
        charHeadOutOfWater = player.IsNoUpInWater;
    }
    
    private void CurAirTimer()
    {
        // 물속에 있고 머리도 물속으로 들어가있을때
        if (inwater == true && charHeadOutOfWater == true) 
        {
            if(airBarUiObject.gameObject.activeSelf == false)
            {
                airBarUiObject.gameObject.SetActive(true);
            }

            curAirTime -= Time.deltaTime;
        }

        // 물속에 몸은 있고 머리가 물밖으로 나와있을때
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
        
        // 물밖에있을때
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

    //현재 숨통치 제한
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
    IEnumerator NoAirDMG() // 물속 숨없으면 대미지 (딜레이 2초)
    {
        yield return null;
        playerStatsManager.F_Player_On_Hit(50);
        yield return airDMGDealy;
        hit = false;
    }


    // AirTime 프론트바 및 Fillamount 제어
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
    

    // 텍스트 업데이트
    private void AirTime_Text_Updater()
    {
        airBartext.text = $"수증 호흡  ( 남은 시간 : { curAirTime.ToString("0")} 초 )";
    }
}
