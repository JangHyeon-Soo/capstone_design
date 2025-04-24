
using UnityEngine;
using System.Collections;

public class RecoilController : MonoBehaviour
{
    public PlayerController Owner;
    public Transform ikTarget;         // ������ IK Ÿ�� (ī�޶� �ڽ�)
    public Vector3 recoilOffset; // �ݵ�: �� + �ڷ�
    public Quaternion rotationOffset;
    public float recoilDuration = 0.5f;
    public float returnDuration = 0.1f;

    private Vector3 originalLocalPosition;

    public Rigidbody[] spineRigidbodies; // Spine1 ~ Spine5
    public float recoilAmount = 5f;
    public float recoilSpeed = 5f;

    
    // ī�޶� ���ؿ��� IK Ÿ���� ��� �� �ִ� �ִ� �Ÿ�
    public float maxRecoilDistance = 0.15f;
    
    [HideInInspector]public bool isRecoiling;


    //public float recoilAmount = 5f;
    public float frequency = 20f;
    public float damping = 6f;
    //public float recoilDuration = 0.5f;

    private float recoilTime = 0f;
    private Quaternion originalRot;
    public void Start()
    {
        if(gameObject.tag == "Player")
        {
            Owner = GetComponentInParent<PlayerController>();
        }
    }
    public void ApplyRecoil()
    {
        if (ikTarget == null) return;
        else if (isRecoiling) return;

        // ���� �÷���
        transform.GetComponent<WeaponScript>().FireEffect(); 

        //ī�޶� ����
        Owner.GetComponent<PlayerController>().playerCamera.GetComponent<CamController>().StartCameraShake(0.1f, 0.001f, 2f); // 1��, ���� 0.2, �� 2

        isRecoiling = true;
        recoilTime = 0;


        //�ݵ� 
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

        // ������ ���� (�ѱ� ����)
        Vector3 recoilDir = transform.GetComponent<WeaponScript>().FirePoint.forward.normalized;

        // ������ ��ġ ������ (�ڷ�)
        Vector3 posOffset = recoilDir * Random.Range(-0.03f, -0.07f);
        Vector3 targetPos = originalPos + posOffset;

        float recoilDuration = 0.04f;
        float returnDuration = 0.04f;

        float time = 0f;

        // �ݵ� Ʀ (������)
        while (time < recoilDuration)
        {
            float t = time / recoilDuration;
            ikTarget.localPosition = Vector3.Lerp(originalPos, targetPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        // ����ġ ���� + ���� ���� ȸ��
        time = 0f;
        while (time < returnDuration)
        {
            float t = time / returnDuration;

            // �⺻ ���� ��ġ
            Vector3 currentPos = Vector3.Lerp(targetPos, originalPos, t);

            // ��ġ ����
            ikTarget.localPosition = currentPos;

            time += Time.deltaTime;
            yield return null;
        }

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


