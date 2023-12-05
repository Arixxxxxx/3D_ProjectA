using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkBox_Text_Effect : MonoBehaviour
{
    TMP_Text mainText;
    [SerializeField] float Speed;

    private void Awake()
    {
        mainText = GetComponent<TMP_Text>();
    }

    string GetText;
    string ConfirmString;
    [SerializeField] int index;
    public void F_Set_TalkBox_Main_Text(string value)
    {
        GetText = value;
        mainText.text = string.Empty;
        Insert_Word();
    }


    private void Insert_Word()
    {
        if (mainText.text == GetText)
        {
            End_Text();
            return;
        }

        mainText.text += GetText[index];
        index ++;

       


        Invoke("Insert_Word", 1 / Speed);
    }

    private void End_Text()
    {
        index = 0;
        TalkData.inst.F_IsNextOk();
    }
}
