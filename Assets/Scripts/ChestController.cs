using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Transform ChestDoor;

    public bool Lock;
    public bool isOpen;

    Quaternion openRot, closeRot;
    float timer = 0;
    private void Start()
    {
        closeRot = ChestDoor.localRotation;
        openRot = closeRot * Quaternion.Euler(0, 0, -90);

    }

    private void Update()
    {
        if(!Lock)
        {
            if(isOpen)
            {
                ChestDoor.localRotation = Quaternion.Slerp(ChestDoor.localRotation, openRot, Time.deltaTime * 5f);
            }

            else
            {
                ChestDoor.localRotation = Quaternion.Slerp(ChestDoor.localRotation, closeRot, Time.deltaTime * 5f);
            }

        }
    }
    public void DoorOpen()
    {
        if(!Lock) isOpen = !isOpen;
    }

    public void LockUnlock()
    {
        Lock = !Lock;
    }


}
