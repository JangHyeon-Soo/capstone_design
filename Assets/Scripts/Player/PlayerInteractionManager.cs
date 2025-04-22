using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionManager : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Camera playerCamera;
    public LayerMask interactionLayer;
    public float rayDistance;

    public GameObject interactionObject;

    public TMP_Text interactionText;
    Vector3 center;

    PlayerInput playerInput;
    InputAction IAinput;
    void Start()
    {
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // 상호작용 인풋
        playerInput = GetComponent<PlayerInput>();
        IAinput = playerInput.actions.FindAction("Interaction");

        if (inventoryManager == null) GetComponent<InventoryManager>();

    }


    void Update()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(center);
        ray.direction = playerCamera.transform.forward;
        
        bool isHit = Physics.Raycast(ray.origin, playerCamera.transform.forward, out hit, rayDistance, interactionLayer);
        if(isHit && hit.transform.GetComponent<ItemMananger>())
        {
            interactionObject = hit.transform.gameObject;
            interactionText.text = "[" + playerInput.actions.FindAction("Interaction").bindings[0].path.Split('/')[1].ToUpper() + "]"; 
        }

        else
        {
            interactionObject = null;
            interactionText.text = "";

        }

        Debug.DrawRay(ray.origin, playerCamera.transform.forward * rayDistance, isHit ? Color.red : Color.white);
        
    }

    public void OnInteraction(InputValue value)
    {
        if (interactionObject == null) return;

        

        switch (interactionObject.GetComponent<ItemMananger>().item.itemType)
        {
            case GameManager.ItemType.Weapon:
                
                break;
        }


        inventoryManager.AddItemToInventory(interactionObject.GetComponent<ItemMananger>().item);
        Destroy(interactionObject);
        interactionObject = null;

    }
}
