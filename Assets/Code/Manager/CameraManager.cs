using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager inst;
    [SerializeField] PlayerNavMeshSC navsc;
    [SerializeField] CinemachineVirtualCamera[] Cams;
    [SerializeField] Camera TownCam;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject ExitTarget;
    int curCamNum;
    PlayerMoveController player;
    [SerializeField] bool inDownTown;

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
    private void Update()
    {
        TownCamPosition();
        CameraRotStop();
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
            case 1: // 활쏠때 에임카메라
                Cams[0].gameObject.SetActive(false);
                Cams[1].gameObject.SetActive(true);
                Cams[2].gameObject.SetActive(false);
                TownCam.gameObject.SetActive(false);
                break;
            case 2: // 마을 쿼터뷰 카메라
                Cams[0].gameObject.SetActive(false);
                Cams[1].gameObject.SetActive(false);
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
}
