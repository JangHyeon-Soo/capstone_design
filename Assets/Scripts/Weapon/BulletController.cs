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

    void OnDestroy()
    {
        Debug.LogWarning($"[DESTROY TRACE] {gameObject.name} 이 삭제됨! (시간: {Time.time}) 스택트레이스: {Environment.StackTrace}");
    }

}
