using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    [SerializeField] GameObject[] InsertWindow_Obj;

    private GameObject water_Obj;
    public GameObject Water_Obj { get { return water_Obj; } set { water_Obj = value; } }

    private bool playerRotStop;
    private bool rotStopRequest;
    private bool moveStopRequest;
    private bool gameStopRequest;
    private bool isCursorOn;
    private bool inHomeTown;
    [SerializeField] private bool isWindowOpen;
    [SerializeField] NPC_Talk_Num Unity_Chan;

    private UnityAction<bool> readyAction;

    public bool IsWindowOpen {
        set
        {
            isWindowOpen = value;
            readyAction?.Invoke(IsWindowOpen);
        }
        get 
        {
            return isWindowOpen; 
        }
    }

    public void SetAction(UnityAction<bool> _action)
    {
        readyAction = _action;
    }

    public bool InHomeTown {  get { return inHomeTown; } set { inHomeTown = value; } }

    private void Awake()
    {
        AwakeSingleTone();
    }
    private void Update()
    {
        OnScrrenCursurOnOFF();
        GameUI_Or_WindowOn_Cheaker();
    }
    private void AwakeSingleTone()
    {
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    private void GameUI_Or_WindowOn_Cheaker()
    {
        if (InsertWindow_Obj[0].gameObject.activeSelf == true || InsertWindow_Obj[1].gameObject.activeSelf == true || InsertWindow_Obj[2].gameObject.activeSelf == true)
        {
            IsWindowOpen = true;
        }
        else
        {
            IsWindowOpen = false;
        }
    }
    private void OnScrrenCursurOnOFF()
    {
        if (inHomeTown) { Cursor.lockState = CursorLockMode.None; return; }
        if (isCursorOn)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (IsWindowOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void F_SetMouseScrrenRotationStop(bool value)
    {
        playerRotStop = value;
    }
    public void F_SetScreenCursorOnOFF(bool value)
    {
        isCursorOn = value;
    }
    public bool F_GetMouseScrrenRotationStop()
    {
        return  playerRotStop;
    }

   [SerializeField] bool no;
    public bool NoChangeMode {  get { return no; } }
    public void F_ModeChangeNo(bool value)
    {
        no = value;
     }
    
    public bool F_Windows_Popup()
    {
        return isWindowOpen;
    }

    /// <summary>
    /// Npc ID/TalkNum 증가 매개변수 0=ID / 
    /// </summary>
    /// <param name="value"></param>
    public void F_NPC_TalkNum_Up(int value)
    {
        Unity_Chan.F_ValueUpdate(value);
    }
}
