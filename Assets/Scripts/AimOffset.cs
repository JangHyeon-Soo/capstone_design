using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Animations.Rigging;
using System;

public class AimOffset : MonoBehaviour
{
    CharacterController controller;
    PlayerController pc;

   

    private Vector2 lookInput;
    public Transform AimPoint;
    public Transform sphere;
    public Rig rig;

    public float xRot = 0, yRot = 0;


    [Header("Turn In Place")]
    public string Left90dAnimName;
    public string Right90dAnimName;
    public string Left180dAnimName;
    public string Right180dAnimName;

    public Transform LFP;
    public Transform RFP;
    public Transform RF_Target;
    public Transform LF_Target;

    [Space(20)]

    Animator animator;
    public bool turn;

    float turnDuration = 0.2f;
   

    //Vector3 midRot; 
    Vector3 startPos_l, startPos_r;
    Quaternion startRot, midRot, endRot;
    Vector3 aimStartDir, aimMidDir,aimEndDir;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        pc = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();




    }

    private void Update()
    {
        if(pc.isVaulting || !pc.InputOn) return;

        Vector3 aimDir = AimPoint.forward;
        aimDir.y = 0;

        LFP.position = transform.position + AimPoint.right * -0.12f;
        LFP.rotation = Quaternion.Euler(0, xRot, 0);

        RFP.position = transform.position + AimPoint.right * 0.12f;
        RFP.rotation = Quaternion.Euler(0, xRot, 0);

        Debug.DrawRay(LFP.position, LFP.forward, Color.yellow);
        Debug.DrawRay (RFP.position, RFP.forward, Color.yellow);

        Debug.DrawRay(AimPoint.position, aimDir * 3f, Color.blue);
        Debug.DrawRay(controller.transform.position, controller.transform.forward * 3f, Color.red);
        float offset = Vector3.SignedAngle(transform.forward, aimDir, Vector3.up);


        if (!pc.isMove)
        {
            #region TurnInPlace
            ///턴인플레이스 부분
            if (pc.isGrounded)
            {
                if (Mathf.Abs(offset) > 75 && !turn)
                {
                    turn = true;
                    animator.applyRootMotion = true;


                    if (offset > 0)
                    {

                        if (offset > 135) animator.Play(Right180dAnimName);
                        else animator.Play(Right90dAnimName);

                    }

                    else
                    {


                        if (offset < -135) animator.Play(Left180dAnimName);
                        else animator.Play(Left90dAnimName);
                    }


                }

                if (turn)
                {

                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
                    {
                        turn = false;
                        animator.applyRootMotion = false;
                    }
                } 
            }
            /////////////////////////////////////////////////////////////////// 
            #endregion

            AimPoint.rotation = Quaternion.Lerp(AimPoint.rotation, Quaternion.Euler(yRot, xRot, 0), Time.deltaTime * 5f);

            
        }

        else
        {

            AimPoint.rotation = Quaternion.Lerp(AimPoint.rotation, Quaternion.Euler(yRot, xRot, 0), Time.deltaTime * 20f);
            pc.controller.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, xRot, 0), Time.deltaTime * 20f);


            turn = false;
            animator.applyRootMotion = false;

        }




    }

    public void RootMotionOn()
    {
        if(animator != null)
        {
            animator.applyRootMotion = true;
        }
    }

    public void OnLook(InputValue value)
    {
        
        if(turn) return;
        lookInput = value.Get<Vector2>();

        xRot += lookInput.x * Time.deltaTime * 5f;
        yRot -= lookInput.y * Time.deltaTime * 5f;

        xRot %= 360;

        
        //xRot = Mathf.Clamp(xRot, -90f, 90f);
        yRot = Mathf.Clamp(yRot, -30f, 70f);
    }

    public IEnumerator Turn(float amount, float duration)
    {
        turn = true;
        Debug.Log("turn");

        float currentTime = 0;
        
        float t;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, amount, 0);

        Quaternion aimRot = AimPoint.rotation;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;

            t = currentTime / turnDuration; 

            Quaternion rot = Quaternion.Lerp(startRot, targetRot, t);
            transform.rotation = rot;


            //WxRot += (pc.playerModel.rotation.y - rot.y) * (amount < 0 ? 1 : -1) * t;
            //AimPoint.rotation = aimRot;
            //AimPoint.localRotation = Quaternion.Lerp(AimPoint.localRotation, Quaternion.Euler(yRot, xRot, 0), Time.deltaTime * 20f);

            yield return null;
        }

        turn = false;
    }


}
