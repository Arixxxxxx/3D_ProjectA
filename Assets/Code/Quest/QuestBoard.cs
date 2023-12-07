using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{

    [SerializeField] int CheakQuest;
    [SerializeField] float popupDistance;
    [SerializeField] float playerAndMeDistance;

    GameObject haveQuestMaker;
    GameObject completeQuestMaker;
    private void Start()
    {
        haveQuestMaker = transform.Find("Canvas/!").gameObject;
        completeQuestMaker = transform.Find("Canvas/?").gameObject;
    }

    private void Update()
    {
        Cheack_Distance();
     
    }
    private void Cheack_Distance()
    {
        playerAndMeDistance = PlayerMoveController.Inst.F_Get_PlayerAndObject_Distance(transform.position);
    }

    
    /// <summary>
    /// 마을에 퀘스트보드 마커 컨트롤 0완료 / 1 느낌표 / 없음
    /// </summary>
    /// <param name="value"></param>
    public void F_Swich_Maker(int value)
    {
        switch (value)
        {
            case 0:
                
                if(completeQuestMaker.activeSelf == true) { return; }

                haveQuestMaker.SetActive(false);
                completeQuestMaker.SetActive(true);
            break;

                case 1:
                if (haveQuestMaker.activeSelf == true) { return; }

                haveQuestMaker.SetActive(true);
                completeQuestMaker.SetActive(false);
                break;

            case 2:
                if (haveQuestMaker.activeSelf == false) { return; }

                haveQuestMaker.SetActive(false);
                completeQuestMaker.SetActive(false);
                break;
        }

    }

}
