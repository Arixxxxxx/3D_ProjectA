using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager inst;
    [SerializeField] PlayerNavMeshSC navsc;
    [SerializeField] CinemachineVirtualCamera[] Cams;
    [SerializeField] Camera TownCam;
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject ExitTarget;
    [SerializeField] int curCamNum;
    PlayerMoveController player;
    [SerializeField] bool inDownTown;
    [SerializeField] float rayDis;
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }
        curCamNum = 0;
        player = FindAnyObjectByType<PlayerMoveController>();
    }

    private void Start()
    {
        mainCam = Camera.main;
    }
    private void Update()
    {
        TownCamPosition();
        CameraRotStop();
        LockOnCameraRotationSetting();
        mouseWhellZoomInOut();
        MainCamGroundCheker();
    }
    float Timer;
    Vector3 stopVec;


    bool isGround;
    bool once;
    float PosY;

    private void MainCamGroundCheker()
    {
        isGround = Physics.Raycast(mainCam.transform.position, Vector3.down, out RaycastHit hit, rayDis, LayerMask.GetMask("Ground"));

        if (isGround == true)
        {
            if (!once)
            {
                once = true;
                PosY = mainCam.transform.position.y;
            }

            PosY = Mathf.Max(PosY, mainCam.transform.position.y);
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, PosY, mainCam.transform.position.z);

            // 카메라 VerValue값 가져와서 변경해줘야할듯
            //Debug.Log("11"); anim.SetLayerWeight(1, 1);
        }
        else if (isGround == false)
        {
            once = false;
            PosY = 0;
        }
    }
    //캐릭터 휠전환으로  줌인아웃 가능
    float townCamMathFloat;
    private void mouseWhellZoomInOut()
    {
        float whellF = Input.GetAxis("Mouse ScrollWheel");
        Timer += Time.deltaTime;

        if (whellF < 0 && Timer > 0.25f)
        {
            Timer = 0;
            switch (curCamNum)
            {
                case 0:
                    Cams[0].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += 1f;
                    break;
                case 2:
                    TownCam.fieldOfView += 1;
                    break;
                case 3:
                    Cams[2].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += 1f;
                    break;


            }
        }
        if (whellF > 0 && Timer > 0.25f)
        {
            Timer = 0;
            switch (curCamNum)
            {
                case 0:
                    Cams[0].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= 1f;
                    break;
                case 2:
                    TownCam.fieldOfView -= 1;
                    //StopCoroutine(UpMathf(false));
                    //StartCoroutine(UpMathf(false));
                    break;
                case 3:
                    Cams[2].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= 1f;
                    break;

            }
        }

    }

    IEnumerator UpMathf(bool value)
    {
        townCamMathFloat = 0;

        if (value == true)
        {

            while (townCamMathFloat < 1.95f)
            {
                townCamMathFloat = Mathf.Lerp(0, 2, Time.deltaTime);
                TownCam.fieldOfView += townCamMathFloat;
                yield return null;
            }
        }
        else
        {
            while (townCamMathFloat < 1.95f)
            {
                townCamMathFloat = Mathf.Lerp(0, 2, Time.deltaTime);
                TownCam.fieldOfView -= townCamMathFloat;
                yield return null;
            }
        }
    }
    private void LockOnCameraRotationSetting()
    {
        if (Cams[2].gameObject.activeSelf == true)
        {
            Transform PlayerPos = Cams[2].Follow;
            Cams[2].GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = PlayerPos.eulerAngles.y;

        }

    }
    /// <summary>
    /// 카메라 체인져 / 0번 3인칭 / 1번 1인칭 / 2번 마을 / 3번 타겟팅
    /// </summary>
    /// <param name="value">0번 3인칭 / 1번 1인칭 / 2번 마을 / 3번 타겟팅</param>
    public void F_ChangeCam(int value)
    {
        curCamNum = value;

        switch (value)
        {
            case 0: // 일반 3인칭 카메라
                Cams[0].gameObject.SetActive(true);
                Cams[1].gameObject.SetActive(false);
                Cams[2].gameObject.SetActive(false);
                TownCam.gameObject.SetActive(false);
                break;
            case 1: // 총쏠때 1인칭 에임카메라
                Cams[0].gameObject.SetActive(false);
                Cams[1].gameObject.SetActive(true);
                Cams[2].gameObject.SetActive(false);
                TownCam.gameObject.SetActive(false);
                break;
            case 2: // 마을 쿼터뷰 카메라
                Cams[0].gameObject.SetActive(false);
                Cams[1].gameObject.SetActive(false);
                Cams[2].gameObject.SetActive(false);
                TownCam.gameObject.SetActive(true);
                break;
            case 3: // 타겟팅 카메라
                Cams[0].gameObject.SetActive(false);
                Cams[1].gameObject.SetActive(false);
                Cams[2].gameObject.SetActive(true);
                TownCam.gameObject.SetActive(false);
                break;
        }
    }



    private void CameraRotStop()
    {
        if (GameManager.Inst.F_GetMouseScrrenRotationStop() == true)
        {
            for (int i = 0; i < Cams.Length; i++)
            {
                Cams[i].GetCinemachineComponent<CinemachinePOV>().enabled = false;
            }

        }
        else if (GameManager.Inst.F_GetMouseScrrenRotationStop() == false)
        {
            for (int i = 0; i < Cams.Length; i++)
            {
                Cams[i].GetCinemachineComponent<CinemachinePOV>().enabled = true;
            }
        }
    }

    [SerializeField] float cams_CahseSize;
    public void F_Set_CamsDis(bool value)
    {
        switch (curCamNum)
        {
            case 0:

                if (value)
                {

                    cams_CahseSize = Cams[0].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
                    Cams[0].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += 2f;
                }
                else
                {
                    Cams[0].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = cams_CahseSize;
                }

                break;
        }
    }


    [SerializeField] float addX;
    [SerializeField] float addZ;
    private void TownCamPosition()
    {
        if (TownCam.gameObject.activeSelf)
        {
            Vector3 PlayerVec = navsc.F_Out_Player_Pos();
            Vector3 enterVec = new Vector3(PlayerVec.x + addX, TownCam.transform.position.y, PlayerVec.z + addZ);
            TownCam.transform.position = enterVec;

        }
    }
    public bool F_isPlayerDowntown()
    {
        return inDownTown;
    }

    public void F_SetDowntownMode(bool value)
    {
        inDownTown = value;
    }

    public void F_RotChager(Vector3 Pos, int value)
    {
        Player.transform.eulerAngles = new Vector3(Player.transform.eulerAngles.x, 158, Player.transform.eulerAngles.z);

        switch (value)
        {
            case 0:
                Cams[0].Follow = ExitTarget.transform;
                Cams[0].LookAt = ExitTarget.transform;


                Vector3 Rot = new Vector3(-4, 158, 0);
                Cams[0].transform.position = Pos;
                Cams[0].GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = -1.43f;
                Cams[0].GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = 150.3f;
                Camera.main.transform.eulerAngles = Rot;
                Camera.main.transform.position = Pos;


                break;

            case 1:
                Cams[0].Follow = Player.transform;
                Cams[0].LookAt = Player.transform;
                break;
        }
    }
    WaitForSeconds shootInverval = new WaitForSeconds(0.02f);
    WaitForSeconds meleeInverval = new WaitForSeconds(0.01f);
    bool isCorotineEnd;
    /// <summary>
    /// 카메라 줌인아웃 액션
    /// </summary>
    /// <param name="value">0=3인칭/1=에임/3=타겟팅</param>
    public void F_FireCameraZoonOutIn()
    {
        if(isCorotineEnd == false)
        {
            isCorotineEnd = true;
            StartCoroutine(Shot());
        }
        
    }
    float zoomDis;
    float originCamDis;
    int selectModeNum;
    IEnumerator Shot()
    {
        selectModeNum  = 0; 

        switch (curCamNum) // 3인칭 카메라가 3번이여서 이 과정을 추가함.
        {
            case 0:
                selectModeNum = 0;
                break;

            case 1:
                selectModeNum = 1;
                break;

            case 3:
                selectModeNum = 2;
                break;
        }

        originCamDis = Cams[selectModeNum].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;

        switch (selectModeNum)
        {
            case 0:
                zoomDis = originCamDis + 1.5f;
                break;

            case 1:
                zoomDis = originCamDis + 0.8f;
                break;

            case 2:
                zoomDis = originCamDis + 0.5f;
                break;
        }


        Cams[selectModeNum].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = zoomDis;

        while (zoomDis > originCamDis)
        {
            switch (selectModeNum)
            {
                case 0:
                    zoomDis -= 0.1f;
                    break;

                case 1:
                    zoomDis -= 0.02f;
                    break;

                case 2:
                    zoomDis -= 0.02f;
                    break;
            }
           
            Cams[selectModeNum].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = zoomDis;
       
            switch (selectModeNum)
            {
                case 0:
                    yield return meleeInverval;
                    break;

                case 1:
                    yield return shootInverval;
                    break;

                case 2:
                    yield return shootInverval;
                    break;
            }
      
        }

        Cams[selectModeNum].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = originCamDis;

        zoomDis = 0;
        originCamDis = 0;
        isCorotineEnd = false;
    }

    public float F_1rd_Cam_VerticalValue()
    {
        return Cams[1].GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value;
    }
}
