using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityBehaviour : MonoBehaviour
{

    public GameObject bullet;
    public float bulletForwardForce = 15.0f;
    public float bulletUpwardForce = 3.0f;

    public int health;


    private Rigidbody rb;
    private bool wasForceApplied = false;

    private HealthBar healthBar;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        healthBar = GetComponent<HealthBar>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ///I probably can put these two statements in their respective class
        if(IsEntityDead())
        {
            if(gameObject.CompareTag("Player") && wasForceApplied == false)
            {
                //dont destroy the player but activate some sort of game
                //stopping mechanism and maybe some animation
                rb.freezeRotation = false;
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                wasForceApplied = true;
            }

            if(!gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);

            }
        }
    }

    /// <summary>
    /// Shoots a bullet prefab in the forward direction that the player is facing.
    /// </summary>
    /// <param name="entity">the game object that shot the bullet</param>
    public void ShootBullet(GameObject entity)
    {
        //spawn the bullet just a little below the camera
        Vector3 spawnPos = (entity.transform.position + Vector3.up) + entity.transform.forward * 1.7f;
        //make sure the bullet has same rotation as player
        GameObject currentBullet = Instantiate(bullet, spawnPos, entity.transform.rotation);
        //apply a forward and upward force for the bullet
        var currentBulletRb = currentBullet.GetComponent<Rigidbody>();
        currentBulletRb.AddForce(entity.transform.forward * bulletForwardForce, ForceMode.Impulse);
        currentBulletRb.AddForce(Vector3.up * bulletUpwardForce, ForceMode.Impulse);
        //let bullet know who shot the bullet
        currentBullet.GetComponent<BulletBehaviour>().entityThatShot = entity;
    }


    /// <summary>
    /// if a bullet hits an entity, their health goes down
    /// </summary>
    /// <param name="collision">colliding object</param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            health--;
            UpdateHealthBar(health);
        }
    }

    /// <summary>
    /// Checks if the entity has 0 health
    /// </summary>
    /// <returns>true if no health, false otherwise</returns>
    private bool IsEntityDead()
    {
        if(health == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the entities healthbar based on the amount given
    /// </summary>
    /// <param name="amount">remaining health</param>
    private void UpdateHealthBar(int amount)
    {
        healthBar.healthSlider.value = amount;
    }

}
