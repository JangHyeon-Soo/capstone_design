using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    [Header("��ũ��Ʈ")]
    public PlayerController playerController; //�÷��̾� ��Ʈ�ѷ�

    public PlayerInteractionManager pim;

    public Transform CameraFollowTarget; // ī�޶� ���� Ÿ�� ������Ʈ
    public Transform lookTarget;

    public Transform T_fps;
    public Transform T_tps;

    public Transform aimPoint;


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

    // ī�޶� ���� ���� �߰�
    private Vector3 shakeOriginPosition;
    private Quaternion shakeOriginRotation;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float shakeFrequency = 1f;

    // ���� �ε巯���� ���� ��
    private float shakeDecay = 0.1f; // ���� ������
    private Vector3 shakeVelocity;

    private void Awake()
    {
        playerInput = playerController.transform.GetComponent<PlayerInput>();
        lookAction = playerInput.actions.FindAction("Look");

        //headBone = GameManager.FindChildRecursive(playerController.playerBody, "head Cam").parent;
        pim =transform.root.GetComponentInChildren<PlayerInteractionManager>();
    }

    void LateUpdate()
    {
        if (!InputOn) return;

        switch (playerController.cameraMode)
        {
            case GameManager.CameraMode.FPS:
                if (CameraFollowTarget != T_fps)
                {
                    CameraFollowTarget = T_fps;
                }

                if (playerController.IsAimDown && playerController.currentWeapon != null && !playerController.isReloading && !playerController.isRun)
                {
                    transform.position = Vector3.Lerp(transform.position, playerController.currentWeapon.GetComponent<WeaponScript>().AimSocket.position, Time.deltaTime * 13f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, playerController.currentWeapon.GetComponent<WeaponScript>().AimSocket.rotation, Time.deltaTime * 13f);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, CameraFollowTarget.position, Time.deltaTime * 13f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        playerController.isVaulting || playerController.isEquipping || playerController.isUnequipping 
                        || playerController.isReloading || !playerController.isMove? CameraFollowTarget.rotation :Quaternion.LookRotation((aimPoint.position - transform.position).normalized)
                        , Time.deltaTime * 13f);
                }

                break;

            case GameManager.CameraMode.TPS:
                if (playerController.IsAimDown)
                {
                    CameraFollowTarget = Tps_Aim;
                }
                else
                {
                    CameraFollowTarget = T_tps;
                }

                transform.position = Vector3.Lerp(transform.position, CameraFollowTarget.position, Time.deltaTime * 13f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position, Vector3.up), Time.deltaTime * 13f);
                break;

            case GameManager.CameraMode.PadLock:

                if(pim.interactionObject != null)
                {


                }
                transform.position = Vector3.Lerp(transform.position, pim.interactionObject.GetComponent<PadLockController>().camPosition.position, Time.deltaTime * 5f);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation((pim.interactionObject.transform.position - transform.position).normalized), Time.deltaTime * 5f);
                break;
        }

        //// ī�޶� ���� ���� (�ε巴��)
        //if (shakeDuration > 0)
        //{
        //    Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
        //    transform.position = Vector3.SmoothDamp(transform.position, shakeOriginPosition + shakeOffset, ref shakeVelocity, shakeDecay);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, shakeOriginRotation * Quaternion.Euler(shakeOffset * shakeMagnitude), Time.deltaTime * 5f);

        //    shakeDuration -= Time.deltaTime * shakeFrequency;
        //}
    }

    // ī�޶� ���� ����
    public void StartCameraShake(float duration, float magnitude, float frequency)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeFrequency = frequency;

        shakeOriginPosition = transform.position;
        shakeOriginRotation = transform.rotation;
    }

    // ī�޶� ���� ���߱�
    public void StopCameraShake()
    {
        shakeDuration = 0f;
        transform.position = shakeOriginPosition;
        transform.rotation = shakeOriginRotation;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        xRot += lookInput.x * Time.deltaTime * 5f;
        yRot -= lookInput.y * Time.deltaTime * 5f;

        yRot = Mathf.Clamp(yRot, -30f, 70f);
    }
}
