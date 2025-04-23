using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Animations.Rigging;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region 메인 설정
    [Header("Player")]
    public Transform playerTF;
    public Transform playerModel;
    public Transform playerBody;
    public GameManager.CameraMode cameraMode;
    public Camera playerCamera;

    public Rig RigObject;
    Transform playerCam;
    [Space(20)] 
    #endregion
    #region 이동관련 변수
    [Header("이동")]

    public CharacterController controller;

    public float Direction;
    public float WalkSpeed;
    public float RunSpeed;

    public GameManager.armState armState;
    public float camRotation_y;

    public bool isRun;
    float moveSpeed;
    Vector3 normalizedInput;
    [Space(20)]
    #endregion
    #region 점프관련 변수
    [Header("Jump")]
    private Vector3 moveInput;
    private bool isJumping;

    public float gravity = -20f; 
    private Vector3 velocity;
    private bool isGrounded;
    public float jumpDistance;
    Vector3 movement;
    private Vector3 startPos;
    public float arcHeight;
    private float totalTime;
    private float currentTime;
    Vector3 currentVelocity;
    [Space(20)]
    #endregion
    #region 인풋 액션

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction inputAction;
    InputAction runAction;
    InputAction jumpAction;
    InputAction fireAction;
    [Space(20)]
    #endregion
    #region 애니메이션 관련 변수
    [Header("Animation")]
    public Animator animator;

    public string RifleDrawAnim;
    public string RiflePutAnim;
    public string PistolDrawAnim;
    public string PistolPutAnim;

    public float ikWeight = 0f; // IK 적용 강도 (0 ~ 1)
    public GameObject handPositionSphere;
    public float blendSpeed;

    
    [Range(0.1f, 0.5f)]
    public float IKHandsWidth = 0.2f;

    [Space(20)]
    #endregion
    #region 소켓
    [Header("PlayerSocket")]
    public GameObject PistolSocket;
    public GameObject WeaponSocket;
    public GameObject RifleSocket;
    [Space(10)]
    #endregion
    #region 상태 변수
    [Header("무기")]
    public WeaponScript currentWeapon;
    public bool IsAimDown;
    public bool isFireDown;

    [Space(10)]
    public bool isReloading;
    public bool isEquipping;
    public bool isUnequipping;
    [Space(10)]
    #endregion

    [Header("Vault")]
    public bool isVaulting;

    public Transform VaultRootBone;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;

    float vaultTimer = 0;
    float duration;
    [Space(10)]

    float xRot;
    float yRot;
    public bool mantleCheck;
    GameObject mantleObject;
    Vector3 handPos;
    [HideInInspector] public bool InputOn = true;
    
    public bool isMove;

    void Start()
    {
        
        playerCam = playerCamera.transform; // 플레이어 카메라 세팅
        GameManager.CursorOff(); // 커서 끄기

        //캐릭터 컨트롤러 세팅
        controller = GetComponent<CharacterController>();
        controller.center = new Vector3(0, 0.9f, 0);
        
       

        #region InputActionSettings
        //인풋액션 세팅
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        runAction = playerInput.actions.FindAction("Run");
        jumpAction = playerInput.actions.FindAction("Jump");
        fireAction = playerInput.actions.FindAction("Fire");
        #endregion
        #region 소켓 세팅
        PistolSocket = GameObject.FindGameObjectWithTag("PistolSocket");
        WeaponSocket = GameObject.FindGameObjectWithTag("WeaponSocket");
        RifleSocket = GameObject.FindGameObjectWithTag("RifleSocket"); 
        #endregion
    }


    private void FixedUpdate()
    {
        if(isVaulting) return;
        Move(); // 이동 처리 함수

        #region 중력

        if(!isVaulting)
        {
            currentTime += Time.deltaTime;

            currentVelocity = Vector3.Slerp(currentVelocity, velocity, Time.deltaTime * 60f);
            currentVelocity.y += -9.81f * currentTime; // 중력 적용

            if (controller.enabled)
                controller.Move(currentVelocity * Time.deltaTime);

            if (isJumping && controller.isGrounded)
            {
                isJumping = false;
                velocity = Vector3.zero;

            }
        }
        



        #endregion
        #region 이동속도
        if (isMove)
        {
            if (isRun)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, RunSpeed, Time.deltaTime * 5f);
                IsAimDown = false;
                CancelInvoke();
            }

            else if (!isRun) moveSpeed = Mathf.Lerp(moveSpeed, WalkSpeed, Time.deltaTime * 5f);
            else moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 5f);
        }


        #endregion
    }



    void Update()
    {
        isGrounded = GroundCheck(); // 땅 체크
        mantleCheck = MantleCheck();

        animator.SetBool("IsGrounded", isGrounded);

        #region Variation of Animator

        animator.SetBool("Is Move", (moveInput.z != 0 || moveInput.x != 0) ? true : false);
        

        if (isMove)
        {
            animator.SetBool("IsRun", isRun);

            animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), moveInput.x, Time.deltaTime * 5f));
            animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), moveInput.z, Time.deltaTime * 5f));
        }

        else
        {
            animator.SetBool("IsRun", false);

            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);

        }
        

        animator.SetFloat("ArmState", (int)armState);
        animator.SetInteger("int_armState", (int)armState);
        
        if (IsAimDown && !isRun && !isEquipping && !isReloading && !isUnequipping)
        {
            animator.SetBool("IsAim", true);

        }

        else
        {
            animator.SetBool("IsAim", false);
        }
        #endregion 

        isMove = moveInput != Vector3.zero ? true : false;
        
        #region 플레이어 상태 변수
        if (isEquipping || isUnequipping || isReloading)
        {
            if (animator && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.95f)
            {
                isEquipping = false;
                isUnequipping = false;
                isReloading = false;
            }
        }
        #endregion

        if(isVaulting)
        {
            
            RigObject.weight = 0;
            transform.root.position = controller.transform.position;


            if (animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.93f)
            {
                controller.enabled = true;

                Debug.Log("rootPosition: " + animator.deltaPosition);
                isVaulting = false;
                animator.applyRootMotion = false;
                vaultTimer = 0;

                if(!isGrounded)
                {
                    controller.Move(transform.forward * 0.3f);
                }
                
            }
        }

        else
        {
            RigObject.weight = Mathf.Lerp(RigObject.weight, 1, Time.deltaTime * 3f);
        }
    }

    #region 무기 이벤트
    public void OnSwitchFireMode()
    {
        if (armState == GameManager.armState.Unarmed) return;

        WeaponScript ws = currentWeapon.GetComponent<WeaponScript>();

        switch (ws.fireMode)
        {
            case WeaponScript.FireMode.Single:

                if (armState == GameManager.armState.Pistol) return;

                ws.fireMode = WeaponScript.FireMode.Auto;

                Debug.Log(ws.fireMode);
                break;

            case WeaponScript.FireMode.Auto:
                ws.fireMode = WeaponScript.FireMode.Single;

                Debug.Log(ws.fireMode);
                break;
        }
    }
    public void OnFire(InputValue value)
    {
        isFireDown = value.Get<float>() == 1 ? true : false;

        if (isFireDown && IsAimDown)
        {
            if (armState == GameManager.armState.Pistol)
            {
                WeaponSocket.transform.GetChild(0).GetComponent<WeaponScript>().FireStart();
                animator.SetTrigger("Fire");
            }

            else if (armState == GameManager.armState.Rifle)
            {
                WeaponSocket.transform.GetChild(0).GetComponent<WeaponScript>().FireStart();
                animator.SetTrigger("Fire");
            }


        }

        else
        {
            if (currentWeapon) currentWeapon.GetComponent<WeaponScript>().CancelInvoke();

        }

    }
    public void OnReload()
    {
        if (armState != GameManager.armState.Unarmed)
        {
            isReloading = true;
            animator.SetTrigger("Reload");
            IsAimDown = false;
        }
    }
    public void OnAim(InputValue value)
    {
        IsAimDown = value.Get<float>() == 1 && !isRun && !isReloading && !isEquipping && !isUnequipping ? true : false;
    }
    public InputAction GetFireAction()
    {
        return fireAction;
    } 
    #endregion

    public void ReloadComplete()
    {
        Debug.Log(0);
        if (currentWeapon != null)
        {
            Debug.Log(1);
            currentWeapon.GetComponent<WeaponScript>().ReloadComplete();
        }
    }
    public void RootMotionOff()
    {
        animator.applyRootMotion = false;
    }
    


    #region 이동
    public void Move()
    {
        if (isVaulting) return;


        transform.parent.position = transform.position;

        moveInput = isVaulting ? Vector3.zero : moveAction.ReadValue<Vector3>().normalized;

        movement = controller.transform.forward * moveInput.z + controller.transform.right * moveInput.x;
        //movement.Normalize();



        if (!isVaulting && isGrounded)
        {
            controller.Move(movement * Time.deltaTime * moveSpeed);

        }



    }

    public void OnJump()
    {
        

        if (mantleCheck)
        {
            if(!isVaulting)
            {


                if (animator) 
                {
                    animator.SetTrigger("Vault");

                    animator.applyRootMotion = true;
                    isVaulting = true;
                    Vector3 objPos = mantleObject.transform.position;
                    objPos.y = transform.position.y;
                }

            }
        }

        else
        {
            isGrounded = GroundCheck();
            if (isGrounded)
            {
                if (moveInput == Vector3.zero)
                {
                    arcHeight = 1.3f;
                    jumpDistance = 0;
                    animator.SetTrigger("Jump");
                }
                else
                {

                    jumpDistance = isRun ? 3 : 2;
                    arcHeight = isRun ? 1.2f : 1;
                    animator.SetTrigger("Jump");
                }

                StartJump(transform.position + movement * jumpDistance);
            }
        }
    }

    public void OnRun(InputValue inputValue)
    {
        isRun = inputValue.Get<float>() == 1 ? true : false;
    }
    public bool GroundCheck()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up * 0.2f, 0.2f, Vector3.down, out hit, 0.3f))
        {
            return true;
        }

        return false;
    }

    void StartJump(Vector3 targetPos)
    {
        startPos = transform.position;
        velocity = CalculateParabolaVelocity(targetPos, arcHeight, gravity, out totalTime);
        currentTime = 0f;
        isJumping = true;
    }

    Vector3 CalculateParabolaVelocity(Vector3 targetPos, float arcHeight, float gravity, out float time)
    {
        Vector3 displacement = targetPos - transform.position;
        Vector3 displacementXZ = new Vector3(displacement.x, 0f, displacement.z);

        float timeToApex = Mathf.Sqrt(-2 * arcHeight / gravity);
        float timeFromApex = Mathf.Sqrt(2 * (displacement.y - arcHeight) / gravity);
        time = timeToApex + timeFromApex;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * arcHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY;
    }

    public bool MantleCheck()
    {

        RaycastHit hit;
        bool isHit = Physics.CapsuleCast(transform.position + transform.forward * -0.25f,
            transform.position + new Vector3(0, controller.height, 0), 0.25f, transform.forward, out hit, 0.75f);
        Debug.DrawRay(point1.transform.position + transform.forward * -0.25f, point1.transform.forward * 0.75f, isHit ? Color.red : Color.white, Time.deltaTime, false);
        if (isHit)
        {
            mantleObject = hit.transform.gameObject;

            Vector3 center = mantleObject.GetComponent<Collider>().bounds.center;
            // 박스의 크기를 로컬에서 월드 크기로 변환
            Vector3 size = mantleObject.GetComponent<Collider>().bounds.size;
            Vector3 result = new Vector3(hit.point.x, center.y + size.y / 2, hit.point.z);

            return true;
        }

        mantleObject = null;
        return false;



    }

    #endregion

    public void OnSwitchCameraMode(InputValue value)
    {
        switch(cameraMode)
        {
            case GameManager.CameraMode.FPS:
                //rigObject.GetComponent<Rig>().weight = 1;
                cameraMode = GameManager.CameraMode.TPS;

                playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("PlayerHead"));
                break;


            case GameManager.CameraMode.TPS:
                //rigObject.GetComponent<Rig>().weight = 0;
                cameraMode = GameManager.CameraMode.FPS;

                playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerHead"));
                break;
        }

        Debug.Log(cameraMode);
    }

    

    public InputAction GetMoveAction()
    {
        return moveAction;
    }
}
