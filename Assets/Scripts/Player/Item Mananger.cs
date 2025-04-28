using UnityEngine;


public class ItemMananger : MonoBehaviour
{
    
    public GameManager.Item item;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.layer != 3) gameObject.layer = 3;
    }

    public void SetTag(string Tag)
    {
        gameObject.tag = Tag;
    }
}
