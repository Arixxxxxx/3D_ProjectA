using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerMoveController : MonoBehaviour
{
    public static PlayerMoveController Inst;
    GameManager Gm;
    #region [초기화 변수들]
    [Header("# Player Move Value Setting")]
    [Space]
    [SerializeField] private float fowardMoveSpeed;
    [SerializeField] private float backMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float DodgeSpeed;
    [SerializeField] private float SwimSpeed;
    [SerializeField] GameObject lockOnEnemy;
    [SerializeField] ParticleSystem[] modeCheangePs;
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

    [SerializeField] bool SpaceBarKeyDown;
    [SerializeField] bool XkeyDown;
    bool doRun;
    bool isRun;
    bool isSlope;
    bool isInWater;
    bool isNoUpInWater;
    public bool IsNoUpInWater { get { return isNoUpInWater; } set { isNoUpInWater = value; } }
    public bool IsinWater { get { return isInWater; } set { isInWater = value; } }

    [SerializeField] bool isGround;
    public bool IsGround { get { return isGround; } }
    [SerializeField] bool isNormalMode;
    [SerializeField] bool isMeleeMode;
    [SerializeField] bool isMeleeTargetingMode;
    [SerializeField] bool isRangeMode;
    [SerializeField] bool isAimMode;
    [SerializeField] bool camSpinMode;
    [SerializeField] bool camFllowMode = true;
    bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
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

        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
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
        if(GM.IsWindowOpen == true) { return; }
       
        InputFuntion();

        if (GM.F_GetMouseScrrenRotationStop() == false)
        {
            PlayerRotation();
        }

        ModeChanger();
        CharRunSpeedAdd(doRun);
        


        if (isInWater == false)
        {
            CheakGround();
            Char_SlopeMove();
            Character_Ground_Action();
            DontSlopeMountine();
        }
        else if (isInWater == true)
        {
            Character_Swim_Action();
        }

        isDownTown = CameraManager.inst.F_isPlayerDowntown();
        lockOnEnemy = playerBattleController.F_Get_SelectTarget();


    }
    #endregion



    private void CheakGround()
    {
        //isGround = Physics.SphereCast(transform.position, cheakGroundRadios,
        isGround = Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, cheakGroundDis, GroundLayer);

        if (!isGround && isInWater == false)
        {
            verticalVelo += grivity_Value * Time.deltaTime;
        }
        else
        {
            verticalVelo = 0;
        }
        if (SpaceBarKeyDown && isGround && IsAttacking == false && isInWater == false)
        {
            verticalVelo = jumpForce;

        }
        charMoveVec.y = verticalVelo;
    } // 바닥체크

    Vector3 aroundVec;
    private void Character_Ground_Action()
    {
        if (isInWater == true) { return; }

        if (charMoveVec.z < 0)
        {
            charMoveVec *= 0.5f; // 뒤로이동시 이동속도 50% 감소
        }

    

        if (isMeleeTargetingMode == true && lockOnEnemy != null)
        {
            transform.LookAt(lockOnEnemy.transform.position);
        }

        if (isMeleeTargetingMode == true && lockOnEnemy != null && lockOnEnemy.gameObject.activeSelf == false)
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
    } // 바닥지역


    float swimVerticalY;
    private void Character_Swim_Action()
    {
        if(isInWater == true)
        {
            if (charMoveVec.z < 0)
            {
                charMoveVec *= 0.5f; // 뒤로이동시 이동속도 50% 감소
            }

            if(isNoUpInWater == true)
            {
                swimVerticalY = Input.GetAxis("SwimSpace");
            }
            else
            {
                swimVerticalY = 0;
            }
            swimVerticalY -= Input.GetAxis("SwimX");
            //if (XkeyDown)
            //{
            //    swimVerticalY -= SwimSpeed * Time.deltaTime;
            //}

            //if (SpaceBarKeyDown)
            //{
            //    swimVerticalY += SwimSpeed * Time.deltaTime;
            //}
            //else
            //{
            //    swimVerticalY = 0;
            //}

            charMoveVec.y = swimVerticalY;

            cCon.Move(transform.TransformDirection(charMoveVec) * SwimSpeed * Time.deltaTime);
            transform.eulerAngles += charRotVec * rotationSpeed * Time.deltaTime;
     
        }
    }
    private void Char_SlopeMove() // 슬로프
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

        


        doRun = Input.GetKey(KeyCode.LeftShift);


        //if (Input.GetKeyUp(KeyCode.LeftShift))  {  isRun = false; }


        SpaceBarKeyDown = Input.GetKeyDown(KeyCode.Space);
        
        if (isInWater)
        {
            //SpaceBarKeyDown = Input.GetKey(KeyCode.Space);
            //XkeyDown = Input.GetKeyDown(KeyCode.X);
            //XkeyDown = Input.GetKey(KeyCode.X);
        }
        

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

        if (_1keyDown && GameManager.Inst.NoChangeMode == false)
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

                playerBattleController.F_Set_CurModeWeapon(1);
            }



        }

        if (_2keyDown && GameManager.Inst.NoChangeMode == false)
        {
            if (isRangeMode)
            {
                F_ModeSelect("normal");
                anim.F_PlayerCurMode(0);
                playerBattleController.F_Set_CurModeWeapon(0);
            }
            else

            {
                F_ModeSelect("range");
                anim.F_PlayerCurMode(2);
                playerBattleController.F_Set_CurModeWeapon(2);
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
                isAimMode = false;
                CameraManager.inst.F_ChangeCam(0);

                break;

            case "melee":
                isNormalMode = false;
                isMeleeMode = true;
                isRangeMode = false;
                isMeleeTargetingMode = false;
                isAimMode = false;
                CameraManager.inst.F_ChangeCam(0);
                modeCheangePs[0].Play();
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
                isAimMode = false;
                isMeleeTargetingMode = false;
                CameraManager.inst.F_ChangeCam(0);
                modeCheangePs[1].Play();
                break;


            case "aim":
                isNormalMode = false;
                isMeleeMode = false;
                isRangeMode = false;
                isAimMode = true;
                isMeleeTargetingMode = false;
                CameraManager.inst.F_ChangeCam(0);
                break;
        }

    }
    /// <summary>
    /// 플레이어 현재 모드 / 외부송출 함수
    /// </summary>
    /// <returns> 0 = 노말 / 1 = 밀리 / 2= 레인지 / 3타겟팅 // 4에임</returns>
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
        else if (isAimMode)
        {
            return 4;
        }

        return -1;
    } // 현재 모드 외부송출

    public void F_AimModeOff_NoParticle_Funtion()
    {
        isNormalMode = false;
        isMeleeMode = false;
        isRangeMode = true;
        isAimMode = false;
        isMeleeTargetingMode = false;
        CameraManager.inst.F_ChangeCam(0);
    }

    public Vector3 F_Get_PlayerCurPos()
    {
        return transform.position;
    }

    public bool F_IsInWater()
    {
        return IsinWater;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }

}
