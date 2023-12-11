using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuestManager : MonoBehaviour
{
    public static QuestManager inst;
    [Header("# Player Quest Num")]
    [Space]
    [SerializeField] int player_Quest_Num;
    [Header("# Insert Object In Inspecter")]
    [Space]
    [SerializeField] GameObject[] UI_Quest;
    [SerializeField] Transform UI_QuestList;
    [SerializeField] QuestBoard QuestBoard;
    [Header("# UIQuestObj & Town QuestBoard")]
    [Space]
    [SerializeField] GameObject[] Board_Quest_Prefabs;
    [SerializeField] GameObject TownQuestBoard;
    [SerializeField] Transform TownQuestBoardSlot;
    [Header("# Quest UI Info")]
    [Space]
    [SerializeField] Animator questAcceptWindow;
    [SerializeField] TMP_Text questAcceptWindowText;
    [SerializeField] float dealy;
    WaitForSeconds DealyWait;
    [Header("# Insert Value In Inspecter")]
    [Space]
    [SerializeField] int QuestCount;
    [SerializeField] Dictionary<int, int> PlayerQuest_Situation = new Dictionary<int, int>();

    List<GameObject> CurPlayQuest = new List<GameObject>();
    
    List<GameObject> QuestBoard_Prefab_List = new List<GameObject>();

    Transform CompleteQuestTransform;

    [Header("# Quest Info")]
    [Space]
    [SerializeField] GameObject Q1_Obj_Group;
    [SerializeField] GameObject Q2_Obj_Group;
    
    [SerializeField] List<int> Cur_Ea_QuestNum;
    [SerializeField] List<int> Max_Ea_QuestNum;


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

        for (int i = 0; i < QuestCount; i++)
        {
            PlayerQuest_Situation[i] = 0;
            Debug.Log(PlayerQuest_Situation[i]);
        }

        CompleteQuestTransform = transform.Find("CompleteQuest").GetComponent<Transform>();
    }
    void Start()
    {
        DealyWait = new WaitForSeconds(dealy);
    }

    // Update is called once per frame
    void Update()
    {
        WindowClose_Input_Chek();
        Insert_Quest_Board_Prefabs();

    }
    // 플레이어 퀘스트진행 관련 함수

    public void F_player_Quest_Num_Up()
    {
        player_Quest_Num++;
    }

    // ## 퀘스트별 진행사항 함수들

    // 퀘스트넘버 올라가면 보드에 퀘스트 받을수있게 버튼 생성
    bool Quest1Start;
    bool Quest2Start;
    bool Quest3Start;
    bool Quest4Start;
    private void Insert_Quest_Board_Prefabs()
    {
        switch (player_Quest_Num)
        {
            case 1:
                if (!Quest1Start)
                {
                    Quest1Start = true;
                    GameObject obj = Instantiate(Board_Quest_Prefabs[player_Quest_Num - 1]);
                    QuestBoard_Prefab_List.Add(obj);
                    obj.transform.SetParent(TownQuestBoardSlot);
                    QuestBoard.F_Swich_Maker(1);


                }
                break;

            case 2:
                if (!Quest2Start)
                {
                    Quest2Start = true;
                    GameObject obj = Instantiate(Board_Quest_Prefabs[player_Quest_Num - 1]);
                    QuestBoard_Prefab_List.Add(obj);
                    obj.transform.SetParent(TownQuestBoardSlot);
                    QuestBoard.F_Swich_Maker(1);
                }
                break;


            case 3:
                if (!Quest3Start)
                {
                    Quest3Start = true;
                    GameObject obj = Instantiate(Board_Quest_Prefabs[player_Quest_Num - 1]);
                    QuestBoard_Prefab_List.Add(obj);
                    obj.transform.SetParent(TownQuestBoardSlot);
                    QuestBoard.F_Swich_Maker(1);
                }
                break;

            case 4:
                if (!Quest4Start)
                {
                    Quest4Start = true;
                    GameObject obj = Instantiate(Board_Quest_Prefabs[player_Quest_Num - 1]);
                    QuestBoard_Prefab_List.Add(obj);
                    obj.transform.SetParent(TownQuestBoardSlot);
                    QuestBoard.F_Swich_Maker(1);
                }
                break;
        }
    }


    // << 퀘스트 내용 정리 >>
    // 퀘스트0번 마을찾기
    // 퀘스트1번 약초캐기


    /// <summary>
    ///  퀘스트 진행사항 변경함수
    /// </summary>
    /// <param name="_Quest_ID"> 퀘스트번호 </param>
    /// <param name="_Quest_Num"> 퀘스트 진행사항</param>
    public void F_Set_Quest(int _Quest_ID)
    {
        Cheak_Quest_Acept_Update(_Quest_ID);
    }


    // 0 퀘스트 대기(수락하면) // 1 진행중 채움->2 진행완료 -> 3 완료 -> 4
    // 퀘스트 받았음을 해당 위치에서 켜주는 함수
    // 0~1 그리고 1~2는 각기 다른곳들에서 올려줌

    private void Cheak_Quest_Acept_Update(int value)
    {
        if (PlayerQuest_Situation[value] == 0) //대기중
        {
            PlayerQuest_Situation[value] = 1;
            F_Insert_Ui_QuestList(value);
            StartCoroutine(anim(value)); // 퀘스트 진행시작 UI
            Enable_Quest_Obj(value); // 켜줄께 잇다면 퀘스트 오브젝트 켜줌
        }
        else if (PlayerQuest_Situation[value] == 1) //진행중
        {
            PlayerQuest_Situation[value] = 2;
            //F_Complete_Ui_QuestList(value);
        }
        else if (PlayerQuest_Situation[value] == 2) //진행완료
        {
            PlayerQuest_Situation[value] = 3;
        }
        else if (PlayerQuest_Situation[value] == 3) //퀘스트완료처리 이동
        {
            PlayerQuest_Situation[value] = 4;
            F_Complete_Ui_QuestList(value);
        }
    }

    /// <summary>
    ///  퀘스트번호 넣고확인 Return value <  0 = 수락전 / 1진행중 / 2완료 >
    /// </summary>
    /// <param name="value">퀘스트번호</param>
    /// <returns></returns>
    public int F_Cur_Quest_Chaker(int value)
    {
        return PlayerQuest_Situation[value];
    }

    // 퀘스트진행사항 추적 [외부에서 입력값 받아줌]
    public void F_Quest_Realtime_Update(int value)
    {
        if (Cur_Ea_QuestNum[value] < Max_Ea_QuestNum[value])
        {
            Cur_Ea_QuestNum[value]++;

            if (Cur_Ea_QuestNum[value] == Max_Ea_QuestNum[value])
            {
                F_Set_Quest(value);
                QuestBoard.F_Swich_Maker(0);
            }
        }
    }

    // 퀘스트 열림에따라 오브젝트 활성화
    private void Enable_Quest_Obj(int value)
    {
        switch (value)
        {
            case 1:
                Q1_Obj_Group.SetActive(true);
            break;

            case 2:
                Q2_Obj_Group.SetActive(true);
                break;

        }

    }

    // 퀘스트 갱신UI창 띄움
    IEnumerator anim(int value)
    {
        switch (value)
        {
            case 0:
                questAcceptWindowText.text = $"퀘스트 갱신 : 마을로 찾아 가기";
                break;

            case 1:
                questAcceptWindowText.text = $"퀘스트 갱신 : 초원에서 버섯 캐기";
                break;

            case 2:
                questAcceptWindowText.text = $"퀘스트 갱신 : 호수에서 보물상자 찾기";
                break;

            case 3:
                questAcceptWindowText.text = $"퀘스트 갱신 : 근접 무기 얻기";
                break;

            case 4:
                questAcceptWindowText.text = $"퀘스트 갱신 : 가이드NPC 다시 만나기";
                break;
        }


        if (questAcceptWindow.gameObject.activeSelf == false)
        {
            questAcceptWindow.gameObject.SetActive(true);
        }

        questAcceptWindow.SetBool("On", true);
        yield return DealyWait;
        questAcceptWindow.SetBool("On", false);

        yield return null;
        while (questAcceptWindow.GetCurrentAnimatorStateInfo(0).IsName("Off") && questAcceptWindow.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        questAcceptWindow.gameObject.SetActive(false);
    }

    // 메인화면 UI에 퀘스트 내용 입력
    public void F_Insert_Ui_QuestList(int value)
    {
        GameObject obj = Instantiate(UI_Quest[value]);
        CurPlayQuest.Add(obj);
        obj.transform.SetParent(UI_QuestList);
    }


    // 메인화면 UI 및 퀘스트보더에서 퀘스트 내용 제거 [완료시]
    public void F_Complete_Ui_QuestList(int value)
    {
        
        CurPlayQuest[value].transform.SetParent(CompleteQuestTransform);
        CurPlayQuest[value].gameObject.SetActive(false);

        if (value > 0)  // 0번 마을찾기만 예외
        {
        
            QuestBoard_Prefab_List[value - 1].transform.SetParent(CompleteQuestTransform);
            QuestBoard_Prefab_List[value - 1].gameObject.SetActive(false);
        }

    }

    // 퀘스트창 게시판이 열려있다면  ESC키로 끄는 기능
    private void WindowClose_Input_Chek()
    {
        if (TownQuestBoard.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            TownQuestBoard.gameObject.SetActive(false);
        }
    }

    public int[] F_Return_Quest_Obj_Ea(int value)
    {
        int[] result = new int[2];
        result[0] = Cur_Ea_QuestNum[value];
        result[1] = Max_Ea_QuestNum[value];

        return result;
    }
   
}
