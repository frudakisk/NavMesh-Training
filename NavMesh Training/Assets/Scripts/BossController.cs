using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lookRadius = 45.0f;
        shootTime = 0.2f;
        sTime = shootTime;
        health = 20;
    }

    private void Update()
    {
        Debug.Log($"sTime in boss is {sTime}");
        if (IsEntityDead())
        {
            Destroy(gameObject);
        }

        //always know distance between player and enemy
        float distance = GetDistance();
        if (distance <= lookRadius)
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

    protected override void Attack()
    {
        sTime = sTime - Time.deltaTime;
        if(sTime < 0)
        {
            Debug.Log("Boss Shot a Bullet");
            ShootBullet(gameObject, 2.5f, new Vector3(0, 2, 0));
            sTime = shootTime;
        }
    }


}
