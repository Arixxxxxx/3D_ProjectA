using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPreFabs : MonoBehaviour
{
    public enum QuestNumber
    {
        Q1, Q2, Q3
    }
    public QuestNumber type;

    private Button QuestBoardButton;

    private void Awake()
    {
        QuestBoardButton = transform.Find("Q_1/Button").GetComponent<Button>();
        
    }
    void Start()
    {
        QuestBoardButton.onClick.AddListener(() => { QuestManager.inst.F_Set_Quest(1); });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
