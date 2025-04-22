using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public PlayerController playerController; //플레이어 컨트롤러
    public Transform headBone;


    public Transform CameraFollowTarget; // 카메라가 따라갈 타겟 오브젝트
    public Transform lookTarget;

    public Transform T_fps;
    public Transform T_tps;


    public Transform aimPoint;
    public GameManager.CameraMode cameraMode;

    [Space(10)]
    [Header("TPS")]
    public Transform Tps_Aim;

    PlayerInput playerInput;
    InputAction lookAction;

    [Range(1, 5)]
    public float stabilizeSpeed;

    float yRot = 0, xRot = 0;

    [HideInInspector] public bool InputOn = true;
    Vector2 lookInput;


    Vector3 currentVelocity;
    private void Awake()
    {
        //playerController = GetComponentInParent<PlayerController>();
        playerInput = playerController.transform.GetComponent<PlayerInput>();
        lookAction = playerInput.actions.FindAction("Look");

        //CameraFollowTarget = T_fps;
        headBone = GameManager.FindChildRecursive(playerController.playerBody, "head Cam").parent;
    }
    // Update is called once per frame
    void Update()
    {
        if (!InputOn) return;

        switch (playerController.cameraMode)
        {

            case GameManager.CameraMode.FPS:
                if (CameraFollowTarget != T_fps)
                {
                    CameraFollowTarget = T_fps;
                }

                if (playerController.IsAimDown && playerController.currentWeapon != null && playerController.isReloading == false && !playerController.isRun)
                {
                    transform.position = Vector3.Lerp(transform.position, playerController.currentWeapon.GetComponent<WeaponScript>().AimSocket.position, Time.deltaTime * 13f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, playerController.currentWeapon.GetComponent<WeaponScript>().AimSocket.rotation, Time.deltaTime * 13f);
                }



                else //if (true)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, CameraFollowTarget.position, ref currentVelocity ,0.2f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, CameraFollowTarget.rotation, Time.deltaTime * 13f);
                }

                break;

            case GameManager.CameraMode.TPS:


                if(playerController.IsAimDown)
                {
                    CameraFollowTarget = Tps_Aim;
                }

                else
                {
                    CameraFollowTarget = T_tps;
                }

                transform.position = Vector3.SmoothDamp(transform.position, CameraFollowTarget.position, ref currentVelocity, 0.2f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position, Vector3.up), Time.deltaTime * 13f);
                break;

            
        }

        
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        xRot += lookInput.x * Time.deltaTime * 5f;
        yRot -= lookInput.y * Time.deltaTime * 5f;

        yRot = Mathf.Clamp(yRot, -30f, 70f);
    }
}
