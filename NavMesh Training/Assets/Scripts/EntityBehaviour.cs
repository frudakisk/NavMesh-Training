using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{

    public GameObject bullet;
    public float bulletForwardForce = 15.0f;
    public float bulletUpwardForce = 3.0f;

    public int health;

    private Rigidbody rb;
    private bool wasForceApplied = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
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
    public void ShootBullet(GameObject entity)
    {
        Vector3 spawnPos = (entity.transform.position + Vector3.up) + entity.transform.forward * 1.7f;
        Debug.Log($"Spawn Pos: {spawnPos}");
        //spawn the bullet just a little below the camera
        //amke sure the bullet has same rotation as player
        GameObject currentBullet = Instantiate(bullet, spawnPos, entity.transform.rotation);
        //apply a forward and upward force for the bullet
        var currentBulletRb = currentBullet.GetComponent<Rigidbody>();
        currentBulletRb.AddForce(entity.transform.forward * bulletForwardForce, ForceMode.Impulse);
        currentBulletRb.AddForce(Vector3.up * bulletUpwardForce, ForceMode.Impulse);
        Debug.Log("Bullet was shot");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            health--;
        }
    }

    private bool IsEntityDead()
    {
        if(health == 0)
        {
            return true;
        }
        return false;
    }
}
