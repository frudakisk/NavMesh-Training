using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject bullet;
    public float bulletForwardForce = 15.0f;
    public float bulletUpwardForce = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Shoots a bullet prefab in the forward direction that the player is facing.
    /// </summary>
    public void ShootBullet(GameObject entity)
    {
        Vector3 spawnPos =  entity.transform.position + entity.transform.forward * 1.7f;
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

    /// <summary>
    /// The surface must be a perfect square for this method. Finds the distance from
    /// the center of the square to an edge of a square
    /// </summary>
    /// <returns>float distance from center of square to edge of it</returns>
    public float NavMeshSurfaceRange()
    {
        GameObject floor = PlayerManager.Instance.floor;
        float distanceToEdge = floor.transform.localScale.x * 0.5f * floor.GetComponent<BoxCollider>().size.x - 1;
        return distanceToEdge;

    }
}
