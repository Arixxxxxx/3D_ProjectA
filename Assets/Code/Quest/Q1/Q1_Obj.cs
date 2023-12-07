using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q1_Obj : MonoBehaviour
{
    public enum QuestObj_Num
    {
        Q1, Q2, Q3, Q4
    }
    public QuestObj_Num type;

    [SerializeField] int QuestNum;
    [SerializeField] float Dis;
    [SerializeField] float PopUpDistance;
    [SerializeField] GameObject Arrow;
    

    Transform Box;
    void Start()
    {
        init();
        Box = transform.GetChild(0).GetComponent<Transform>();
        Arrow = transform.Find("IMG/Arrow").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Dis = PlayerMoveController.Inst.F_Get_PlayerAndMeDistance(transform.position);

        if (Dis < PopUpDistance && Box.gameObject.activeSelf == false)
        {
            Box.gameObject.SetActive(true);
            Arrow.gameObject.SetActive(false);
        }
        else if (Dis > PopUpDistance && Box.gameObject.activeSelf == true)
        {
            Box.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(true);
        }


        if (Box.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.E))
        {
            //Ä³±â
            QuestManager.inst.F_Quest_Realtime_Update(QuestNum);
            gameObject.SetActive(false);
        }

    }


    private void init()
    {
        switch (type)
        {
            case QuestObj_Num.Q1:
                QuestNum = 1;
                break;

            case QuestObj_Num.Q2:
                QuestNum = 2;
                break;

            case QuestObj_Num.Q3:
                QuestNum = 3;
                break;


        }
    }


}
