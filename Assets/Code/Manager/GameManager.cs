using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    [SerializeField] GameObject[] windowCheker;

    private GameObject water_Obj;
    public GameObject Water_Obj { get { return water_Obj; } set { water_Obj = value; } }

    private bool playerRotStop;
    private bool rotStopRequest;
    private bool moveStopRequest;
    private bool gameStopRequest;
    private bool isCursorOn;
    private bool inHomeTown;
    private bool isWindowOpen;
    public bool IsWindowOpen { get { return isWindowOpen; }}
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
        if (windowCheker[0].gameObject.activeSelf == true)
        {
            isWindowOpen = true;
        }
        else
        {
            isWindowOpen = false;
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

   [SerializeField] bool no;
    public bool NoChangeMode {  get { return no; } }
    public void F_ModeChangeNo(bool value)
    {
        no = value;
     }
    
}
