using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerController pc;
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
        pc = GetComponent<PlayerController>();

        if (inventoryManager == null) GetComponent<InventoryManager>();

    }


    void Update()
    {
        switch (pc.cameraMode)
        {
            case GameManager.CameraMode.FPS:

                Ray ray = playerCamera.ScreenPointToRay(center);
                ray.direction = playerCamera.transform.forward;
                RaycastHit[] hit = Physics.RaycastAll(ray.origin, playerCamera.transform.forward, rayDistance, interactionLayer);


                bool isHit = hit.Length > 0;
                Debug.DrawRay(ray.origin, playerCamera.transform.forward * rayDistance, isHit ? Color.red : Color.white);

                if (isHit)
                {
                    interactionObject = hit[0].transform.gameObject;

                    switch (interactionObject.layer)
                    {
                        case 3:
                            interactionText.text = "[" + playerInput.actions.FindAction("Interaction").bindings[0].path.Split('/')[1].ToUpper() + "]";
                            break;

                        case 11:
                            if(pc.cameraMode != GameManager.CameraMode.PadLock)
                            {
                                interactionText.text = "[" + playerInput.actions.FindAction("Interaction").bindings[0].path.Split('/')[1].ToUpper() + "] 를 눌러 자물쇠 확인";
                            }

                            else
                            {

                            }
                                break;
                    }


                }

                else
                {
                    interactionObject = null;
                    interactionText.text = "";

                }
                break;

            case GameManager.CameraMode.PadLock:

                Ray ray1 = pc.playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray1.origin, ray1.direction, 3f);
                bool isHit1 = hits.Length > 0;

                foreach(var i in hits)
                {
                    if(i.transform.gameObject.tag.Equals("PadLockGear"))
                    {
                        interactionText.text = "";

                        if (Input.GetMouseButtonDown(0))
                        {
                            i.transform.gameObject.GetComponent<GearController>().Turn();
                        }

                        if (interactionObject.GetComponent<PadLockController>().Lock == false)
                        {
                            pc.cameraMode = GameManager.CameraMode.FPS;
                            GameManager.CursorOff();
                        }
                    }

                }

                break;
        }


    }

    public void OnInteraction(InputValue value)
    {
        if(pc.cameraMode == GameManager.CameraMode.PadLock)
        {
            pc.cameraMode = GameManager.CameraMode.FPS;
            GameManager.CursorOff();
            return;
        }
        if (interactionObject == null) return;

        switch (interactionObject.layer)
        {
            case 3:
                inventoryManager.AddItemToInventory(interactionObject.GetComponent<ItemMananger>().item);
                Destroy(interactionObject);
                interactionObject = null;

                break;

            case 11:

                if (pc.cameraMode == GameManager.CameraMode.PadLock) return;


                pc.cameraMode = GameManager.CameraMode.PadLock;
                GameManager.CursorOn();

                break;
        }
    }
}
