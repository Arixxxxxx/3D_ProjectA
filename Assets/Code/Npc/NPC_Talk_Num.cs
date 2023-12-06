using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Talk_Num : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] int TalkNum;
    [SerializeField] int Quest_Num;
    [SerializeField] GameObject Quest_start;
    [SerializeField] GameObject Quest_comeplete;

    void Start()
    {
     
    }

    /// <summary>
    /// ID / TalkNum / Quest_Num 증가
    /// </summary>
    /// <param name="value"> 0 = ID / TalkNum = 1 / Quest_Num2</param>
    public void F_ValueUpdate(int value)
    {
        switch (value)
        {
            case 0:
                ID++;
                break;
            case 1:
                TalkNum++;
                break;

            case 2:
                Quest_Num++;
                break;

        }
    }

    public int F_Get_NPC_TalkID()
    {
        return ID;
    }


    /// <summary>
    /// NPC 머리위 퀘스트마커 스위치
    /// </summary>
    /// <param name="value"> 0 느낌표 / 1물음표 / 2 없음</param>
    public void F_Swithing_QuestMarker(int value)
    {
        switch (value)
        {
            case 0:
                Quest_start.SetActive(true);
                Quest_comeplete.SetActive(false);
                break;

            case 1:
                Quest_start.SetActive(false);
                Quest_comeplete.SetActive(true);
                break;

            case 2:
                Quest_start.SetActive(false);
                Quest_comeplete.SetActive(false);
                break;

        }

    }
}
