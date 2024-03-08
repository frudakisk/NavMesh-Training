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
            //MAYBE ADD EFFECT TO EXPLODE BULLET WHEN THIS HAPPENS
            Debug.Log("Entity that shot was destroyed");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks for the different kinds of collisions that a bullet can make
    /// and takes into account who shot the bullet as well. This helps
    /// create the accuracy percentage
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(entityThatShot != null)
        {
            //if bullet coming from player & hits an enemy, add tally
            if (entityThatShot.CompareTag("Player") &&
               collision.gameObject.CompareTag("Enemy"))
            {
                gameManager.shotsThatHitEnemy++;
                gameManager.totalShots++;
            }
            else if (entityThatShot.CompareTag("Player"))
            {
                //if the player shoots and misses the enemy and
                //bullet collides with something else
                gameManager.totalShots++;
            }
            else if (entityThatShot.CompareTag("Enemy") &&
                collision.gameObject.CompareTag("Enemy"))
            {
                //enemy hit enemy, do not take health away
                EntityBehaviour objectHit = collision.gameObject.GetComponent<EntityBehaviour>();
                objectHit.health++;
                objectHit.UpdateHealthBar(objectHit.health);

            }
        }
        Destroy(gameObject);
    }


    /// <summary>
    /// Check the y position of a bullet and destroys it if too low.
    /// Also added to total shots if the shot game from the player only
    /// </summary>
    private void CheckBulletHeight()
    {
        if(transform.position.y <= -1.0f)
        {
            Destroy(gameObject);
            if(entityThatShot.CompareTag("Player"))
            {
                //if a bullet from the player falls off the map, add to accuracy calculations
                gameManager.totalShots++;
            }
        }
    }
}
