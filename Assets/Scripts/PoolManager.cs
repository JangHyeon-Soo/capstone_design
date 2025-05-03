using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{

    public static PoolManager instance;

    public enum PoolingObjectType
    {
        Bullet_556,
        Bullet_9mm,
        Shell_556,
        Shell_9mm
    }
    [System.Serializable]
    public class PoolClass
    {
        PoolingObjectType poolingObjectType;
        List<GameObject> poolObjectList;
    }


    GameObject managedObjects;

    [Header("BulletPool")]
    public Dictionary<PoolingObjectType,Queue<GameObject>> Pool;

    private void Start()
    {
        instance = this;

        Pool = new Dictionary<PoolingObjectType, Queue<GameObject>>();
        managedObjects = new GameObject();

    }

    public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation, PoolingObjectType poolingObjectType)
    {
        if(Pool.ContainsKey(poolingObjectType))
        {
            if(Pool[poolingObjectType].Count < 30)
            {
                GameObject createdObject = Instantiate(obj, position, rotation, managedObjects.transform);
                Pool[poolingObjectType].Enqueue(createdObject);

                return createdObject;
            }

            else
            {
                GameObject lastObject = Pool[poolingObjectType].Dequeue();
                lastObject.SetActive(true);

                lastObject.transform.position = position;
                lastObject.transform.rotation = rotation;
                Pool[poolingObjectType].Enqueue(lastObject);

                return lastObject;

            }
        }

        else
        {
            CreatePool(poolingObjectType, 30);

            GameObject createdObject = Instantiate(obj, position, rotation, managedObjects.transform);
            Pool[poolingObjectType].Enqueue(createdObject);
            Debug.Log(Pool[poolingObjectType].Count);

            return createdObject;
        }
    }


    public void CreatePool(PoolingObjectType poolingObjectType, int initialSize)
    {
        Queue<GameObject> queue = new Queue<GameObject>();

        Pool.Add(poolingObjectType, queue);

    }
}
