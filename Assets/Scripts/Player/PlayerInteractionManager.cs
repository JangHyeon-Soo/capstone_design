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

        // ��ȣ�ۿ� ��ǲ
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
                GetComponent<UserInterfaceLogic>().PadLockUI.text = "";
                #region ȭ���߽� ����ĳ��Ʈ
                Ray ray = playerCamera.ScreenPointToRay(center);
                ray.direction = playerCamera.transform.forward;
                RaycastHit[] hit = Physics.RaycastAll(ray.origin, playerCamera.transform.forward, rayDistance, interactionLayer);


                bool isHit = hit.Length > 0;
                Debug.DrawRay(ray.origin, playerCamera.transform.forward * rayDistance, isHit ? Color.red : Color.white); 
                #endregion

                if (isHit)
                {
                    #region InteractionObject ����
                    if (hit.Length == 1)
                    {
                        interactionObject = hit[0].transform.gameObject;
                    }

                    else
                    {
                        foreach (var item in hit)
                        {
                            if ((item.transform.GetComponent<ChestController>() != null && !item.transform.GetComponent<ChestController>().isOpen)
                                || (item.transform.GetComponentInParent<ChestController>() != null && !item.transform.GetComponentInParent<ChestController>().isOpen))
                            {

                                interactionObject = item.transform.gameObject;
                                break;
                            }

                            else if (item.transform.GetComponent<PadLockController>())
                            {
                                interactionObject = item.transform.gameObject;
                                break;
                            }

                            else if (item.transform.GetComponent<ItemMananger>())
                            {
                                interactionObject = item.transform.gameObject;
                                break;
                            }
                        }
                    } 
                    #endregion
                    #region InteractionMessage
                    if(interactionObject == null) return;
                    switch (interactionObject.layer)
                    {
                        case 3:
                            interactionText.text = "[" + playerInput.actions.FindAction("Interaction").bindings[0].path.Split('/')[1].ToUpper() + "] �� ���� ������ ȹ��";
                            break;

                        case 11:
                            if (pc.cameraMode != GameManager.CameraMode.PadLock)
                            {
                                interactionText.text = "[" + playerInput.actions.FindAction("Interaction").bindings[0].path.Split('/')[1].ToUpper() + "] �� ���� �ڹ��� Ȯ��";
                            }

                            else
                            {
                                interactionText.text = "";
                            }
                            break;

                        case 12:
                            if (interactionObject.GetComponent<ChestController>() != null && interactionObject.GetComponent<ChestController>().Lock)
                            {

                                interactionText.text = "�������";
                            }

                            else if(interactionObject.GetComponentInParent<ChestController>() != null && interactionObject.GetComponentInParent<ChestController>().Lock)
                            {
                                interactionText.text = "�������";
                            }
                            else
                            {
                                interactionText.text = "[" + playerInput.actions.FindAction("Interaction").bindings[0].path.Split('/')[1].ToUpper() + "] �� ���� �ڽ� ����/�ݱ�";
                            }

                            break;

                    } 
                    #endregion
                }

                else
                {
                    if(pc.cameraMode == GameManager.CameraMode.FPS)
                    {
                        interactionObject = null;
                        interactionText.text = "";
                    }

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

            case 12:

                if(interactionObject.GetComponent<ChestController>() != null)
                {
                    interactionObject.GetComponent<ChestController>().DoorOpen();
                }

                else if (interactionObject.GetComponentInParent<ChestController>() != null )
                {
                    interactionObject.GetComponentInParent<ChestController>().DoorOpen();
                }

                break;
        }
    }
}
