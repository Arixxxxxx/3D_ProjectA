using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDoTalkOnOFF : MonoBehaviour
{
    [SerializeField] GameObject talkbox;
    [SerializeField] float playerDistance;
    [SerializeField] float box_ActiveTrue_Value;
    [SerializeField] float box_ActiveFlase_Value;
    [SerializeField] float Hand_Rasing_Distance;

    [Header("# Talking Enable false Obj")]
    [SerializeField] GameObject TalkCam;
    [SerializeField] GameObject QuestMaker;

    Animator anim;
    PlayerMoveController Player;
    QuestManager QuestManager;
    TalkData player_npc;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        Player = PlayerMoveController.Inst;
        player_npc = QuestManager.inst.transform.GetComponent<TalkData>();
    }
    
    bool once;
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

        if(playerDistance < Hand_Rasing_Distance && once == false)
        {
            once = true;
            transform.LookAt(Player.transform);
            anim.SetTrigger("Hello");
        }
        else if(playerDistance > Hand_Rasing_Distance && once == true)
        {
            once = false;
        }

        if(TalkCam.gameObject.activeSelf == true && QuestMaker.gameObject.activeSelf == true) 
        {
         QuestMaker.gameObject.SetActive(false);
        }
        else if(TalkCam.gameObject.activeSelf == false && QuestMaker.gameObject.activeSelf == false)
        {
            QuestMaker.gameObject.SetActive(true);
        }
    }

    
}
