
using UnityEngine;
using System.Collections;

public class RecoilController : MonoBehaviour
{
    public Transform Owner;
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

        // ������ ���� (�ѱ� ����)
        Vector3 recoilDir = transform.GetComponent<WeaponScript>().FirePoint.forward.normalized;

        // ������ ��ġ ������ (�ڷ�)
        Vector3 posOffset = recoilDir * Random.Range(-0.03f, -0.07f);
        Vector3 targetPos = originalPos + posOffset;

        //// ���� ���� ȸ�� ���� (�ѱ� ���� ȸ��)
        //float dirAmplitude = 6f;    // �ѱ� ���� ȸ���� (degrees)
        //float dirFrequency = 20f;   // ���� �ӵ�
        //float dirDamping = 5f;      // ���� �ӵ�

        //// ���� ���� ȸ�� ���� (���� X�� ȸ��)
        //float xAmplitude = 4f;     // ���� X�� ȸ���� (degrees)
        //float xFrequency = 18f;    // ���� �ӵ�
        //float xDamping = 5f;       // ���� �ӵ�

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

            //// ���� ���� �� (�ѱ� ����)
            //float dirOscillation = dirAmplitude * Mathf.Sin(dirFrequency * time) * Mathf.Exp(-dirDamping * time);
            //dirOscillation = Mathf.Clamp(dirOscillation, -2f, 2f);
            //Quaternion dirRotationOffset = Quaternion.AngleAxis(dirOscillation, recoilDir);
            

            //// ���� ���� �� (���� X��)
            //float xOscillation = xAmplitude * Mathf.Sin(xFrequency * time) * Mathf.Exp(-xDamping * time);
            //xOscillation = Mathf.Clamp(xOscillation, -3f, 3f);

            //// ���� ���� �� (���� Z��)
            //float zOscillation = xAmplitude * Mathf.Sin(xFrequency * time + Mathf.PI / 2f) * Mathf.Exp(-xDamping * time);
            //zOscillation = Mathf.Clamp(zOscillation, -2f, 2f);

            //Quaternion xRotationOffset = Quaternion.Euler(xOscillation, 0f, zOscillation);

            //// ���� ȸ�� ���� (�ѱ� ���� + ���� X��)
            //ikTarget.localRotation = originalRot * dirRotationOffset * xRotationOffset;

            // ��ġ ����
            ikTarget.localPosition = currentPos;

            time += Time.deltaTime;
            yield return null;
        }

        // ������ ����ġ
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


