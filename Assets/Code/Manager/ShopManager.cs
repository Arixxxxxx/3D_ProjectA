using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopManager : MonoBehaviour
{
    public static ShopManager inst;


    [Header("# Insert Slot Script")]
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
    [Header("# Insert Shop Slot 1")]
    [Space]
    [Space]
    [SerializeField] Image slot1_Img;
    [SerializeField] TMP_Text slot1_Pirce;
    [SerializeField] TMP_Text slot1_ItemText;
    [SerializeField] GameObject CoinIMG;
    [SerializeField] TMP_Text MyGold_Text;
    [SerializeField] Button slot1_Button;
    [SerializeField] Button ExitButton;
    [SerializeField] Town_Npc_Animation NPC_Food;
    [SerializeField] Town_Npc_Animation NPC_Weapon;
    [Header("# Npc Talk Box")]
    [Space]
    [SerializeField] Animator talkbox;
    [SerializeField] TMP_Text NPC_Talk_Text;
    [Space]
    [Space]
    [SerializeField] int coin;
    [SerializeField] Color normal_Color;
    [SerializeField] Color cancle_Color;

    [SerializeField] bool OpenShop;
    [SerializeField] int NpcNumber;

    [Header("# Npc Talk Box")]
    [Space]
    [SerializeField] GameObject Popup_Box;
    [SerializeField] TMP_Text Cur_Coin_Text;
    [SerializeField] TMP_Text MinusCoin_Text;
    [SerializeField] TMP_Text Sum_Text;

    [Header("# ITem Price List")]
    [Space]
    [SerializeField] List<int> Shop_Food_Price_List;
    [SerializeField] List<int> Shop_Smith_Price_List;
    private void Awake()
    {
        if (inst == null)
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
        if (OpenShop == true)
        {
            coin = Inventory.F_Get_PlayerCoin();
            MyGold_Text.text = coin.ToString("N0");
            RealTime_PopupBox_Update();
        }

        if (OpenShop == true && Input.GetKeyDown(KeyCode.Escape))
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

                switch (NpcNumber)
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



            switch (value)
            {
                // 음식상점 0번
                case 0:

                    //상점 UI 초기화
                    slot1_Button.onClick.RemoveAllListeners();
                    shopName.text = "음식 상점";
                    slot1_Img.sprite = slotImg[0];
                    slot1_ItemText.text = "모든 체력 회복";
                    slot1_Pirce.text = "1,000";
                    Set_SoldOutShop(true);

                    Play_NPC_Talk("안녕하세요.\n식사 하시고 가세요!", Color.black);

                    slot1_Button.onClick.AddListener(() =>
                    {
                        if (coin >= Shop_Food_Price_List[0])
                        {
                            PlayerStatsManager.inst.F_Recovery_HP();
                            Inventory.F_AddCoin(false, Shop_Food_Price_List[0]);
                            Play_NPC_Talk("구매해주셔서\n감사합니다!", Color.blue);
                            Popup_Box.gameObject.SetActive(false);
                        }
                        if (coin < Shop_Food_Price_List[0])
                        {

                            Play_NPC_Talk("손님.. 돈이 조금\n모자라네요..", cancle_Color);
                        }

                    });
                    break;

                // 무기상점 1번
                case 1:
                    shopName.text = "무기 상점";
                    slot1_Button.onClick.RemoveAllListeners();
                    Set_SoldOutShop(true);

                    if (GameManager.Inst.Isget_Melee_Weapon == false)
                    {
                        slot1_Img.sprite = slotImg[1];
                        slot1_ItemText.text = "근접 무기 (쌍수)";
                        slot1_Pirce.text = "2,500";
                    }
                    else
                    {
                        slot1_Img.sprite = slotImg[2];
                        Set_SoldOutShop(false);
                    }
                    

                    Play_NPC_Talk("최고의 무기!\n여기 모두 있소!", Color.black);
                    slot1_Button.onClick.AddListener(() =>
                    {
                        if (coin >= Shop_Smith_Price_List[0])
                        {
                            GameManager.Inst.Isget_Melee_Weapon = true;

                            QuestManager.inst.F_Set_Quest(3);
                            QuestManager.inst.F_Complete_Ui_QuestList(3);
                            GameUIManager.Inst.F_QuestComplete_UI_Open(3);
                            QuestManager.inst.F_player_Quest_Num_Up();
                            TalkData.inst.F_Set_Npc_QuestMaker(2, 2);



                            Inventory.F_AddCoin(false, Shop_Smith_Price_List[0]);
                            Play_NPC_Talk("구매해주셔서\n감사합니다!", Color.blue);

                            slot1_Img.sprite = slotImg[2];
                            Set_SoldOutShop(false);
                            Popup_Box.gameObject.SetActive(false);
                        }
                        else if (coin < Shop_Smith_Price_List[0])
                        {
                            Play_NPC_Talk("손님.. 돈이 조금\n모자라네요..", cancle_Color);
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

    // 상점 npc 대화상자
    private void Play_NPC_Talk(string Value, Color color)
    {
        StopCoroutine(Npc_TalkBox(Value, color));
        StartCoroutine(Npc_TalkBox(Value, color));
    }

    IEnumerator Npc_TalkBox(string Value, Color color)
    {
        if (talkbox.gameObject.activeSelf == false)
        {
            talkbox.gameObject.SetActive(true);
        }
        NPC_Talk_Text.color = color;
        NPC_Talk_Text.text = Value;

        yield return null;

        talkbox.SetTrigger("Play");
    }

    // 팝업박스 계산 제어
    private void RealTime_PopupBox_Update()
    {
        if (Popup_Box.gameObject.activeSelf == false) { return; }

        switch (NpcNumber)
        {
            case 0:
                Cur_Coin_Text.text = Sum_Price(0, coin, Shop_Food_Price_List[0]);
                MinusCoin_Text.text = Sum_Price(1, coin, Shop_Food_Price_List[0]);


                if(coin < Shop_Food_Price_List[0])
                {
                    Sum_Text.color = cancle_Color;
                }
                else if(coin >= Shop_Food_Price_List[0])
                {
                    Sum_Text.color = Color.white;
                }

                    Sum_Text.text = Sum_Price(2, coin, Shop_Food_Price_List[0]);
                break;

            case 1:

                Cur_Coin_Text.text = Sum_Price(0, coin, Shop_Smith_Price_List[0]);
                MinusCoin_Text.text = Sum_Price(1, coin, Shop_Smith_Price_List[0]);

                if (coin < Shop_Smith_Price_List[0])
                {
                    Sum_Text.color = cancle_Color;
                }
                else if (coin >= Shop_Smith_Price_List[0])
                {
                    Sum_Text.color = Color.white;
                }
                Sum_Text.text = Sum_Price(2, coin, Shop_Smith_Price_List[0]);
                break;
        }
    }



    // 버튼 이벤트 트리거 함수
    public void Event_EnterPopupBox(bool value)
    {
        if (value == true)
        {
            if (Popup_Box.gameObject.activeSelf == false)
            {
                Popup_Box.gameObject.SetActive(true);
            }
        }
        else
        {
            Popup_Box.gameObject.SetActive(false);
        }
    }
    /// <summary>
    ///  상점 팝업박스 계산해서 String Data Return
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value_0">현재 금액</param>
    /// <param name="value_1">물건 금액</param>
    private string Sum_Price(int type, int value_0, int value_1)
    {
        string result = string.Empty;

        switch (type)
        {
            case 0:
                result = value_0.ToString("N0");
                return result;


            case 1:
                result = value_1.ToString("N0");
                return result;


            case 2:
                result = (value_0 - value_1).ToString("N0");
                return result;
        }

        return result;
    }

    private void Set_SoldOutShop(bool value)
    {
        if(value == true)
        {
            slot1_ItemText.gameObject.SetActive(true);
            slot1_Pirce.gameObject.SetActive(true);
            slot1_Button.gameObject.SetActive(true);
            CoinIMG.gameObject.SetActive(true);
        }
        else
        {
            slot1_ItemText.gameObject.SetActive(false);
            slot1_Pirce.gameObject.SetActive(false);
            slot1_Button.gameObject.SetActive(false);
            CoinIMG.gameObject.SetActive(false);
        }
       
    }
}
