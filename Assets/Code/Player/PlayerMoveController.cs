using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerMoveController : MonoBehaviour
{
    #region [초기화 변수들]
    [Header("# Player Move Value Setting")]
    [Space]
    [SerializeField] private float fowardMoveSpeed;
    [SerializeField] private float backMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float DodgeSpeed;
    [SerializeField] GameObject lockOnEnemy;

    private float verticalVelo;
    private float grivity_Value = -9.81f;
    private float originSpeed = 0f;

    private CharacterController cCon;
    private AnimationContoller anim;
    private PlayerBattleController playerBattleController;
    private Vector3 charMoveVec;
    private Vector3 charRotVec;
    private Vector3 animVec;
    private Vector3 slopeVec;
    private Camera mainCam;

    bool doJump;
    bool doRun;
    bool isRun;
    bool isSlope;
    [SerializeField] bool isGround;
    public bool IsGround { get { return isGround; } }
    [SerializeField] bool isNormalMode;
    [SerializeField] bool isMeleeMode;
    [SerializeField] bool isMeleeTargetingMode;
    [SerializeField] bool isRangeMode;
    [SerializeField] bool camSpinMode;
    [SerializeField] bool camFllowMode = true;

      bool _1keyDown, _2keyDown;
    bool isDownTown;

    [SerializeField] bool mouseRightClick;
    GameManager GM;

    [SerializeField] float cheakGroundRadios;
    [SerializeField] float cheakGroundDis;
    [SerializeField] LayerMask GroundLayer;
    Vector3 DodgeVec;
    bool DodgeVecOnce;

    #endregion



    #region[Awake,Start,Update]
    private void Awake()
    {
        anim = GetComponent<AnimationContoller>();
        playerBattleController = GetComponent<PlayerBattleController>();
    }
    void Start()
    {
        cCon = GetComponent<CharacterController>();
        isNormalMode = true;
        GM = GameManager.Inst;
        mainCam = Camera.main;
    }

    void Update()
    {
        CheakGround();
        InputFuntion();

        if (GM.F_GetMouseScrrenRotationStop() == false)
        {
            PlayerRotation();
        }

        ModeChanger();
        CharRunSpeedAdd(doRun);
        DontSlopeMountine();



        Char_SlopeMove();

        Char_Action();

        isDownTown = CameraManager.inst.F_isPlayerDowntown();
        lockOnEnemy = playerBattleController.F_Get_SelectTarget();


    }
    #endregion



    private void CheakGround()
    {
        //isGround = Physics.SphereCast(transform.position, cheakGroundRadios,
        isGround = Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, cheakGroundDis, GroundLayer);
        if (!isGround)
        {
            verticalVelo += grivity_Value * Time.deltaTime;
        }
        else
        {
            verticalVelo = 0;
        }
        if (doJump && isGround)
        {
            verticalVelo = jumpForce;
        }
    } // 바닥체크

    Vector3 aroundVec;
    private void Char_Action()
    {
        if (charMoveVec.z < 0)
        {
            charMoveVec *= 0.5f; // 뒤로이동시 이동속도 50% 감소
        }

        if(isMeleeTargetingMode == true && lockOnEnemy != null)
        {
            transform.LookAt(lockOnEnemy.transform.position);
        }

        if(isMeleeTargetingMode == true && lockOnEnemy != null && lockOnEnemy.gameObject.activeSelf == false)
        {
            lockOnEnemy = null;
            F_ModeSelect("melee");
        }

        if (!anim.Isdodge)
        {
            if (DodgeVecOnce) { DodgeVecOnce = false; }

            cCon.Move(transform.TransformDirection(charMoveVec) * fowardMoveSpeed * Time.deltaTime);
            transform.eulerAngles += charRotVec * rotationSpeed * Time.deltaTime;
        }
        else if (anim.Isdodge)
        {
            if (!DodgeVecOnce)
            {
                DodgeVecOnce = true;
                DodgeVec = charMoveVec;
            }
            DodgeVec = Vector3.Lerp(DodgeVec, Vector3.zero, Time.deltaTime);
            cCon.Move(transform.TransformDirection(DodgeVec) * DodgeSpeed * Time.deltaTime);
            transform.eulerAngles += charRotVec * rotationSpeed * Time.deltaTime;
        }
    } // 이동 함수
        private void Char_SlopeMove()
    {
        if (isSlope)
        {
            cCon.Move(slopeVec);
        }
    }  //슬로프 모드 이동
    private void InputFuntion()
    {
        _1keyDown = Input.GetKeyDown(KeyCode.Alpha1);
        _2keyDown = Input.GetKeyDown(KeyCode.Alpha2);
        mouseRightClick = Input.GetMouseButton(1);
        charMoveVec.z = Input.GetAxisRaw("Vertical");
        charMoveVec.x = Input.GetAxisRaw("Horizontal");

        //charRotVec.y = Camera.main.transform.eulerAngles.y;

        charMoveVec = charMoveVec.normalized;

        charMoveVec.y = verticalVelo;


        doRun = Input.GetKey(KeyCode.LeftShift);


        //if (Input.GetKeyUp(KeyCode.LeftShift))  {  isRun = false; }


        doJump = Input.GetKeyDown(KeyCode.Space);


    }//입력신호

    [SerializeField] float RotY;
    private void PlayerRotation()
    {
        if (isMeleeTargetingMode) 
        {
            return;
            //RotY = transform.eulerAngles.y;
            //mainCam.transform.eulerAngles = new Vector3(mainCam.transform.eulerAngles.x, RotY, mainCam.transform.eulerAngles.y);
        }
        else
        {
            RotY = Camera.main.transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, RotY, 0);
        }
        
    }//플레이어 회전

    private void CharRunSpeedAdd(bool value)
    {
        if (!isNormalMode) { return; }

        if (value)
        {
            if (!isRun)
            {
                isRun = true;
                originSpeed = fowardMoveSpeed;
                fowardMoveSpeed += RunSpeed;
                CameraManager.inst.F_Set_CamsDis(true);
            }

        }
        else
        {
            if (isRun)
            {
                isRun = false;
                fowardMoveSpeed = originSpeed;
                CameraManager.inst.F_Set_CamsDis(false);
            }

        }
    } //스프린트

    private void ModeChanger()
    {

        if (_1keyDown)
        {
            if (isMeleeMode)
            {
                F_ModeSelect("normal");
                anim.F_Set_LayerWeight(1, false);
                anim.F_PlayerCurMode(0);
                playerBattleController.F_Set_CurModeWeapon(0);
            }
            else

            {
                F_ModeSelect("melee");
                anim.F_PlayerCurMode(1);
                //anim.F_Set_LayerWeight(1, true);
                playerBattleController.F_Set_CurModeWeapon(1);
            }



        }

        if (_2keyDown)
        {
            if (isRangeMode)
            {
                F_ModeSelect("normal");
            }
            else

            {
                F_ModeSelect("range");
            }

        }
    }// 모드 변경

    private void DontSlopeMountine()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, cCon.height * 0.6f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle > cCon.slopeLimit)
            {
                isSlope = true;
                slopeVec = Vector3.ProjectOnPlane(new Vector3(0, grivity_Value, 0), hit.normal) * Time.deltaTime;
            }
            else
            {
                isSlope = false;
            }
        }


    } // 슬로프 계산



    /// <summary>
    /// 캐릭터 모드 변경
    /// </summary>
    /// <param name="Value">Input String = normal / melee / targeting / ragne </param>
    public void F_ModeSelect(string Value)
    {
        switch (Value)
        {
            case "normal":
                isNormalMode = true;
                isMeleeMode = false;
                isRangeMode = false;
                isMeleeTargetingMode = false;
                CameraManager.inst.F_ChangeCam(0);

                break;

            case "melee":
                isNormalMode = false;
                isMeleeMode = true;
                isRangeMode = false;
                isMeleeTargetingMode = false;
                CameraManager.inst.F_ChangeCam(0);
                break;

            case "targeting":
                isMeleeTargetingMode = true;
                isNormalMode = false;
                isMeleeMode = false;
                isRangeMode = false;
                CameraManager.inst.F_ChangeCam(3);
                break;

            case "range":
                isNormalMode = false;
                isMeleeMode = false;
                isRangeMode = true;
                isMeleeTargetingMode = false;
                CameraManager.inst.F_ChangeCam(1);
                break;
        }

    } 
    /// <summary>
    /// 플레이어 현재 모드 / 외부송출 함수
    /// </summary>
    /// <returns> 0 = 노말 / 1 = 밀리 / 2= 레인지 / 3타겟팅</returns>
    public int F_GetPlayerAttackModeNum()
    {
        if (isNormalMode)
        {
            return 0;
        }
        else if (isMeleeMode)
        {
            return 1;
        }
        else if (isRangeMode)
        {
            return 2;
        }
        else if (isMeleeTargetingMode)
        {
            return 3;
        }

        return -1;
    } // 현재 모드 외부송출




}
