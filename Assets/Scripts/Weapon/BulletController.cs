using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    
    public float bulletSpeed;
    public PoolManager.PoolingObjectType poolingObjectType;

    float timer = 0;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;
            transform.position += transform.forward * Time.deltaTime * bulletSpeed;

            if(timer > 5)
            {
                timer = 0;
                gameObject.SetActive(false);
            }
        }
    }



}
