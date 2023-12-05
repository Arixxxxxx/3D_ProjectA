using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RawImageRotation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GameObject Character_Avatar;
    [SerializeField] float spinSpeed;
    [SerializeField] float returnSpeed;
    [SerializeField] Vector3 cur_rot;
    [SerializeField] Vector3 End_rot;
    [SerializeField] Vector3 OriginRot;
    [SerializeField] bool returnRot;
    private void Start()
    {
        cur_rot = Character_Avatar.transform.localEulerAngles;
        OriginRot = Character_Avatar.transform.localEulerAngles;
    }

    private void Update()
    {
        if (returnRot)
        {
            if(End_rot.y > OriginRot.y)
            {
                //End_rot.y -= Time.deltaTime * returnSpeed;
                End_rot.y = Mathf.Lerp(End_rot.y, OriginRot.y, Time.deltaTime * 2);
                Character_Avatar.transform.localEulerAngles = End_rot;
            }
           else if(End_rot.y < OriginRot.y)
            {
                //End_rot.y += Time.deltaTime *returnSpeed;
                End_rot.y = Mathf.Lerp(End_rot.y, OriginRot.y, Time.deltaTime * 2);
                Character_Avatar.transform.localEulerAngles = End_rot;
            }
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        returnRot = false;
    
    }

    public void OnDrag(PointerEventData eventData)
    {
        cur_rot.y += Input.GetAxis("Mouse X") * spinSpeed;
        Character_Avatar.transform.localEulerAngles = -cur_rot;
    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
        End_rot = Character_Avatar.transform.localEulerAngles;
        Invoke("ReturnRot_True_Invoke_Only", 0.3f);
     }
    private void ReturnRot_True_Invoke_Only()
    {
        returnRot = true;
    }
}
