using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player_Cur_Inventory : MonoBehaviour
{
    [SerializeField] int player_Coin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int F_Get_PlayerCoin()
    {
        return player_Coin;
    }

    /// <summary>
    /// True 입금 // False 차감
    /// </summary>
    /// <param name="_BValue"></param>
    /// <param name="_Value"></param>
    public void F_AddCoin(bool _BValue, int _Value)
    {
        if(_BValue == true)
        {
            player_Coin += _Value;
        }
        else
        {
            player_Coin -= _Value;
        }
        
    }
}
