using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(AudioSource))]
public class WeaponScript : MonoBehaviour
{
    public enum FireMode
    {
        Single,
        Auto
    }


    public PlayerController pc;

    [Header("무기 설정")]
    public bool isequipped;
    public int MagzCapacity;
    public int currentAmmo;
    public FireMode fireMode;
    public Vector3 AimPoint;
    
    [Space(10)]

    [Header("트랜스폼")]
    public Transform FirePoint;
    public Transform AimSocket;
    public Transform Mags;
    public Transform MagsPrefab;

    public Transform MagsSocket;
    [Space(10)]

    [Header("VFX")]
    public VisualEffect vfx;
    public ParticleSystem muzzleFlash;
    [Space(10)]

    [Header("SFX")]
    public AudioSource FireSound;
    public AudioClip FireClip;
    private void Start()
    {
        if (gameObject.tag != "Player") return;

        FirePoint = GameManager.FindChildRecursive(transform, "FirePoint");
        currentAmmo = MagzCapacity;

        FireSound = GetComponent<AudioSource>();
        pc = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if(FirePoint != null)
            AimPoint = FirePoint.position + FirePoint.forward * 30f;

        Debug.DrawRay(FirePoint.position, FirePoint.forward * 30f, Color.white);

    }
    public void FireStart()
    {
        if (gameObject.tag != "Player") return;

        switch (fireMode)
        {
            case FireMode.Single:

                if (GetComponentInParent<PlayerController>() == null) 
                    return;

                else 
                    
                    Fire();

                break;

            case FireMode.Auto:
                InvokeRepeating(nameof(Fire), 0f, 0.1f);
                //Fire();
                break;
        }
    }

    public void Fire()
    {
        if(currentAmmo > 0)
        {
            if(GetComponent<RecoilController>().isRecoiling == false)
            {
                GetComponent<RecoilController>().ApplyRecoil();
                currentAmmo--;
                FireSound.PlayOneShot(FireClip);
                pc.GetComponent<AimOffset>().xRot += Random.Range(-0.1f, 0.3f);
                pc.GetComponent<AimOffset>().yRot -= Random.Range(0.1f, 0.7f);
            }
            
        }

        else
        {
            CancelInvoke();
        }
    }

    public void FireEffect()
    {
        //vfx.SetVector3("FirePosition", FirePoint.position);
        //vfx.SetVector3("minFireDir", Quaternion.AngleAxis(-10, Vector3.up) * FirePoint.forward);
        //vfx.SetVector3("maxFireDir", Quaternion.AngleAxis(10, Vector3.up) * FirePoint.forward);
        //vfx.Play();
        muzzleFlash.Play();
        //GameObject obj = Instantiate(vfx, FirePoint.position, Quaternion.LookRotation(FirePoint.forward));
    }

    public void Reload()
    {
        PlayerController pc = GetComponentInParent<PlayerController>();
        pc.isReloading = true; 
    }

    public void ReloadComplete()
    {
        GetComponentInParent<PlayerController>().isReloading = false;
        currentAmmo = MagzCapacity;
    }

}
