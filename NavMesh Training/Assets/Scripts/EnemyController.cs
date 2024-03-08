using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityBehaviour
{
    //NavMesh variables
    private Vector3 destination;
    protected float lookRadius;
    protected bool isDestinationSet;

    //References
    protected NavMeshAgent agent;
    private Transform player;

    //bullet information
    public float shootTime;
    protected float sTime;

    //animations
    private bool deathAnimationActive;
    public ParticleSystem deathParticles;

    //audio
    public List<AudioClip> deathGrunts = new List<AudioClip>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>(); //only enemies make noises
        lookRadius = 20.0f;
        isDestinationSet = false;
        shootTime = 1.0f;
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
        else if (distance > lookRadius && isDestinationSet == false && agent.enabled == true)
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
        bool hitSuccess = NavMesh.SamplePosition(position, out _, 0.1f, NavMesh.AllAreas);
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
                if (!GameManager.isGameOver) Attack();
                FaceTarget();
            }
        }

    }

    /// <summary>
    /// Shoot a bullet at a timed interval determined by the class
    /// </summary>
    protected virtual void Attack()
    {
        sTime = sTime - Time.deltaTime;
        if (sTime < 0)
        {
            ShootBullet(gameObject, 1.7f, Vector3.up);
            sTime = shootTime;
        }
    }

    /// <summary>
    /// This is what happens when an enemy dies. We turn off its agent so that
    /// we can apply physics to it and give it a little push to the ground
    /// before we wait a couple of seconds and make an explosion animation happen.
    /// Then we destroy the object
    /// </summary>
    /// <returns>a coroutine</returns>
    protected IEnumerator DeathRoutine()
    {
        deathAnimationActive = true;
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(new Vector3(0, 5, -1), ForceMode.Impulse);
        AudioClip clip = deathGrunts[Random.Range(0, deathGrunts.Count)];
        audioSource.PlayOneShot(clip, 1.0f);
        yield return new WaitForSeconds(3.0f);
        ParticleSystem particles = Instantiate(deathParticles, transform.position + transform.up, Quaternion.identity);
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
