using UnityEngine;

public class FlyingMonsterController : MonoBehaviour
{
    public enum MovementMode
    {
        Default,
        PlayerChase
    }


    public float reconRange;
    public Vector3 Destination;
    public MovementMode movementMode;
    public LayerMask ObstacleLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destination = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (Destination - transform.position).normalized;
        float range = Vector3.Distance(Destination, transform.position);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit,range))
        {
            Vector3 avoidanceDir = Vector3.Cross(direction, Vector3.up); // 옆으로 피하기
            direction += avoidanceDir;
        }


        if(range > 0.1f)
        {
            transform.position += direction.normalized * 3 * Time.deltaTime;
        }



    }

    public Vector3 GetRandomPosition()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * reconRange;
       
        if (randomPos.y <= 0) 
        {
            GetRandomPosition();
        }

        else
        {
            Vector3 direction = (randomPos - transform.position).normalized;
            float range = Vector3.Distance(randomPos, transform.position);
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range, ObstacleLayer))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Environments"))
                {
                    return hit.point;
                }
            }
        }

        return randomPos;


    }
}
