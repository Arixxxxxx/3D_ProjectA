using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiBoxHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log("aa");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log("cc");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
