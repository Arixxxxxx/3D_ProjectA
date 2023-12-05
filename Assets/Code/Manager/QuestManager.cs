using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager inst;
    [SerializeField] GameObject[] UI_Quest;
    [SerializeField] Transform UI_QuestList;
    [SerializeField] GameObject QuestWindow;

    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WindowClose_Input_Chek();
    }

    private void WindowClose_Input_Chek()
    {
        if(QuestWindow.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            QuestWindow.gameObject.SetActive(false);
        }
    }

    public void F_Insert_Ui_QuestList(int value)
    {
        switch (value) 
        {
           case 0:
                GameObject obj = Instantiate(UI_Quest[0]);
                obj.transform.SetParent(UI_QuestList);
                break;
        }

    }

    public void F_Complete_Ui_QuestList(int value)
    {
        switch (value)
        {
            case 0:
                UI_Quest[0].transform.SetParent(transform);
                break;
        }

    }
}
