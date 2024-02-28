using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityBehaviour
{
    //NavMesh variables
    private Vector3 destination;
    private float lookRadius = 7.0f;
    private float walkRange;
    private bool isDestinationSet = false;

    //References
    private NavMeshAgent agent;
    private Transform player;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        walkRange = gameManager.NavMeshSurfaceRange();
    }

    // Update is called once per frame
    void Update()
    {
        //always know distance between player and enemy
        float distance = GetDistance();
        if(distance <= lookRadius)
        {
            Chase(distance);
        }
        else if (distance > lookRadius && isDestinationSet == false)
        {
            agent.stoppingDistance = 0.1f;
            WalkAround();
        }

        CheckAtWalkDestination();
    }

    /// <summary>
    /// Have the enemy agent always face the player while the player is in the stoppingDistance
    /// of this object
    /// </summary>
    void FaceTarget()
    {
        //get direction of target
        Vector3 direction = (player.position - transform.position).normalized;
        //get rotation that points to the target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //set the rotation of our enemy
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    /// <summary>
    /// Return the distance between the player and this object
    /// </summary>
    /// <returns>a float that represents the distance betweeb
    /// player and enemy</returns>
    float GetDistance()
    {
        return Vector3.Distance(player.position, transform.position);
    }


    /// <summary>
    /// Search for the player when we are not chasing the player.
    /// Checks to see if the destination is within the NavMesh and
    /// is a walkable spot in the NavMesh.
    /// </summary>
    void WalkAround()
    {
        if(!isDestinationSet)
        {
            do
            {
                float randomX = Random.Range(-walkRange, walkRange);
                float randomZ = Random.Range(-walkRange, walkRange);
                destination = new Vector3(randomX, 0, randomZ);
            } while (!IsPositionWalkable(destination));
            //could also just set a higher obstacle avoidance radius in the agent
            //but where's the fun in that!
            agent.SetDestination(destination);
            isDestinationSet = true;
        }
    }


    /// <summary>
    /// Checks to see if a position is a walkable position within the NavMesh
    /// </summary>
    /// <param name="position">a Vector3 position of desired destination</param>
    /// <returns>True if position is a walkable spot, false otherwise</returns>
    private bool IsPositionWalkable(Vector3 position)
    {
        NavMeshHit hit;
        bool hitSuccess = NavMesh.SamplePosition(position, out hit, 0.1f, NavMesh.AllAreas);
        Debug.Log($"{position} is a walkable position");
        return hitSuccess;
    }


    /// <summary>
    /// Checks if we are at our destination set by our WalkAround() method
    /// and has a small offset so we can deal with floating point inaccuracy
    /// </summary>
    void CheckAtWalkDestination()
    {
        if(isDestinationSet)
        {
            if (Vector3.Distance(transform.position, destination) < 0.2f)
            {
                Debug.Log("We Have reached our destination");
                isDestinationSet = false;
            }
        }
    }


    /// <summary>
    /// When called, we change the direction of the enemy to go towards the
    /// player until we are told not to. We also attack the player when the player
    /// is within our stoppingDistance.
    /// </summary>
    /// <param name="distance">distance between the player and the enemy</param>
    void Chase(float distance)
    {
        agent.stoppingDistance = 5.0f;
        agent.SetDestination(player.position);
        isDestinationSet = false;
        if (distance <= agent.stoppingDistance)
        {
            //attack
            FaceTarget();
        }
    }


    /// <summary>
    /// Draws a sphere around this object to show the lookRadius in the editor
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
