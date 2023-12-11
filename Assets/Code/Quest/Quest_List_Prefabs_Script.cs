using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Quest_List_Prefabs_Script : MonoBehaviour
{
    public enum Quest_Number
    {
        Q1, Q2, Q3, Q4  
    }
    public Quest_Number type;

    TMP_Text quest_List_Text;
    int Quest_Num; // 1약초/ 2호수 / 3상점 / 4 근접무기 테스트 / 5원거리무기 /  6캐논 / 7보스
    int Num_Value; // 0 대기중 / 1 진행중 / 2완료
    int[] getValue = new int[2];
    int Quest_Cur_Value;
    int Quest_Max_Value;


    void Start()
    {
        quest_List_Text = GetComponent<TMP_Text>();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        Insert_Quest_Value();
        Quest_Ui_Text_Updater();

        if (type == Quest_Number.Q2 || type == Quest_Number.Q3)
        {
            RealTime_Quest_Obj_Update();
        }
    }
    private void init()
    {
        switch (type)
        {
            case Quest_Number.Q1:
                Quest_Num = 1;
                break;

            case Quest_Number.Q2:
                Quest_Num = 2;
                break;

            case Quest_Number.Q3:
                Quest_Num = 3;
                break;

            case Quest_Number.Q4:
                Quest_Num = 4;
                break;

        }
    }

    private void RealTime_Quest_Obj_Update()
    {
        getValue = QuestManager.inst.F_Return_Quest_Obj_Ea(Quest_Num);
        Quest_Cur_Value = getValue[0];
        Quest_Max_Value = getValue[1];
    }


    // 퀘스트 진행상황 실시간체크
    private void Insert_Quest_Value()
    {
        Num_Value = QuestManager.inst.F_Cur_Quest_Chaker(Quest_Num);
    }

    // 퀘스트 UI 업데이트
    private void Quest_Ui_Text_Updater()
    {
        switch (type)
        {
            case Quest_Number.Q1:

                if (Num_Value == 1)
                {
                    quest_List_Text.text = $"  - 초원에서 버섯 캐기 ( {Quest_Cur_Value} / {Quest_Max_Value} )";
                }

                if (Num_Value == 2)
                {
                    quest_List_Text.text = $"  - 초원에서 버섯 캐기 <color=yellow><b>( 완료 )</b></color>";
                }

                break;

            case Quest_Number.Q2:

                if (Num_Value == 1)
                {
                    quest_List_Text.text = $"  - 호수에서 보물상자 찾기 ( {Quest_Cur_Value} / {Quest_Max_Value} )";
                }

                if (Num_Value == 2)
                {
                    quest_List_Text.text = $"  - 호수에서 보물상자 찾기 <color=yellow><b>( 완료 )</b></color>";
                }

                break;

            case Quest_Number.Q3:

                if (Num_Value == 1)
                {
                    quest_List_Text.text = $"  - 대장장이에게 무기 구매";
                }

                break;

            case Quest_Number.Q4:

                if (Num_Value == 1)
                {
                    quest_List_Text.text = $"  - 마을밖 가이드NPC 찾아가기";
                }

                break;

        }
    }
}
