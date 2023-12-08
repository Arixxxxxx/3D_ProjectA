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
    /// 0������ // 1������� //����  true // �ݱ� false
    /// </summary>
    /// <param name="value"></param>
    public void F_Open_Shop(int value, bool bValue)
    {
        NpcNumber = value;

        if (bValue == false && (shop_UI.gameObject.activeSelf == true)) // ���� �ݱ�
        {
            shop_UI.gameObject.SetActive(false);
            return;
        }

        else if (bValue == true && (shop_UI.gameObject.activeSelf == false)) //���� ����
        {
            OpenShop = true;

            ExitButton.onClick.AddListener(() => //���� ��ư �ʱ�ȭ
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
                // ���Ļ��� 0��
                case 0:
                    shopName.text = "���� ����";
                    slot1_Img.sprite = slotImg[0];
                    slot1_ItemText.text = "��� ü�� ȸ��";
                    slot1_Pirce.text = "1,000";
                    BottomMsg.color = normal_Color;
                    BottomMsg.text = "�ȳ��ϼ���. ������ ���͵帱���?";
                    slot1_Button.onClick.AddListener(() =>
                    {
                        if (coin >= 1000)
                        {
                            PlayerStatsManager.inst.F_Recovery_HP();
                            Inventory.F_AddCoin(false, 1000);
                            coin = Inventory.F_Get_PlayerCoin();

                            BottomMsg.color = Color.green;
                            BottomMsg.text = "ü���� ���� ȸ���Ǿ����ϴ�.";
                        }
                        else if (coin < 1000)
                        {
                            BottomMsg.color = cancle_Color;
                            BottomMsg.text = "�����Ͻ� �ݾ��� �����մϴ�.";
                        }

                    });
                    break;
                
                // ������� 1��
                case 1:
                    shopName.text = "���� ����";
                    slot1_Img.sprite = slotImg[1];
                    slot1_ItemText.text = "���� ���� (�ּ�)";
                    slot1_Pirce.text = "2,500";
                    BottomMsg.color = normal_Color;
                    BottomMsg.text = "�ȳ��ϼ���. ������ ���͵帱���?";

                    slot1_Button.onClick.AddListener(() =>
                    {
                        if (coin >= 2500)
                        {
                            Debug.Log("���ⱸ�ſϷ�"); // ���� 


                            Inventory.F_AddCoin(false, 2500);
                            coin = Inventory.F_Get_PlayerCoin();

                            BottomMsg.color = Color.green;
                            BottomMsg.text = "���� ���� �Ϸ�!!";
                        }
                        else if (coin < 2500)
                        {
                            BottomMsg.color = cancle_Color;
                            BottomMsg.text = "�����Ͻ� �ݾ��� �����մϴ�.";
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
