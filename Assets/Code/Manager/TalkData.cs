using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class TalkData : MonoBehaviour
{
    public static TalkData inst;

    [SerializeField] GameObject NearNpc;
    [SerializeField] GameObject TalkBok_UI;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Text;
    TalkBox_Text_Effect mainText;
    [SerializeField] int CurNpcNum;

    /// <summary>
    ///  0����/1Ǫ��/2����
    /// </summary>
    [SerializeField] NPC_Talk_Num[] npcs;

    Dictionary<int, string[]> TalkList = new Dictionary<int, string[]>();

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
        TalkDataInit();
    }

    private void Start()
    {
        mainText = Text.GetComponent<TalkBox_Text_Effect>();
    }

    private void Update()
    {
        InteractionNPC();
        CurNpCNum_Updater();
    }

    private void CurNpCNum_Updater()
    {
        if (NearNpc != null)
        {
            NPC_Talk_Num sc = NearNpc.GetComponent<NPC_Talk_Num>();
            CurNpcNum = sc.F_Get_NPC_TalkID();
        }
    }
    private void TalkDataInit()
    {
        TalkList.Add(100, new string[] { "�ȳ��ϼ���. �̰��� ���Ű� ȯ���մϴ�.\n �տ� �ִ� �����ΰ��� ����Ʈ�� �ް� ������ ü���غ�����.", "����� ���ڽ��ϴ�." });
        TalkList.Add(101, new string[] { "������ �� ã�ƿ��̳׿�!\n �����ȿ� �Խ��ǿ��� �߰����� ����Ʈ�� ���� �� �ֽ��ϴ�.\n ������ �ѷ�������. :)" });
        TalkList.Add(102, new string[] { "���� �帱������ ���׿�. ���߿� �ٽ� ã�ƿ�����.." });
        TalkList.Add(103, new string[] { "���� ������ ���ؼ� �˾ƺ� �ð��Դϴ�. \n �������̵带 �����帱�״� Ȯ���Ͻð� �տ� ����翡�� �׽�Ʈ�غ�����" });
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
    /// <summary>
    /// npc��ȣ�ۿ�
    /// </summary>
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

            if (TalkBok_UI.gameObject.activeSelf == false)
            {
                TalkBok_UI.gameObject.SetActive(true);
            }


            TextValue = Get_TalkINdex(CurNpcNum, talkIndexs);



            if (TextValue == null && TalkBok_UI.gameObject.activeSelf == true) // ��ȭ�����
            {
                End_Chat_Envent(); // ���� �̺�Ʈ


                TalkBok_UI.gameObject.SetActive(false);
                CameraManager.inst.F_ChangeCam(0);
                talkIndexs = 0;
                once = false;
                return;
            }

            else if (TextValue != null && isNextTextOk == false)
            {
                isNextTextOk = true;
                mainText.F_Set_TalkBox_Main_Text(TextValue);
                talkIndexs++;
            }
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
        switch (NearNpc.name)
        {
            case "Main_NPC":

                switch (CurNpcNum)
                {
                    case 100:
                        StartCoroutine(Event_100());
                        break;

                    case 101:
                        StartCoroutine(Event_101());
                        break;

                    case 103:
                        StartCoroutine(Event_103());
                        break;
                }

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
    IEnumerator Event_100()
    {
        QuestManager.inst.F_Set_Quest(0);

        yield return wait05;

        NPC_Talk_Num sc = NearNpc.GetComponent<NPC_Talk_Num>();
        sc.F_ValueUpdate(0); // ��ȭ �ѹ� �÷���

        GameObject obj = PoolManager.Inst.F_GetObj(1); // �����̵� ��ƼŬ
        obj.SetActive(true);
        obj.GetComponent<ParticleSystem>().Play();
        obj.transform.position = NearNpc.transform.position + new Vector3(0, 1, 0);

        Unit_TelePort.inst.F_Teleport(sc.gameObject, 0); // ��ġ�̵�
        sc.F_Swithing_QuestMarker(1); // ����Ʈ ��Ŀ ����
    }

    IEnumerator Event_101()
    {

        QuestManager.inst.F_Set_Quest(0);
        QuestManager.inst.F_Complete_Ui_QuestList(0);
        QuestManager.inst.F_player_Quest_Num_Up();
        GameUIManager.Inst.F_QuestComplete_UI_Open(0);

        yield return wait05;

        NPC_Talk_Num sc = NearNpc.GetComponent<NPC_Talk_Num>();
        sc.F_ValueUpdate(0);
        sc.F_Swithing_QuestMarker(2);
    }

    IEnumerator Event_103()
    {
        F_Set_Npc_ID_ValueUp(0, 0);
        F_Set_Npc_QuestMaker(0, 2);
        QuestManager.inst.F_Complete_Ui_QuestList(4);
        //UI �����ֱ�
        yield return wait05;
        Debug.Log("����UIâ ����");

    }


    /// <summary>
    /// 0����ǥ/1����ǥ/2����
    /// </summary>
    /// <param name="NpcNum"></param>
    /// <param name="value"></param>
    public void F_Set_Npc_QuestMaker(int NpcNum, int value)
    {
        npcs[NpcNum].F_Swithing_QuestMarker(value);
    }


    /// <summary>
    /// npc ��ȭID ����
    /// </summary>
    /// <param name="NpcNum"> 0����/1Ǫ��/2����</param>
    /// <param name="value">0=ID</param>
    public void F_Set_Npc_ID_ValueUp(int npc_num, int value)
    {
        npcs[npc_num].F_ValueUpdate(value);
    }

}
