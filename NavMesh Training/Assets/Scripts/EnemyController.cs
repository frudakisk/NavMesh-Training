using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityBehaviour
{
    //NavMesh variables
    private Vector3 destination;
    protected float lookRadius = 20.0f;
    protected bool isDestinationSet = false;

    //References
    protected NavMeshAgent agent;
    private Transform player;

    public float shootTime = 1.0f;
    protected float sTime;

    private bool deathAnimationActive;
    public ParticleSystem deathParticles;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        health = 3;
        sTime = shootTime;
        deathAnimationActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEntityDead() && deathAnimationActive == false)
        {
            StartCoroutine(DeathRoutine());
        }

        //always know distance between player and enemy
        float distance = GetDistance();
        if(distance <= lookRadius)
        {
            Chase(distance);
        }
        else if (distance > lookRadius && isDestinationSet == false)
        {
            if(agent.enabled == true)
            {
                agent.stoppingDistance = 0.1f;
                WalkAround();
            }
            
        }

        CheckAtWalkDestination();
    }

    /// <summary>
    /// Have the enemy agent always face the player while the player is in the stoppingDistance
    /// of this object
    /// </summary>
    protected void FaceTarget()
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
    protected float GetDistance()
    {
        return Vector3.Distance(player.position, transform.position);
    }


    /// <summary>
    /// Search for the player when we are not chasing the player.
    /// Checks to see if the destination is within the NavMesh and
    /// is a walkable spot in the NavMesh.
    /// </summary>
    protected void WalkAround()
    {
        if(!isDestinationSet)
        {
            do
            {
                float randomX = Random.Range(-floorRange, floorRange);
                float randomZ = Random.Range(-floorRange, floorRange);
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
    protected bool IsPositionWalkable(Vector3 position)
    {
        NavMeshHit hit;
        bool hitSuccess = NavMesh.SamplePosition(position, out hit, 0.1f, NavMesh.AllAreas);
        return hitSuccess;
    }


    /// <summary>
    /// Checks if we are at our destination set by our WalkAround() method
    /// and has a small offset so we can deal with floating point inaccuracy
    /// </summary>
    protected void CheckAtWalkDestination()
    {
        if(isDestinationSet)
        {
            if (Vector3.Distance(transform.position, destination) < 0.2f)
            {
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
    protected void Chase(float distance)
    {
        
        if(agent.isActiveAndEnabled)
        {
            agent.stoppingDistance = 10.0f;
            agent.SetDestination(player.position);
            isDestinationSet = false;
            if (distance <= agent.stoppingDistance)
            {
                //attack
                if (!GameManager.isGameOver) Attack();
                FaceTarget();
            }
        }

    }

    protected virtual void Attack()
    {
        sTime = sTime - Time.deltaTime;
        if (sTime < 0)
        {
            ShootBullet(gameObject, 1.7f, Vector3.up);
            sTime = shootTime;
        }
    }

    protected IEnumerator DeathRoutine()
    {
        deathAnimationActive = true;
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(new Vector3(0, 5, -1), ForceMode.Impulse);
        yield return new WaitForSeconds(3.0f);
        ParticleSystem particles = Instantiate(deathParticles, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        particles.Play();
        Destroy(gameObject);
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
