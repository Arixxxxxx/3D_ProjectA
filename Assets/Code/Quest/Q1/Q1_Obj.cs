using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q1_Obj : MonoBehaviour
{
    [SerializeField] float Dis;
    [SerializeField] float PopUpDistance;
    [SerializeField] GameObject Arrow;
    
    Transform Box;
    void Start()
    {
        Box = transform.GetChild(0).GetComponent<Transform>();
        Arrow = transform.Find("IMG/Arrow").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Dis = PlayerMoveController.Inst.F_Get_PlayerAndMeDistance(transform.position);
        
        if(Dis < PopUpDistance && Box.gameObject.activeSelf == false)  
        {
           Box.gameObject.SetActive(true);
           Arrow.gameObject.SetActive(false);
        }
        else if(Dis > PopUpDistance && Box.gameObject.activeSelf == true)
        {
            Box.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(true);
        }
      
        
        if(Box.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.E))
        {
            //Ä³±â
            QuestManager.inst.F_Quest_Realtime_Update(1);
            gameObject.SetActive(false);
        }
        
    }

    
    
}
