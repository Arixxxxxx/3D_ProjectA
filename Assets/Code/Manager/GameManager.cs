using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    private bool playerRotStop;

    private bool rotStopRequest;
    private bool moveStopRequest;
    private bool gameStopRequest;
    private bool isCursorOn;
    private bool inHomeTown;
    public bool InHomeTown {  get { return inHomeTown; } set { inHomeTown = value; } }

    private void Awake()
    {
        AwakeSingleTone();
    }
    private void Update()
    {
        OnScrrenCursurOnOFF();
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

    
}
