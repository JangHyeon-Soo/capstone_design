using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
    public Image weaponImage;
    public TMP_Text weaponName;
    public TMP_Text weaponAmmo;
    public TMP_Text weaponFireMode;

    PlayerController pc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(pc.armState != GameManager.armState.Unarmed)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.one, Time.deltaTime * 3f);

            weaponName.text = pc.currentWeapon.GetComponent<ItemMananger>().item.ItemName;
            weaponAmmo.text = pc.currentWeapon.GetComponent<WeaponScript>().currentAmmo + " / " + pc.currentWeapon.GetComponent<WeaponScript>().MagzCapacity;
            weaponImage.sprite = pc.currentWeapon.GetComponent<ItemMananger>().item.ItemImage;
            weaponFireMode. text = pc.currentWeapon.GetComponent<WeaponScript>().fireMode.ToString();
        }

        else
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, Time.deltaTime * 3f);
            return;
        }
    }
}
