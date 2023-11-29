using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DownTownPlayerScan : MonoBehaviour
{
    public enum EnterType
    {
        TownIn, TownOut
    }
    public EnterType type;

    [SerializeField] GameObject[] PlayerPrefabs;
    [SerializeField] Transform reSetTr;
    [SerializeField] Transform camPos;
    [SerializeField] PlayerMoveController player;
    [SerializeField] AnimationContoller anim;
    [Header("# 질문창")]
    [SerializeField] GameObject questionBox;
    [SerializeField] TMP_Text questionText;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private NavMeshSurface nms;
    private CameraManager camM;



    private void Awake()
    {
        nms = GetComponent<NavMeshSurface>();

    }

    private void Start()
    {
        camM = CameraManager.inst;

    }

    private void OnTriggerEnter(Collider other)
    {

        switch (type)
        {
            case EnterType.TownIn:
                if (other.CompareTag("Player"))
                {
                    GameManager.Inst.F_SetScreenCursorOnOFF(true);
                    if (questionBox.gameObject.activeSelf == false)
                    {
                        questionBox.gameObject.SetActive(true);
                    }
                    questionText.text = $"마을에 들어가시겠습니까?";
                    yesButton.onClick.AddListener(() => { StartCoroutine(EnterButton()); });
                    
                }
                break;

            case EnterType.TownOut:
                if (other.CompareTag("Player"))
                {
                    if (questionBox.gameObject.activeSelf == false)
                    {
                        questionBox.gameObject.SetActive(true);
                    }
                    questionText.text = $"마을에서 나가시겠습니까?";
                    yesButton.onClick.AddListener(() => { StartCoroutine(EnterButton()); });
                }
                break;

        }
        //switch (type)
        //{
        //    case EnterType.TownIn:
        //        if (other.CompareTag("Player"))
        //        {
        //            GameManager.Inst.F_SetScreenCursorOnOFF(true);
        //            anim.Isdodge = false;
        //            player.F_ModeSelect("normal");
        //            camM.F_ChangeCam(2); // 카메라 전환
        //            camM.F_RotChager(camPos.position, 0); // 3인칭 카메라 이동
        //            PlayerObjTransformSet(0); // 외부 플레이어 프리펩 위치 변환
        //            GameManager.Inst.InHomeTown = true;
        //            PlayerPrefabsChanger(1);


        //        }
        //        break;

        //    case EnterType.TownOut:
        //        if (other.CompareTag("Player"))
        //        {
        //            PlayerObjTransformSet(1);
        //            GameManager.Inst.InHomeTown = false;
        //            PlayerPrefabsChanger(0);
        //            camM.F_ChangeCam(0);
        //            camM.F_RotChager(camPos.position, 1);
        //            GameManager.Inst.F_SetScreenCursorOnOFF(false);
        //        }
        //        break;

        //}

    }
    WaitForSeconds waitCutten = new WaitForSeconds(1);
    IEnumerator EnterButton()
    {
        switch (type)
        {
            case EnterType.TownIn:
                GameUIManager.Inst.F_BlackScrrenOn();
                yield return waitCutten;
                GameManager.Inst.F_SetScreenCursorOnOFF(true);
                anim.Isdodge = false;
                player.F_ModeSelect("normal");
                camM.F_ChangeCam(2); // 카메라 전환
                camM.F_RotChager(camPos.position, 0); // 3인칭 카메라 이동
                PlayerObjTransformSet(0); // 외부 플레이어 프리펩 위치 변환
                GameManager.Inst.InHomeTown = true;
                PlayerPrefabsChanger(1);
                questionBox.gameObject.SetActive(false);

                break;

            case EnterType.TownOut:
                GameUIManager.Inst.F_BlackScrrenOn();
                yield return waitCutten;
                PlayerObjTransformSet(1);
                GameManager.Inst.InHomeTown = false;
                PlayerPrefabsChanger(0);
                camM.F_ChangeCam(0);
                camM.F_RotChager(camPos.position, 1);
                GameManager.Inst.F_SetScreenCursorOnOFF(false);

                break;

        }
    }
    /// <summary>
    /// 프리펩교체 , 외부는 캐릭터컨트롤러, 마을은 네브메쉬에이젼트 사용 [0,1]로 제어
    /// </summary>
    /// <param name="value">0번 필드용//1번 마을용</param>
    private void PlayerPrefabsChanger(int value)
    {
        for (int i = 0; i < PlayerPrefabs.Length; i++)
        {
            if (i == value)
            {
                PlayerPrefabs[i].gameObject.SetActive(true);
            }
            else
            {
                PlayerPrefabs[i].gameObject.SetActive(false);
            }

        }
    }

    private void PlayerObjTransformSet(int value)
    {

        switch (value)
        {
            case 0:
                PlayerPrefabs[0].transform.position = reSetTr.position;

                break;

            case 1:
                PlayerPrefabs[1].transform.position = reSetTr.position;
                PlayerPrefabs[1].transform.eulerAngles = reSetTr.eulerAngles;
                break;
        }

    }
}
