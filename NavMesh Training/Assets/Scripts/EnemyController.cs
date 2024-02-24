using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    bool needSetDestination;
    Vector3 destination;

    public float walkRange = 13.0f;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        needSetDestination = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(needSetDestination)
        {
            WalkAround();
            needSetDestination = false;
        }
        //Debug.Log(transform.position + "-" + destination);
        //dont make this so presice
        if(Vector3.Distance(transform.position, destination) < 0.1f)
        {
            Debug.Log("We Have reached our destination");
            needSetDestination = true;
        }
    }

    void WalkAround()
    {
        float randomX = Random.Range(-walkRange, walkRange);
        float randomZ = Random.Range(-walkRange, walkRange);
        destination = new Vector3(randomX, 0, randomZ);
        agent.SetDestination(destination);

    }
}
