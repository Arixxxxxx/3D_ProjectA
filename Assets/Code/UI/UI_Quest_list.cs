using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Quest_list : MonoBehaviour
{
    [SerializeField]  GameObject UI_Quest_List;
    Transform Content;

    private void Awake()
    {
        Content = UI_Quest_List.transform.Find("Viewport/Content").GetComponent<Transform>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Content.transform.childCount == 0)
        {
            UI_Quest_List.gameObject.SetActive(false);
        }
        else if(Content.transform.childCount > 0)
        {
            UI_Quest_List.gameObject.SetActive(true);
        }
    }
}
