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
    ///  0메인/1푸드/2무기
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
        TalkList.Add(100, new string[] { "안녕하세요. 이곳에 오신걸 환영합니다.\n 앞에 있는 마을로가서 퀘스트를 받고 마을을 체험해보세요.", "행운을 빌겠습니다." });
        TalkList.Add(101, new string[] { "마을에 잘 찾아오셨네요!\n 마을안에 게시판에서 추가적인 퀘스트를 받을 수 있습니다.\n 마을을 둘러보세요. :)" });
        TalkList.Add(102, new string[] { "아직 드릴말씀이 없네요. 나중에 다시 찾아오세요.." });
        TalkList.Add(103, new string[] { "이제 전투에 대해서 알아볼 시간입니다. \n 전투가이드를 보여드릴테니 확인하시고 앞에 좀비밭에서 테스트해보세요" });
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

    // npc 대화
    bool once;
    bool chat;
    [SerializeField] int talkIndexs;
    bool isNextTextOk;

    string TextValue;
    /// <summary>
    /// npc상호작용
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



            if (TextValue == null && TalkBok_UI.gameObject.activeSelf == true) // 대화종료시
            {
                End_Chat_Envent(); // 종료 이벤트


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


    // 채팅창 이름 미리 적어놓기
    private void TextBox_NameUpdate(GameObject obj)
    {
        if (NearNpc != null)
        {
            switch (obj.name)
            {
                case "unitychan":
                    Name.text = "<< 게임가이드 NPC >>";
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

    // 채팅 종료후 특별한 이벤트 발동
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
    // 플레이어 주변 NPC 확인
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
        sc.F_ValueUpdate(0); // 대화 넘버 올려줌

        GameObject obj = PoolManager.Inst.F_GetObj(1); // 순간이동 파티클
        obj.SetActive(true);
        obj.GetComponent<ParticleSystem>().Play();
        obj.transform.position = NearNpc.transform.position + new Vector3(0, 1, 0);

        Unit_TelePort.inst.F_Teleport(sc.gameObject, 0); // 위치이동
        sc.F_Swithing_QuestMarker(1); // 퀘스트 마커 변경
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
        //UI 보여주기
        yield return wait05;
        Debug.Log("전투UI창 열림");

    }


    /// <summary>
    /// 0느낌표/1물음표/2없음
    /// </summary>
    /// <param name="NpcNum"></param>
    /// <param name="value"></param>
    public void F_Set_Npc_QuestMaker(int NpcNum, int value)
    {
        npcs[NpcNum].F_Swithing_QuestMarker(value);
    }


    /// <summary>
    /// npc 대화ID 증가
    /// </summary>
    /// <param name="NpcNum"> 0메인/1푸드/2상점</param>
    /// <param name="value">0=ID</param>
    public void F_Set_Npc_ID_ValueUp(int npc_num, int value)
    {
        npcs[npc_num].F_ValueUpdate(value);
    }

}
