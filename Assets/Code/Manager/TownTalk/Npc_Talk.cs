using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Npc_Talk : MonoBehaviour
{
    [SerializeField] float talk_Duration;
    [SerializeField] float talkBox_Open_Dealy;
    WaitForSeconds talk_Time;
    WaitForSeconds open_Dealy;
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text maintext;
        void Start()
    {
      
        talk_Time = new WaitForSeconds(talk_Duration);
        open_Dealy = new WaitForSeconds(talkBox_Open_Dealy);
    }
    public void F_Talk_NPC(string text)
    {
        if(anim.gameObject.activeSelf == false)
        {
            anim.gameObject.SetActive(true);
        }
        anim.gameObject.transform.localScale = new Vector3(0, 0, 1);

        StopCoroutine(Start_Talk(text));
        StartCoroutine(Start_Talk(text));
    }

    IEnumerator Start_Talk(string text)
    {
        yield return open_Dealy;

        maintext.text = text;
        anim.SetTrigger("Look");
        yield return talk_Time;
        anim.gameObject.transform.localScale = new Vector3(0, 0, 1);
        anim.gameObject.SetActive(false);
       
    }
}
