using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPreFabs : MonoBehaviour
{
    public enum QuestNumber
    {
        Q1, Q2, Q3, Q4, Q5, Q6, Q7, Q8,
    }
    public QuestNumber type;

    [SerializeField] int QeustNum;
    [SerializeField] int Cur_Value;
    private Button QuestBoardButton;
    [SerializeField] private GameObject WaitMaker;
    [SerializeField] private GameObject Quest_Ing;
    [SerializeField] private GameObject Complete;
    [SerializeField] private QuestBoard Board;
    TMP_Text mainText;

    private void Awake()
    {
        QuestBoardButton = transform.Find("Q_1/Button").GetComponent<Button>();
        WaitMaker = transform.Find("Q_1/IMG/!").gameObject;
        Quest_Ing = transform.Find("Q_1/IMG/?").gameObject;
        Complete = transform.Find("Q_1/IMG/Complete").gameObject;
        mainText = QuestBoardButton.transform.Find("Main_Text").GetComponent<TMP_Text>();
        Board = FindAnyObjectByType<QuestBoard>();
    }
    void Start()
    {
        init();
        switch (type)
        {
            case QuestNumber.Q1:
            case QuestNumber.Q2:

                QuestBoardButton.onClick.AddListener(() =>
                {
                    QuestManager.inst.F_Set_Quest(QeustNum);
                    QuestBoardButton.interactable = false;
                    Board.F_Swich_Maker(2);
                    GameManager.Inst.F_NPC_TalkNum_Up(0);
                });

                break;


            case QuestNumber.Q3:

                QuestBoardButton.onClick.AddListener(() =>
                {
                    QuestManager.inst.F_Set_Quest(QeustNum);
                    QuestBoardButton.interactable = false;
                    Board.F_Swich_Maker(2);
                });

                break;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case QuestNumber.Q1:
            case QuestNumber.Q2:

                Cur_Value = QuestManager.inst.F_Cur_Quest_Chaker(QeustNum);

                if (Cur_Value == 0 && WaitMaker.gameObject.activeSelf == false)
                {
                    WaitMaker.gameObject.SetActive(true);
                }
                else if (Cur_Value == 1 && Quest_Ing.gameObject.activeSelf == false)
                {
                    WaitMaker.gameObject.SetActive(false);
                    Quest_Ing.gameObject.SetActive(true);
                }
                else if (Cur_Value == 2 && Complete.gameObject.activeSelf == false) // 퀘스트 진행사항 완료
                {
                    WaitMaker.gameObject.SetActive(false);
                    Quest_Ing.gameObject.SetActive(false);
                    Complete.gameObject.SetActive(true);
                    mainText.text = $" 퀘스트 완료 - 보상받기 (클릭)";
                    QuestBoardButton.interactable = true;

                    QuestBoardButton.onClick.AddListener(() =>
                    {
                        QuestManager.inst.F_Set_Quest(QeustNum);
                        GameUIManager.Inst.F_QuestComplete_UI_Open(QeustNum);
                        QuestManager.inst.F_player_Quest_Num_Up();
                    });
                }
                break;

            case QuestNumber.Q3:

                Cur_Value = QuestManager.inst.F_Cur_Quest_Chaker(QeustNum);

                if (Cur_Value == 0 && WaitMaker.gameObject.activeSelf == false)
                {
                    WaitMaker.gameObject.SetActive(true);
                }
                else if (Cur_Value == 1 && Quest_Ing.gameObject.activeSelf == false)
                {
                    WaitMaker.gameObject.SetActive(false);
                    Quest_Ing.gameObject.SetActive(true);
                }
                else if (Cur_Value == 2 && Complete.gameObject.activeSelf == false) // 퀘스트 진행사항 완료
                {
                    WaitMaker.gameObject.SetActive(false);
                    Quest_Ing.gameObject.SetActive(false);
                    Complete.gameObject.SetActive(true);
                    mainText.text = $" 퀘스트 완료 - 보상받기 (클릭)";
                    QuestBoardButton.interactable = true;

                    QuestBoardButton.onClick.AddListener(() =>
                    {
                        QuestManager.inst.F_Set_Quest(QeustNum);
                        GameUIManager.Inst.F_QuestComplete_UI_Open(QeustNum);
                        QuestManager.inst.F_player_Quest_Num_Up();
                    });
                }
                break;
        }


       
    }

    private void init()
    {
        switch (type)
        {
            case QuestNumber.Q1:
                QeustNum = 1;
                break;

            case QuestNumber.Q2:
                QeustNum = 2;
                break;

            case QuestNumber.Q3:
                QeustNum = 3;
                break;

            case QuestNumber.Q4:
                QeustNum = 4;
                break;

            case QuestNumber.Q5:
                QeustNum = 5;
                break;

        }
    }

}
