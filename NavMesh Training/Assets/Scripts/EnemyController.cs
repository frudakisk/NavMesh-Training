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
            agent.stoppingDistance = 5.0f;
            agent.SetDestination(player.position);
            isDestinationSet = false;
            if(distance <= agent.stoppingDistance)
            {
                //attack
                FaceTarget();
            }
        }
        else if (distance > lookRadius && isDestinationSet == false)
        {
            agent.stoppingDistance = 0.1f;
            WalkAround();
        }

        CheckAtWalkDestination();
    }

    void FaceTarget()
    {
        //get direction of target
        Vector3 direction = (player.position - transform.position).normalized;
        //get rotation that points to the target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //set the rotation of our enemy
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
            if (Vector3.Distance(transform.position, destination) < 0.1f)
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
