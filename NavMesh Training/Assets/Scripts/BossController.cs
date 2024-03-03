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
        sTime = 0.2f;
        health = 20;
    }

    protected override void Attack()
    {
        ShootBullet(gameObject, 4.0f);
    }
}
