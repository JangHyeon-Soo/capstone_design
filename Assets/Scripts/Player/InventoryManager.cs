using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public List<GameManager.Item> Inventory;

    public void AddItemToInventory(GameManager.Item item)
    {
        if (Inventory.Count == 0)
        {
            Inventory.Add(item);
            return;
        }

        else
        {
            int index = FindItemIndexInInventoryByCode(item.Code);
            if ( index == -1)
            {
                Inventory.Add(item);
                return;
            }

            else
            {
                Inventory[index].ItemQuantity += item.ItemQuantity;
            }
        }
    }

   public int FindItemIndexInInventoryByCode(int code)
   {
        if (Inventory.Count == 0) return -1;

        for(int i = 0; i < Inventory.Count; ++i)
        {
            if(Inventory[i].Code == code)
            {
                return i;
            }
        }


        return -1;

   }
}
