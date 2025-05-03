using UnityEngine;
using TMPro;

public class InventoryCellLogic : MonoBehaviour
{
    UserInterfaceLogic uif;
    public GameManager.Item Cellitem;
    public TMP_Text quantityText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uif = GetComponentInParent<UserInterfaceLogic>();

        quantityText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if(Cellitem == null)
        {
            quantityText.text = "0";
        }

        else
        {
            quantityText.text = Cellitem.ItemQuantity.ToString();
        }
    }

    public void OnClick()
    {
        if (Cellitem == null) return;

        ItemManagement.instance.ItemFunction(Cellitem.Code);

        switch (Cellitem.Code)
        {
            case 1:
                ClearCell();
                break;

            case 2:
                ClearCell();
                break;

            case 3:

                break;

            case 4:

                break;
        }


    }

    private void ClearCell()
    {
        if(Cellitem.ItemQuantity == 1)
        {
            GetComponentInParent<InventoryManager>().Inventory.RemoveAt(GetComponentInParent<InventoryManager>().FindItemIndexInInventoryByCode(Cellitem.Code));
            uif.PrintInventory();
            Cellitem = null;
        }

        else
        {
            InventoryManager im = GetComponentInParent<InventoryManager>();

            im.Inventory[im.FindItemIndexInInventoryByCode(Cellitem.Code)].ItemQuantity -= 1;
            uif.PrintInventory();
        }

    }
}
