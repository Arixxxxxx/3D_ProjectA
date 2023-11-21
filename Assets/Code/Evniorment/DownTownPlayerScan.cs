using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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
                    anim.Isdodge = false;
                    player.F_ModeSelect("normal");
                    camM.F_ChangeCam(2); // ī�޶� ��ȯ
                    camM.F_RotChager(camPos.position,0); // 3��Ī ī�޶� �̵�
                    PlayerObjTransformSet(0); // �ܺ� �÷��̾� ������ ��ġ ��ȯ
                    GameManager.Inst.InHomeTown = true;
                    PlayerPrefabsChanger(1);
                   

                }
                break;

                case EnterType.TownOut:
                if (other.CompareTag("Player"))
                {
                    PlayerObjTransformSet(1);
                    GameManager.Inst.InHomeTown = false;
                    PlayerPrefabsChanger(0);
                    camM.F_ChangeCam(0);
                    camM.F_RotChager(camPos.position,1);
                    GameManager.Inst.F_SetScreenCursorOnOFF(false);
                }
                break;

        }
    
    }


    /// <summary>
    /// �����鱳ü , �ܺδ� ĳ������Ʈ�ѷ�, ������ �׺�޽�������Ʈ ��� [0,1]�� ����
    /// </summary>
    /// <param name="value">0�� �ʵ��//1�� ������</param>
    private void PlayerPrefabsChanger(int value)
    {
        for(int i = 0; i < PlayerPrefabs.Length; i++)
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
