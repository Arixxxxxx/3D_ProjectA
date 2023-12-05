using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Talk_Num : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] int TalkNum;
    [SerializeField] int Quest_Num;
    
    void Start()
    {
        
    }

    /// <summary>
    /// ID / TalkNum / Quest_Num ¡ı∞°
    /// </summary>
    /// <param name="value"> 0 = ID / TalkNum = 1 / Quest_Num2</param>
    public void F_ValueUpdate(int value)
    {
        switch(value)
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
        return  ID;
    }

}
