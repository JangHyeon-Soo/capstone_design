using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    #region Enum And Class
    public enum CameraMode
    {
        FPS,
        TPS,
        Bodycam
    }
    [System.Serializable]
    public enum armState
    {
        Unarmed,
        Pistol,
        Rifle
    }

    [System.Serializable]
    public class Item
    {
        public int Code;
        public GameManager.ItemType itemType;
        public string ItemName;
        public Sprite ItemImage;
        public string ItemDescription;
        public int ItemQuantity;

    }

    public enum ItemType
    {
        Item,
        Weapon,
        Note
    }

    #endregion


    void Start()
    {
        instance = this;
    }

    public static void CursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public static void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                //Debug.Log(child.name);
                return child;
            }
                

            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }
}
