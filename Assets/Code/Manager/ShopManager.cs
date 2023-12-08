using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager inst;


    [Header("# Insert Slot Sprite")]
    [Space]
    [SerializeField] GameObject shop_UI;
    [SerializeField] Player_Cur_Inventory Inventory;
    [Header("# Insert Slot Sprite")]
    [Space]
    [SerializeField] Sprite[] slotImg;
    [Header("# Insert Shop Obj")]
    [Space]
    [Space]
    [SerializeField] TMP_Text shopName;
    [SerializeField] Image slot1_Img;
    [SerializeField] TMP_Text slot1_Pirce;
    [SerializeField] TMP_Text slot1_ItemText;
    [SerializeField] TMP_Text BottomMsg;
    [SerializeField] Button slot1_Button;
    [SerializeField] Button ExitButton;
    [SerializeField] Town_Npc_Animation NPC_Food;
    [SerializeField] Town_Npc_Animation NPC_Weapon;

    [SerializeField] int coin;
    [SerializeField] Color normal_Color;
    [SerializeField] Color cancle_Color;

    [SerializeField] bool OpenShop;
    [SerializeField] int NpcNumber;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if(OpenShop == true && Input.GetKeyDown(KeyCode.Escape)) 
        {
            switch (NpcNumber)
            {
                case 0:
                    OpenShop = false;
                    NPC_Food.F_CloseShop();
                    F_Open_Shop(0, false);
                    break;

                case 1:
                    OpenShop = false;
                    NPC_Weapon.F_CloseShop();
                    F_Open_Shop(0, false);
                    break;

            }

            
        }
    }
    /// <summary>
    /// 0음식점 // 1무기상점 //열기  true // 닫기 false
    /// </summary>
    /// <param name="value"></param>
    public void F_Open_Shop(int value, bool bValue)
    {
        NpcNumber = value;

        if (bValue == false && (shop_UI.gameObject.activeSelf == true)) // 상점 닫기
        {
            shop_UI.gameObject.SetActive(false);
            return;
        }

        else if (bValue == true && (shop_UI.gameObject.activeSelf == false)) //상점 열기
        {
            OpenShop = true;

            ExitButton.onClick.AddListener(() => //종료 버튼 초기화
            {
                
                OpenShop = false;

                switch(NpcNumber) 
                {
                    case 0:
                        NPC_Food.F_CloseShop();
                        break;

                    case 1:
                        NPC_Weapon.F_CloseShop();
                        break; 
                }
                
                F_Open_Shop(1, false);
            });

            coin = Inventory.F_Get_PlayerCoin();

            switch (value)
            {
                // 음식상점 0번
                case 0:
                    shopName.text = "음식 상점";
                    slot1_Img.sprite = slotImg[0];
                    slot1_ItemText.text = "모든 체력 회복";
                    slot1_Pirce.text = "1,000";
                    BottomMsg.color = normal_Color;
                    BottomMsg.text = "안녕하세요. 무엇을 도와드릴까요?";
                    slot1_Button.onClick.AddListener(() =>
                    {
                        if (coin >= 1000)
                        {
                            PlayerStatsManager.inst.F_Recovery_HP();
                            Inventory.F_AddCoin(false, 1000);
                            coin = Inventory.F_Get_PlayerCoin();

                            BottomMsg.color = Color.green;
                            BottomMsg.text = "체력이 전부 회복되었습니다.";
                        }
                        else if (coin < 1000)
                        {
                            BottomMsg.color = cancle_Color;
                            BottomMsg.text = "소지하신 금액이 부족합니다.";
                        }

                    });
                    break;
                
                // 무기상점 1번
                case 1:
                    shopName.text = "무기 상점";
                    slot1_Img.sprite = slotImg[1];
                    slot1_ItemText.text = "근접 무기 (쌍수)";
                    slot1_Pirce.text = "2,500";
                    BottomMsg.color = normal_Color;
                    BottomMsg.text = "안녕하세요. 무엇을 도와드릴까요?";

                    slot1_Button.onClick.AddListener(() =>
                    {
                        if (coin >= 2500)
                        {
                            Debug.Log("무기구매완료"); // 무기 


                            Inventory.F_AddCoin(false, 2500);
                            coin = Inventory.F_Get_PlayerCoin();

                            BottomMsg.color = Color.green;
                            BottomMsg.text = "무기 구매 완료!!";
                        }
                        else if (coin < 2500)
                        {
                            BottomMsg.color = cancle_Color;
                            BottomMsg.text = "소지하신 금액이 부족합니다.";
                        }

                        
                    });
                    break;

            }

            if (shop_UI.gameObject.activeSelf == false)
            {
                shop_UI.gameObject.SetActive(true);
            }
        }
       
    }

    
}
