using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject entityThatShot;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBulletHeight();

        if (entityThatShot == null)
        {
            //I wish i could make bullet dissapear a little after
            //the entity dies but it brings up an error
            //MAYBE ADD EFFECT TO EXPLODE BULLET WHEN THIS HAPPENS
            Debug.Log("Entity that shot was destroyed");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if bullet coming from player hits an enemy, add tally to accuracy
        //if bullet coming from enemy hits the player, do not do anything about it
        //if bullet coming from player hits anything else but the enemy, subtract tally to accuracy
        if(entityThatShot.CompareTag("Player") &&
           collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("PLAYER hit an enemy");
            //add to accuracy
            gameManager.shotsThatHitEnemy++;
            //gameManager.totalShots++;
        }
        else if (entityThatShot.CompareTag("Player"))
        {
            //THIS BLOCK MIGHT BE ABLE TO BE DELETED
            //this should only be active for the player not enemy bullets...
            Debug.Log("Bullet from PLAYER did not hit anything");
            //only look for bullets  that player shot, not enemy
            //gameManager.totalShots++;
        }

        gameManager.totalShots++;
        Destroy(gameObject);

    }

    private void CheckBulletHeight()
    {
        if(transform.position.y <= -1.0f)
        {
            Destroy(gameObject);
        }
    }
}
