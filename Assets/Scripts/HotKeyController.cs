using UnityEngine;
using UnityEngine.InputSystem;

public class HotKeyController : MonoBehaviour
{
    PlayerController pc;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }
    public void OnHotkey_1() //Rifle
    {
        
        PlayerController pc = transform.GetComponent<PlayerController>();

        if (pc.armState == GameManager.armState.Pistol) return;

            
        if (pc.armState == GameManager.armState.Unarmed && pc.RifleSocket.transform.childCount > 0 && !pc.isEquipping && !pc.isUnequipping)
        {
            pc.isEquipping = true;
            pc.animator.SetTrigger("1_Equip");
        }

        else if (pc.armState == GameManager.armState.Rifle && pc.WeaponSocket.transform.childCount > 0 && !pc.isEquipping && !pc.isUnequipping)
        {
            pc.isUnequipping = true;
            pc.animator.SetTrigger("1_Unequip");
        }
    }

    


    public void OnHotkey_2()
    {
        PlayerController pc = transform.GetComponent<PlayerController>();

        if (pc.armState == GameManager.armState.Rifle) return;

        if (pc.armState == GameManager.armState.Unarmed && pc.PistolSocket.transform.childCount > 0 && !pc.isEquipping && !pc.isUnequipping)
        {
            pc.isEquipping = true;
            pc.animator.SetTrigger("2_Equip");

        }

        else if(pc.armState == GameManager.armState.Pistol && pc.WeaponSocket.transform.childCount > 0 && !pc.isEquipping && !pc.isUnequipping)
        {
            pc.isUnequipping = true;
            pc.animator.SetTrigger("2_Unequip");
        }

        

    }

    [Tooltip("1 : Rifle, 2: Pistol")]
    public void EquipWeapon(int WeaponCode)
    {

        switch (WeaponCode)
        {
            case 1:
                if (pc.RifleSocket.transform.childCount > 0 && pc.RifleSocket.transform.GetChild(0).GetComponent<ItemMananger>() != null)
                {
                    Transform obj = pc.RifleSocket.transform.GetChild(0);
                    pc.currentWeapon = obj.GetComponent<WeaponScript>();

                    obj.localRotation = Quaternion.Euler(0, 0, 0);
                    obj.localPosition = new Vector3(0, 0, 0);

                    obj.SetParent(pc.WeaponSocket.transform);
                    pc.armState = GameManager.armState.Rifle;
                    //pc.animator.SetLayerWeight(2, 1);

                    obj.localPosition = new Vector3(0, 0, 0);
                    obj.localRotation = Quaternion.Euler(-90, 0, 0);
                }
                break;


            case 2:

                if (pc.PistolSocket.transform.childCount > 0 && pc.PistolSocket.transform.GetChild(0).GetComponent<ItemMananger>() != null)
                {
                    Transform obj = pc.PistolSocket.transform.GetChild(0);

                    pc.currentWeapon = obj.GetComponent<WeaponScript>();
                    obj.localRotation = Quaternion.Euler(0, 0, 0);
                    obj.localPosition = new Vector3(0, 0, 0);

                    obj.SetParent(pc.WeaponSocket.transform);
                    pc.armState = GameManager.armState.Pistol;
                    //pc.animator.SetLayerWeight(1, 1);

                    obj.localPosition = new Vector3(0, 0, 0);
                    obj.localRotation = Quaternion.Euler(-90, 0, 0);
                    //obj.GetComponent<RecoilController>().ikTarget = obj.GetComponentInParent<HandIKController>().RHT;

                }

                break;
        }


    }

    [Tooltip("1 : Rifle, 2: Pistol")]
    public  void UnequipWeapon(int WeaponCode)
    {
        switch (pc.armState)
        {
            case GameManager.armState.Pistol:

                if (pc.WeaponSocket.transform.childCount > 0 && pc.WeaponSocket.transform.GetChild(0).GetComponent<ItemMananger>() != null)
                {
                    Transform obj = pc.WeaponSocket.transform.GetChild(0);

                    pc.currentWeapon = null;
                    obj.SetParent(pc.PistolSocket.transform);
                    pc.armState = GameManager.armState.Unarmed;
                    //pc.animator.SetLayerWeight(1, 0);

                    obj.localPosition = new Vector3(0, 0, 0);
                    obj.localRotation = Quaternion.Euler(-90, 0, 0);

                }

                break;

            case GameManager.armState.Rifle:

                if (pc.WeaponSocket.transform.childCount > 0 && pc.WeaponSocket.transform.GetChild(0).GetComponent<ItemMananger>() != null)
                {
                    Transform obj = pc.WeaponSocket.transform.GetChild(0);
                    pc.currentWeapon = null;
                    obj.SetParent(pc.RifleSocket.transform);
                    pc.armState = GameManager.armState.Unarmed;
                    //pc.animator.SetLayerWeight(2, 0);

                    obj.localPosition = new Vector3(0, 0, 0);
                    obj.localRotation = Quaternion.Euler(-90, 0, 0);

                    obj = pc.RifleSocket.transform.GetChild(0);


                }
                break;
        }


        //switch (WeaponCode)
        //{
        //    case 1: //Rifle
                
        //        break;


        //    case 2: //Pistol
        //        if (pc.WeaponSocket.transform.childCount > 0 && pc.WeaponSocket.transform.GetChild(0).GetComponent<ItemMananger>() != null)
        //        {
        //            Transform obj = pc.WeaponSocket.transform.GetChild(0);

        //            pc.currentWeapon = null;
        //            obj.SetParent(pc.PistolSocket.transform);
        //            pc.armState = GameManager.armState.Unarmed;
        //            //pc.animator.SetLayerWeight(1, 0);

        //            obj.localPosition = new Vector3(0, 0, 0);
        //            obj.localRotation = Quaternion.Euler(-90, 0, 0);

        //        }
        //        break;
        //}

        
    }


    public void SetArmState(int ArmState)
    {
        Debug.Log("Done");
        pc.armState = (GameManager.armState)ArmState;
        pc.isEquipping = false;
        pc.isUnequipping = false;
    }

    public void SetLayerOn(int index)
    {
        pc.animator.SetLayerWeight(index, 1);
    }

    public void SetLayerOff(int index)
    {
        pc.animator.SetLayerWeight(index, 0);
    }



}
