using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Vector3 destination;
    private float lookRadius = 7.0f;

    public float walkRange = 13.0f;

    private NavMeshAgent agent;
    private Transform player;

    private bool isDestinationSet = false;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = GetDistance();
        if(distance <= lookRadius)
        {
            agent.SetDestination(player.position);
            isDestinationSet = false;
        }
        else if (distance > lookRadius && isDestinationSet == false)
        {
            WalkAround();
        }

        CheckAtWalkDestination();
    }

    float GetDistance()
    {
        return Vector3.Distance(player.position, transform.position);
    }

    void WalkAround()
    {
        if(!isDestinationSet)
        {
            float randomX = Random.Range(-walkRange, walkRange);
            float randomZ = Random.Range(-walkRange, walkRange);
            destination = new Vector3(randomX, 0, randomZ);
            agent.SetDestination(destination);
            isDestinationSet = true;
        }
    }

    void CheckAtWalkDestination()
    {
        if(isDestinationSet)
        {
            if (Vector3.Distance(transform.position, destination) < 2.1f)
            {
                Debug.Log("We Have reached our destination");
                isDestinationSet = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
