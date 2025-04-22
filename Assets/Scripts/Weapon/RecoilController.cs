
using UnityEngine;
using System.Collections;

public class RecoilController : MonoBehaviour
{
    public Transform Owner;
    public Transform ikTarget;         // 오른손 IK 타겟 (카메라 자식)
    public Vector3 recoilOffset; // 반동: 위 + 뒤로
    public Quaternion rotationOffset;
    public float recoilDuration = 0.5f;
    public float returnDuration = 0.1f;

    private Vector3 originalLocalPosition;

    public Rigidbody[] spineRigidbodies; // Spine1 ~ Spine5
    public float recoilAmount = 5f;
    public float recoilSpeed = 5f;

    
    // 카메라 기준에서 IK 타겟이 벗어날 수 있는 최대 거리
    public float maxRecoilDistance = 0.15f;
    
    [HideInInspector]public bool isRecoiling;


    //public float recoilAmount = 5f;
    public float frequency = 20f;
    public float damping = 6f;
    //public float recoilDuration = 0.5f;

    private float recoilTime = 0f;
    //private bool isRecoiling = false;
    private Quaternion originalRot;
    public void Start()
    {
        if(gameObject.tag == "Player")
        {
            Owner = transform.root;
        }
    }
    public void ApplyRecoil()
    {
        if (ikTarget == null) return;
        else if (isRecoiling) return;

        transform.GetComponent<WeaponScript>().FireEffect();

        isRecoiling = true;
        recoilTime = 0;

        StopAllCoroutines();
        StartCoroutine(DoRecoil());
    }

    private void Update()
    {
        if (gameObject.tag != "Player") return;

        SetIKTarget();
    }

    private IEnumerator DoRecoil()
    {
        if (ikTarget == null) yield break;

        isRecoiling = true;

        Vector3 originalPos = ikTarget.localPosition;
        Quaternion originalRot = ikTarget.localRotation;

        // 리코일 방향 (총구 방향)
        Vector3 recoilDir = transform.GetComponent<WeaponScript>().FirePoint.forward.normalized;

        // 리코일 위치 오프셋 (뒤로)
        Vector3 posOffset = recoilDir * Random.Range(-0.03f, -0.07f);
        Vector3 targetPos = originalPos + posOffset;

        //// 감쇠 진동 회전 변수 (총구 방향 회전)
        //float dirAmplitude = 6f;    // 총구 방향 회전량 (degrees)
        //float dirFrequency = 20f;   // 진동 속도
        //float dirDamping = 5f;      // 감쇠 속도

        //// 감쇠 진동 회전 변수 (로컬 X축 회전)
        //float xAmplitude = 4f;     // 로컬 X축 회전량 (degrees)
        //float xFrequency = 18f;    // 진동 속도
        //float xDamping = 5f;       // 감쇠 속도

        float recoilDuration = 0.04f;
        float returnDuration = 0.04f;

        float time = 0f;

        // 반동 튐 (빠르게)
        while (time < recoilDuration)
        {
            float t = time / recoilDuration;
            ikTarget.localPosition = Vector3.Lerp(originalPos, targetPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        // 원위치 복귀 + 감쇠 진동 회전
        time = 0f;
        while (time < returnDuration)
        {
            float t = time / returnDuration;

            // 기본 복귀 위치
            Vector3 currentPos = Vector3.Lerp(targetPos, originalPos, t);

            //// 감쇠 진동 값 (총구 방향)
            //float dirOscillation = dirAmplitude * Mathf.Sin(dirFrequency * time) * Mathf.Exp(-dirDamping * time);
            //dirOscillation = Mathf.Clamp(dirOscillation, -2f, 2f);
            //Quaternion dirRotationOffset = Quaternion.AngleAxis(dirOscillation, recoilDir);
            

            //// 감쇠 진동 값 (로컬 X축)
            //float xOscillation = xAmplitude * Mathf.Sin(xFrequency * time) * Mathf.Exp(-xDamping * time);
            //xOscillation = Mathf.Clamp(xOscillation, -3f, 3f);

            //// 감쇠 진동 값 (로컬 Z축)
            //float zOscillation = xAmplitude * Mathf.Sin(xFrequency * time + Mathf.PI / 2f) * Mathf.Exp(-xDamping * time);
            //zOscillation = Mathf.Clamp(zOscillation, -2f, 2f);

            //Quaternion xRotationOffset = Quaternion.Euler(xOscillation, 0f, zOscillation);

            //// 최종 회전 적용 (총구 방향 + 로컬 X축)
            //ikTarget.localRotation = originalRot * dirRotationOffset * xRotationOffset;

            // 위치 복귀
            ikTarget.localPosition = currentPos;

            time += Time.deltaTime;
            yield return null;
        }

        // 완전히 원위치
        //ikTarget.localPosition = originalPos;
        //ikTarget.localRotation = originalRot;

        isRecoiling = false;
    }

    public void SetIKTarget()
    {
        PlayerController pc = GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            switch (pc.armState)
            {
                case GameManager.armState.Unarmed:
                    ikTarget = null;
                    break;

                case GameManager.armState.Pistol:
                    ikTarget = pc.GetComponent<HandIKController>().RHT;
                    break;

                case GameManager.armState.Rifle:
                    ikTarget = pc.GetComponent<HandIKController>().RHT;
                    break;
            }
        }
    }
}


