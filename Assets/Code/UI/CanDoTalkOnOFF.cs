using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDoTalkOnOFF : MonoBehaviour
{
    [SerializeField] GameObject talkbox;
    [SerializeField] float playerDistance;
    [SerializeField] float box_ActiveTrue_Value;
    [SerializeField] float box_ActiveFlase_Value;


    PlayerMoveController Player;
    QuestManager QuestManager;
    TalkData player_npc;
    void Start()
    {
        Player = PlayerMoveController.Inst;
        player_npc = QuestManager.inst.transform.GetComponent<TalkData>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(Player.F_Get_PlayerCurPos(), transform.position);

        if(playerDistance <= box_ActiveTrue_Value && talkbox.activeSelf == false)
        {
            talkbox.gameObject.SetActive(true);
            player_npc.F_Set_Player_Near_Npc(gameObject, true);
            
        }
        else if(playerDistance > box_ActiveFlase_Value && talkbox.activeSelf == true)
        {
            talkbox.gameObject.SetActive(false);
            player_npc.F_Set_Player_Near_Npc(gameObject, false);
        }
    }

    
}
