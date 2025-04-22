using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class UserInterfaceLogic : MonoBehaviour
{
    public PlayerController pc;
    public CamController camController;
    public PlayerInput playerInput;
    InputAction inventoryAction;
    [Space(10)]

    [Header("UI")]
    public GameObject InventoryUI;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerInput = transform.parent.GetComponentInChildren<PlayerInput>();
        inventoryAction = playerInput.actions.FindAction("Inventory");

        InventoryUI.SetActive(false);
    }

    public void OnInventory(InputValue value)
    {
        
        InventoryUI.SetActive(!InventoryUI.activeSelf);

        if (InventoryUI.activeSelf)
        {
            GameManager.CursorOn();
            camController.InputOn = false;
            pc.InputOn = false;
            PrintInventory();

        }

        else
        {
            GameManager.CursorOff();
            pc.InputOn = true;
            camController.InputOn = true;
        }
    }
    
    public void PrintInventory()
    {
        Button[] cells = InventoryUI.GetComponentsInChildren<Button>();
        List<GameManager.Item> items = pc.transform.GetComponent<InventoryManager>().Inventory;
        int cellCount = cells.Length;

        if(items.Count == 0)
        {
            for (int i = 0; i < cellCount; ++i)
            {
                cells[i].GetComponent<Image>().sprite = null;
            }
        }

        else
        {
            for (int i = 0; i < cellCount; ++i)
            {
                if (i >= items.Count)
                {
                    cells[i].GetComponent<Image>().sprite = null;
                    cells[i].GetComponent<InventoryCellLogic>().Cellitem = null;
                }

                else
                {
                    cells[i].GetComponent<Image>().sprite = items[i].ItemImage;
                    cells[i].GetComponent<InventoryCellLogic>().Cellitem = items[i];
                }
                
            }
        }
        
    }

}
