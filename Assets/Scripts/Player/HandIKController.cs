using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;
using UnityEngine.InputSystem;


public class HandIKController : MonoBehaviour
{


    public Animator animator;
    PlayerController pc;
    CharacterController controller;

    public bool IkOn;

    [Tooltip("캐릭터 바디의 루트본")]
    public Transform head;
    public Transform AimPointSphere;
    public Transform Hand_r;
    public Transform Hand_l;
    public Transform MagsThrow;

    //[Tooltip("무기가 들어갈 소켓의 이름")]
    Transform GunPosition;
    Transform currentGun;

    [Header("Pistol Hand Position")]
    public Transform RHT; //오른손 IK 타겟
    public Transform LHT; // 왼손 IK 타겟

    [Space(10)]

    [Header("Pistol Hand Position")]
    public Transform RHP_Pistol;
    public Transform LHP_Pistol;
    public Transform Aim_Pistol;
    [Space(10)]

    [Header("Rifle Hand Position")]
    public Transform RHP_Rifle;
    public Transform LHP_Rifle;
    public Transform Aim_Rifle;
    [Space(10)]

    bool IsHaveLHSocket;

    [Header("FootIK")]
    //public Transform LeftFoot, RightFoot;
    public Transform LFT;
    public Transform RFT;

    private Vector2 lookInput;
    float xRot = 0, yRot = 0;


    float HandWeight_L = 0;
    float HandWeight_R = 0; 

    bool LeftHandOn, LeftHandOff;
    float timer = 0;
    void Start()
    {
        pc = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();

        animator = pc.animator;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (pc.isEquipping || pc.isUnequipping || pc.isReloading)
        {
            
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandWeight_L);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandWeight_L);
        }

        else
        {
            
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandWeight_L);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandWeight_L);

        }

        #region HandIK
        if (pc.armState == GameManager.armState.Pistol)
        {
            if ( !pc.isReloading && !pc.isEquipping )
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandWeight_R);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandWeight_R);
            }

            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandWeight_R);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandWeight_R);
            }

            animator.SetIKPosition(AvatarIKGoal.RightHand, RHT.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RHT.rotation);

            if (LHP_Pistol != null)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, LHT.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, LHT.rotation);
            }



        }

        else if (pc.armState == GameManager.armState.Rifle)
        {
            

            if (!pc.isReloading && !pc.isEquipping)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandWeight_R);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandWeight_R);
            }

            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandWeight_R);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandWeight_R);
            }

            animator.SetIKPosition(AvatarIKGoal.RightHand, RHT.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RHT.rotation);

            if (LHP_Rifle != null)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, LHT.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, LHT.rotation);
            }
        }

        else
        {

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);

            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
        #endregion
    }

    private void Update()
    {
        #region IK 가중치 세팅
        if (pc.isEquipping || pc.isUnequipping || pc.isReloading)
        {
            HandWeight_L = Mathf.Lerp(HandWeight_L, 0, Time.deltaTime * 2f);
            HandWeight_R = Mathf.Lerp(HandWeight_R, 0, Time.deltaTime * 2f);
        }

        else
        {
            HandWeight_L = Mathf.Lerp(HandWeight_L, 1, Time.deltaTime * 2f);
            HandWeight_R = Mathf.Lerp(HandWeight_R, 1, Time.deltaTime * 2f);
        } 

      
        #endregion

        #region Hand IK
        if (pc.armState == GameManager.armState.Pistol)
        {

            if (LHP_Pistol == null)
            {
                currentGun = pc.WeaponSocket.transform.GetChild(0);
                LHP_Pistol = GameManager.FindChildRecursive(currentGun, "Left Hand Position");
            }

            if (pc.IsAimDown )
            {
                RHT.position = Vector3.Lerp(RHT.position, Aim_Pistol.position, Time.deltaTime * 15f);
                RHT.rotation = Quaternion.Slerp(RHT.rotation, Aim_Pistol.rotation, Time.deltaTime * 15f);
            }

            else
            {
                RHT.position = Vector3.Lerp(RHT.position, RHP_Pistol.position, Time.deltaTime * 15f);
                RHT.rotation = Quaternion.Slerp(RHT.rotation, RHP_Pistol.rotation, Time.deltaTime * 15f);
            }


                LHT.position = LHP_Pistol.position;
            LHT.rotation = LHP_Pistol.rotation;
        }

        else if (pc.armState == GameManager.armState.Rifle)
        {
            if (LHP_Rifle == null)
            {
                currentGun = pc.WeaponSocket.transform.GetChild(0);
                LHP_Rifle = GameManager.FindChildRecursive(currentGun, "Left Hand Position");
            }

            if (pc.IsAimDown)
            {

                RHT.position = Vector3.Lerp(RHT.position, Aim_Rifle.position, Time.deltaTime * 5f); //  
                RHT.rotation = Quaternion.Slerp(RHT.rotation, Aim_Rifle.rotation, Time.deltaTime * 5f); //RHP_Pistol.rotation; 
            }

            else
            {

                RHT.position = Vector3.Lerp(RHT.position, RHP_Rifle.position, Time.deltaTime * 5f);
                RHT.rotation = Quaternion.Slerp(RHT.rotation, RHP_Rifle.rotation, Time.deltaTime * 5f);
            }

            LHT.position = LHP_Rifle.position;
            LHT.rotation = LHP_Rifle.rotation;
        }

        else
        {
            currentGun = null;
            LHP_Pistol = null;
        }
        #endregion

    }



    void SetFootIKPosition()
    {
        if (controller.velocity == Vector3.zero)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

            animator.SetIKPosition(AvatarIKGoal.LeftFoot, LFT.position);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, LFT.rotation);

            animator.SetIKPosition(AvatarIKGoal.RightFoot, RFT.position);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, RFT.rotation);
        }

        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        xRot += lookInput.x * Time.deltaTime * 5f;
        yRot -= lookInput.y * Time.deltaTime * 5f;

        yRot = Mathf.Clamp(yRot, -30f, 70f);
    }

    public void DetachMagazine()
    {
        if (pc.armState != GameManager.armState.Unarmed)
        {
            pc.isReloading = true;
            pc.currentWeapon.GetComponent<WeaponScript>().Mags.localScale = Vector3.zero;
        }
    }

    public void ThrowMagazine()
    {

    }

    public void NewMagazine()
    { 
    }

    public void InsertMagazine()
    {
        pc.currentWeapon.GetComponent<WeaponScript>().Mags.localScale = Vector3.one;
        pc.currentWeapon.GetComponent<WeaponScript>().ReloadComplete();
        pc.isReloading = false;
    }

    public void LeftHandIKON()
    {
        LeftHandOn = true;
    }

    public void LeftHandIKOff()
    {

        LeftHandOff = true;

    }

}
