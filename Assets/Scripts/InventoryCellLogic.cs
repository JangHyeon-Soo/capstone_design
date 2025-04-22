using UnityEngine;

public class InventoryCellLogic : MonoBehaviour
{
    UserInterfaceLogic uif;
    public GameManager.Item Cellitem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uif = GetComponentInParent<UserInterfaceLogic>();   
    }


    public void OnClick()
    {
        if (Cellitem == null) return;

        ItemManagement.instance.ItemFunction(Cellitem.Code);
        
        GetComponentInParent<InventoryManager>().Inventory.RemoveAt(GetComponentInParent<InventoryManager>().FindItemIndexInInventoryByCode(Cellitem.Code));
        uif.PrintInventory();
        Cellitem = null;
    }
}
