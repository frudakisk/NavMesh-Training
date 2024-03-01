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
        //if bullet coming from player hits an enemy, add tally
        if(entityThatShot.CompareTag("Player") &&
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
                gameManager.totalShots++;
            }
        }
    }
}
