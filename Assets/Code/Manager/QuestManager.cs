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

    Transform CompleteQuestTransform;

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
    private void Insert_Quest_Board_Prefabs()
    {
        switch(player_Quest_Num)
        {
            case 1:
                if (!Quest1Start)
                {
                    Quest1Start = true;
                    GameObject obj = Instantiate(Board_Quest_Prefabs[player_Quest_Num - 1]);
                    obj.transform.SetParent(TownQuestBoardSlot);
                }
                break;
        }
    }

    /// <summary>
    ///  퀘스트 진행사항 변경함수
    /// </summary>
    /// <param name="_Quest_ID"> 퀘스트번호 </param>
    /// <param name="_Quest_Num"> 퀘스트 진행사항</param>
    public void F_Set_Quest(int _Quest_ID)
    {
        //for (int i = 0; i < QuestCount; i++)
        //{
        //    if (i == _Quest_ID)
        //    {
        //        PlayerQuest_Situation[i] = _Quest_Num;

        //    }
        //}

        Cheak_Quest_Acept_Update(_Quest_ID);

    }


    // 퀘스트 받았음을 해당 위치에서 켜주는 함수
    // 0~1 그리고 1~2는 각기 다른곳들에서 올려줌

    private void Cheak_Quest_Acept_Update(int value)
    {
        if (PlayerQuest_Situation[value] == 0) //수락
        {
            PlayerQuest_Situation[value] = 1;
            F_Insert_Ui_QuestList(value);
            StartCoroutine(anim(value));
        }
        else if (PlayerQuest_Situation[value] == 1)
        {
            PlayerQuest_Situation[value] = 2;
            F_Complete_Ui_QuestList(value);
        }
    }

    IEnumerator anim(int value)
    {
        switch (value)
        {
            case 0:
                questAcceptWindowText.text = $"퀘스트 갱신 : 마을로 찾아 가기";
                break;

            case 1:
                questAcceptWindowText.text = $"퀘스트 갱신 : 초원에서 약초 캐기";
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
        switch (value)
        {
            case 0:
                GameObject obj = Instantiate(UI_Quest[0]);
                CurPlayQuest.Add(obj);
                obj.transform.SetParent(UI_QuestList);
                break;

            case 1:
                GameObject obj1 = Instantiate(UI_Quest[1]);
                CurPlayQuest.Add(obj1);
                obj1.transform.SetParent(UI_QuestList);
                break;
        }

    }


    // 메인화면 UI에서 퀘스트 내용 제거 [완료시]
    public void F_Complete_Ui_QuestList(int value)
    {
        CurPlayQuest[value].transform.SetParent(CompleteQuestTransform);
        CurPlayQuest[value].gameObject.SetActive(false);
    }

    // 퀘스트창 게시판이 열려있다면  ESC키로 끄는 기능
    private void WindowClose_Input_Chek()
    {
        if (TownQuestBoard.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            TownQuestBoard.gameObject.SetActive(false);
        }
    }
}
