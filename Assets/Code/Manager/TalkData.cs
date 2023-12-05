using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class TalkData : MonoBehaviour
{
    public static TalkData inst;

    [SerializeField] GameObject NearNpc;
    [SerializeField] GameObject TalkBok;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Text;
    TalkBox_Text_Effect mainText;
    [SerializeField] int CurNpcNum;

    Dictionary<int, string[]> TalkList = new Dictionary<int, string[]>();

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
        TalkDataInit();
    }

    private void Start()
    {
        mainText = Text.GetComponent<TalkBox_Text_Effect>();
    }

    private void Update()
    {
        InteractionNPC();
    }

    private void TalkDataInit()
    {
        TalkList.Add(100, new string[] { "�ȳ��ϼ���. �̰��� ���Ű� ȯ���մϴ�.\n �տ� �ִ� �����ΰ��� ����Ʈ�� �ް� ������ ü���غ�����.", "����� ���ڽ��ϴ�." });
    }

    private string Get_TalkINdex(int npcID, int talkIndex)
    {

        if (talkIndex == TalkList[npcID].Length)
        {
            return null;
        }
        else
        {
            return TalkList[npcID][talkIndex];
        }
    }

    // npc ��ȭ
    bool once;
    bool chat;
    [SerializeField] int talkIndexs;
    bool isNextTextOk;
    
    string TextValue;
    private void InteractionNPC()
    {
        chat = Input.GetKeyDown(KeyCode.F);

        if (NearNpc != null && chat == true)
        {
            if (once == false)
            {
                once = true;
                CameraManager.inst.F_ChangeCam(4);
            }

            if (TalkBok.gameObject.activeSelf == false)
            {
                TalkBok.gameObject.SetActive(true);
            }

           
            TextValue = Get_TalkINdex(CurNpcNum, talkIndexs);



            if (TextValue == null && TalkBok.gameObject.activeSelf == true) // ��ȭ�����
            {
                End_Chat_Envent();
                TalkBok.gameObject.SetActive(false);
                CameraManager.inst.F_ChangeCam(0);
                talkIndexs = 0;
                once = false;
                return;
            }

            else if (TextValue != null && isNextTextOk == false)
            {
                Debug.Log("2");
                isNextTextOk = true;
                mainText.F_Set_TalkBox_Main_Text(TextValue);
                talkIndexs++;
            }

            
            Debug.Log("++");
        }
    }

    public void F_IsNextOk()
    {
        isNextTextOk = false;
    }
    // ä��â �̸� �̸� �������
    private void TextBox_NameUpdate(GameObject obj)
    {
        if (NearNpc != null)
        {
            switch (obj.name)
            {
                case "unitychan":
                    Name.text = "<< ���Ӱ��̵� NPC >>";
                    NPC_Talk_Num sc = obj.GetComponent<NPC_Talk_Num>();
                    CurNpcNum = sc.F_Get_NPC_TalkID();
                    break;

            }
        }
        else if (NearNpc == null)
        {
            Name.text = string.Empty;
            Text.text = string.Empty;
        }
    }

    // ä�� ������ Ư���� �̺�Ʈ �ߵ�
    private void End_Chat_Envent()
    {
        Debug.Log("1");
        switch (NearNpc.name)
        {
            case "unitychan":

                StartCoroutine(Event_1());
                
                
                break;
        }

    }
    // �÷��̾� �ֺ� NPC Ȯ��
    public void F_Set_Player_Near_Npc(GameObject obj, bool value)
    {
        if (value == true)
        {
            NearNpc = obj;
            TextBox_NameUpdate(obj);
        }
        else
        {
            NearNpc = null;
            TextBox_NameUpdate(obj);
        }
    }









    WaitForSeconds wait05 = new WaitForSeconds(0.5f);
    WaitForSeconds wait1 = new WaitForSeconds(1);
    IEnumerator Event_1()
    {
        QuestManager.inst.F_Insert_Ui_QuestList(0);
        yield return wait05;
        NPC_Talk_Num sc = NearNpc.GetComponent<NPC_Talk_Num>();
        sc.F_ValueUpdate(0);
        GameObject obj = PoolManager.Inst.F_GetObj(1);
        obj.SetActive(true);
        obj.GetComponent<ParticleSystem>().Play();
        obj.transform.position = NearNpc.transform.position + new Vector3(0, 1, 0);
        NearNpc.gameObject.SetActive(false);    
    }

}
