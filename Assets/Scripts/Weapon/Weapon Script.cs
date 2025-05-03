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
    public GameObject Bullet;
    public GameObject Shell;

    [Header("무기 설정")]
    public PoolManager.PoolingObjectType bulletPooingType;
    public PoolManager.PoolingObjectType shellPoolingType;
    public bool isequipped;
    public int MagzCapacity;
    public int currentAmmo;
    public FireMode fireMode;
    public Vector3 AimPoint;
    [Space(10)]

    [Header("트랜스폼")]
    public Transform FirePoint;
    public Transform shellEjection;
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
                // 반동 적용
                GetComponent<RecoilController>().ApplyRecoil(); 

                // 탄창 갱신
                currentAmmo--;

                //소리
                FireSound.PlayOneShot(FireClip); 

                // 탄 풀링
                PoolManager.instance.Instantiate(Bullet, FirePoint.position, Quaternion.LookRotation(FirePoint.forward), bulletPooingType);

                // 탄피 풀링
                if(shellEjection != null)
                {
                    GameObject shell = PoolManager.instance.Instantiate(Shell, shellEjection.position + shellEjection.forward * 0.02f, Quaternion.identity, shellPoolingType);
                    shell.GetComponent<Rigidbody>().AddForce(shellEjection.right * 200f, ForceMode.Force);
                    shell.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);

                }


                Debug.DrawRay(FirePoint.position, FirePoint.forward * 30f, Color.red, 10f);
                pc.GetComponent<AimOffset>().xRot += Random.Range(-0.1f, 0.3f);
                pc.GetComponent<AimOffset>().yRot -= Random.Range(0.1f, 0.7f);

                pc.GetComponent<AimOffset>().ShakeAim();
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
        PlayerController pc = GetComponentInParent<PlayerController>();
        int ammoIndex;
        switch (pc.armState)
        {
            case GameManager.armState.Pistol:
                ammoIndex = pc.GetComponent<InventoryManager>().FindItemIndexInInventoryByCode(3);

                if(pc.GetComponent<InventoryManager>().Inventory[ammoIndex].ItemQuantity > MagzCapacity)
                {
                    currentAmmo = MagzCapacity;
                    pc.GetComponent<InventoryManager>().Inventory[ammoIndex].ItemQuantity -= MagzCapacity;
                }

                else
                {
                    currentAmmo = pc.GetComponent<InventoryManager>().Inventory[ammoIndex].ItemQuantity;

                    pc.GetComponent<InventoryManager>().Inventory.RemoveAt(ammoIndex);
                    pc.GetComponent<UserInterfaceLogic>().PrintInventory();
                }

                break;

            case GameManager.armState.Rifle:
                ammoIndex = pc.GetComponent<InventoryManager>().FindItemIndexInInventoryByCode(4);

                if (pc.GetComponent<InventoryManager>().Inventory[ammoIndex].ItemQuantity > MagzCapacity)
                {
                    currentAmmo = MagzCapacity;
                    pc.GetComponent<InventoryManager>().Inventory[ammoIndex].ItemQuantity -= MagzCapacity;
                }

                else
                {
                    currentAmmo = pc.GetComponent<InventoryManager>().Inventory[ammoIndex].ItemQuantity;

                    pc.GetComponent<InventoryManager>().Inventory.RemoveAt(ammoIndex);
                    pc.GetComponent<UserInterfaceLogic>().PrintInventory();
                }


                break;
        }


    }

}
