using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Inst;

    [SerializeField]  Animator blackScrrenAnim;
    [SerializeField]  Animator QuestCompleteAnim;
    [SerializeField]  Image QeustCompleteBosang;
    [SerializeField] TMP_Text MiddleText;

    [SerializeField] Sprite[] QuestBosang_IMG;
    [Header("# Place Name Art")]
    [SerializeField]  Animator PlaceNameAnim;
    [SerializeField] TMP_Text PlaceNameText;
    [SerializeField] float Dealy;
    WaitForSeconds PlaceNameArtDealyWait;

    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        PlaceNameArtDealyWait = new WaitForSeconds(Dealy);
        F_PlaceNameArt_OnOFF(0);
    }

    // Update is called once per frame
    void Update()
    {
     
        
    }
   
    // ���� �̸�ǥ ���
    public void F_PlaceNameArt_OnOFF(int value)
    {
        StopCoroutine(PlaceName());
        switch (value)
        {
            case 0:
                PlaceNameText.text = "������ �ʿ�"; // �̰����� �̸� �ۼ�
                break;

            case 1:
                PlaceNameText.text = "����"; 
                break;
        }

        StartCoroutine(PlaceName());
    }

    
    IEnumerator PlaceName()
    {
        if(PlaceNameAnim.gameObject.activeSelf == false) 
        {
            PlaceNameAnim.gameObject.SetActive(true);  
        }

        PlaceNameAnim.SetBool("Hide", true);
        yield return PlaceNameArtDealyWait;
        PlaceNameAnim.SetBool("Hide", false);

        yield return null;
        while (PlaceNameAnim.GetCurrentAnimatorStateInfo(0).IsName("Hide 1") && PlaceNameAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        PlaceNameAnim.gameObject.SetActive(false);
    }
    public void F_BlackScrrenOn()
    {
        blackScrrenAnim.SetTrigger("On");
    }


    //����Ʈ�Ϸ� â ��Ʈ��
    public void F_QuestComplete_UI_Open(int Quest_ID)
    {

        switch (Quest_ID)
        {
            case 0:
                QeustCompleteBosang.sprite = QuestBosang_IMG[0];
                MiddleText.text = "< ������ �̵� ����Ʈ >";

                break;

            case 1:
                QeustCompleteBosang.sprite = QuestBosang_IMG[0];
                MiddleText.text = "< ���� 10�� ��Ȯ! >";
                break;
        }

        if (QuestCompleteAnim.gameObject.activeSelf == false)
        {
            QuestCompleteAnim.gameObject.SetActive(true);
        }

        QuestCompleteAnim.SetTrigger("On");


    }
  
}
