using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Quest_List_Prefabs_Script : MonoBehaviour
{
    public enum Quest_Number
    {
        Q1, Q2, Q3
    }
    public Quest_Number type;

    TMP_Text quest_List_Text;
    int Quest_Num; // 1����/
    int Num_Value; // 0 ����� / 1 ������ / 2�Ϸ�
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
        RealTime_Quest_Obj_Update();


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

        }
    }

    private void RealTime_Quest_Obj_Update()
    {
        getValue = QuestManager.inst.F_Return_Quest_Obj_Ea(Quest_Num);
        Quest_Cur_Value = getValue[0];
        Quest_Max_Value = getValue[1];
    }


    // ����Ʈ �����Ȳ �ǽð�üũ
    private void Insert_Quest_Value()
    {
        Num_Value = QuestManager.inst.F_Cur_Quest_Chaker(Quest_Num);
    }

    // ����Ʈ UI ������Ʈ
    private void Quest_Ui_Text_Updater()
    {
        switch (type)
        {
            case Quest_Number.Q1:

                if (Num_Value == 1)
                {
                    quest_List_Text.text = $"  - �ʿ����� ���� ĳ�� ( {Quest_Cur_Value} / {Quest_Max_Value} )";
                }

                if (Num_Value == 2)
                {
                    quest_List_Text.text = $"  - �ʿ����� ���� ĳ�� <color=yellow><b>( �Ϸ� )</b></color>";
                }

                break;

            case Quest_Number.Q2:

                if (Num_Value == 1)
                {
                    quest_List_Text.text = $"  - ȣ������ �������� ã�� ( {Quest_Cur_Value} / {Quest_Max_Value} )";
                }

                if (Num_Value == 2)
                {
                    quest_List_Text.text = $"  - ȣ������ �������� ã�� <color=yellow><b>( �Ϸ� )</b></color>";
                }

                break;

        }
    }
}
