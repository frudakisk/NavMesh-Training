using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float mouseX;

    private float speed = 5.0f;
    private float mouseSpeed = 100.0f;

    private bool wasForceApplied = false;

    public AudioClip hitNoise;
    public GameObject damagePanel;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 10;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsEntityDead())
        {
            PlayerDead();
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");

        if (!GameManager.isGameOver)
        {
            transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
            transform.Rotate(Vector3.up * mouseX * Time.deltaTime * mouseSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootBullet(gameObject, 1.7f, Vector3.up);
            }
        }
        
        CheckBoundary();
    }

    /// <summary>
    /// Checks if the player is out of bounds. Returns them to last safest spot
    /// </summary>
    private void CheckBoundary()
    {

        if (transform.position.x > floorRange)
        {
            transform.position = new Vector3(floorRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -floorRange)
        {
            transform.position = new Vector3(-floorRange, transform.position.y, transform.position.z);
        }

        if (transform.position.z > floorRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, floorRange);
        }

        if (transform.position.z < -floorRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -floorRange);
        }
    }

    /// <summary>
    /// overriding the collision from EntityBehaviour because we need to add
    /// more than just reducing health. We play a visual damage so the player
    /// knows they got hit via visual aspects
    /// </summary>
    /// <param name="collision">the object that collided with this object</param>
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if(collision.gameObject.CompareTag("Bullet"))
        {
            audioSource.PlayOneShot(hitNoise, 1.0f);
            StartCoroutine(DamagePanelRoutine());
        }
    }

    /// <summary>
    /// Plays a quick red screen that flashes over the entire screen
    /// </summary>
    /// <returns>a coroutine</returns>
    private IEnumerator DamagePanelRoutine()
    {
        for(int i = 0; i < 5; i++)
        {
            damagePanel.SetActive(!damagePanel.activeSelf);
            yield return new WaitForSeconds(0.01f);
            damagePanel.SetActive(!damagePanel.activeSelf);
            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// The process that happens when the players health is 0. We apply a force
    /// to the player and let the body have a ragdoll effect and then let the
    /// game manager know the game is over
    /// </summary>
    private void PlayerDead()
    {
        if (wasForceApplied == false)
        {
            rb.freezeRotation = false;
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            wasForceApplied = true;
            GameManager.isGameOver = true;
        }
    }
}
