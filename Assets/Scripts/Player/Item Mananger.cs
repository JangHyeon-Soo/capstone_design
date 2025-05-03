using UnityEngine;


public class ItemMananger : MonoBehaviour
{
    
    public GameManager.Item item;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.layer != 3) gameObject.layer = 3;

        switch (item.ItemName)
        {
            case "Ammo_9mm":

                item.ItemQuantity = Random.Range(3, 10);

                break;

            case "Ammo_556":
                item.ItemQuantity = Random.Range(12, 20);
                break;
        }

    }

    public void SetTag(string Tag)
    {
        gameObject.tag = Tag;
    }
}
