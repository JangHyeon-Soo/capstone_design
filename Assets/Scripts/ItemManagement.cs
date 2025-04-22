using UnityEngine;
using System.Collections.Generic;

public class ItemManagement : MonoBehaviour
{
    public static ItemManagement instance;

    public List<ItemMananger> itemList;

    void Start()
    {
        instance = this;
    }

    public void ItemFunction(int itemCode)
    {
        GameObject obj;
        switch (itemCode)
        {
            case 1: //Pistol

                obj= Instantiate(itemList[itemCode - 1].gameObject);

                obj.transform.SetParent(GameObject.FindGameObjectWithTag("PistolSocket").transform);

                obj.GetComponent<ItemMananger>().SetTag("Player");

                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localRotation = Quaternion.Euler(-90, 0, 0);

                obj.GetComponent<WeaponScript>().isequipped = true;
                
                
                obj.GetComponent<Rigidbody>().isKinematic = true;
                obj.GetComponent<Collider>().enabled = false;
                

                break;

            case 2: //Rifle
                
                obj = Instantiate(itemList[itemCode - 1].gameObject);

                obj.transform.SetParent(GameObject.FindGameObjectWithTag("RifleSocket").transform);
                obj.GetComponent<ItemMananger>().SetTag("Player");
               

                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localRotation = Quaternion.Euler(-90, 0, 0);

                obj.GetComponent<WeaponScript>().isequipped = true;
                obj.GetComponent<Rigidbody>().isKinematic = true;
                obj.GetComponent<Collider>().enabled = false;
                
                break;
        }

    }
}
