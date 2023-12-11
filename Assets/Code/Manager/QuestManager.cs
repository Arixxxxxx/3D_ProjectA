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
    // �÷��̾� ����Ʈ���� ���� �Լ�

    public void F_player_Quest_Num_Up()
    {
        player_Quest_Num++;
    }

    // ## ����Ʈ�� ������� �Լ���

    // ����Ʈ�ѹ� �ö󰡸� ���忡 ����Ʈ �������ְ� ��ư ����
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


    // << ����Ʈ ���� ���� >>
    // ����Ʈ0�� ����ã��
    // ����Ʈ1�� ����ĳ��


    /// <summary>
    ///  ����Ʈ ������� �����Լ�
    /// </summary>
    /// <param name="_Quest_ID"> ����Ʈ��ȣ </param>
    /// <param name="_Quest_Num"> ����Ʈ �������</param>
    public void F_Set_Quest(int _Quest_ID)
    {
        Cheak_Quest_Acept_Update(_Quest_ID);
    }


    // 0 ����Ʈ ���(�����ϸ�) // 1 ������ ä��->2 ����Ϸ� -> 3 �Ϸ� -> 4
    // ����Ʈ �޾����� �ش� ��ġ���� ���ִ� �Լ�
    // 0~1 �׸��� 1~2�� ���� �ٸ����鿡�� �÷���

    private void Cheak_Quest_Acept_Update(int value)
    {
        if (PlayerQuest_Situation[value] == 0) //�����
        {
            PlayerQuest_Situation[value] = 1;
            F_Insert_Ui_QuestList(value);
            StartCoroutine(anim(value)); // ����Ʈ ������� UI
            Enable_Quest_Obj(value); // ���ٲ� �մٸ� ����Ʈ ������Ʈ ����
        }
        else if (PlayerQuest_Situation[value] == 1) //������
        {
            PlayerQuest_Situation[value] = 2;
            //F_Complete_Ui_QuestList(value);
        }
        else if (PlayerQuest_Situation[value] == 2) //����Ϸ�
        {
            PlayerQuest_Situation[value] = 3;
        }
        else if (PlayerQuest_Situation[value] == 3) //����Ʈ�Ϸ�ó�� �̵�
        {
            PlayerQuest_Situation[value] = 4;
            F_Complete_Ui_QuestList(value);
        }
    }

    /// <summary>
    ///  ����Ʈ��ȣ �ְ�Ȯ�� Return value <  0 = ������ / 1������ / 2�Ϸ� >
    /// </summary>
    /// <param name="value">����Ʈ��ȣ</param>
    /// <returns></returns>
    public int F_Cur_Quest_Chaker(int value)
    {
        return PlayerQuest_Situation[value];
    }

    // ����Ʈ������� ���� [�ܺο��� �Է°� �޾���]
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

    // ����Ʈ ���������� ������Ʈ Ȱ��ȭ
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

    // ����Ʈ ����UIâ ���
    IEnumerator anim(int value)
    {
        switch (value)
        {
            case 0:
                questAcceptWindowText.text = $"����Ʈ ���� : ������ ã�� ����";
                break;

            case 1:
                questAcceptWindowText.text = $"����Ʈ ���� : �ʿ����� ���� ĳ��";
                break;

            case 2:
                questAcceptWindowText.text = $"����Ʈ ���� : ȣ������ �������� ã��";
                break;

            case 3:
                questAcceptWindowText.text = $"����Ʈ ���� : ���� ���� ���";
                break;

            case 4:
                questAcceptWindowText.text = $"����Ʈ ���� : ���̵�NPC �ٽ� ������";
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

    // ����ȭ�� UI�� ����Ʈ ���� �Է�
    public void F_Insert_Ui_QuestList(int value)
    {
        GameObject obj = Instantiate(UI_Quest[value]);
        CurPlayQuest.Add(obj);
        obj.transform.SetParent(UI_QuestList);
    }


    // ����ȭ�� UI �� ����Ʈ�������� ����Ʈ ���� ���� [�Ϸ��]
    public void F_Complete_Ui_QuestList(int value)
    {
        
        CurPlayQuest[value].transform.SetParent(CompleteQuestTransform);
        CurPlayQuest[value].gameObject.SetActive(false);

        if (value > 0)  // 0�� ����ã�⸸ ����
        {
        
            QuestBoard_Prefab_List[value - 1].transform.SetParent(CompleteQuestTransform);
            QuestBoard_Prefab_List[value - 1].gameObject.SetActive(false);
        }

    }

    // ����Ʈâ �Խ����� �����ִٸ�  ESCŰ�� ���� ���
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
