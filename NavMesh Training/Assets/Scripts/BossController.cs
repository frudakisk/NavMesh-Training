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

    /// <summary>
    /// Using a different version of the Attack() method for Bosses because
    /// I want a Boss to shoot faster
    /// </summary>
    protected override void Attack()
    {
        sTime = sTime - Time.deltaTime;
        if(sTime < 0)
        {
            ShootBullet(gameObject, 3f, new Vector3(0, 2, 0));
            sTime = shootTime;
        }
    }


}
