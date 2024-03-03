using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float mouseX;

    public float speed = 5.0f;
    public float mouseSpeed = 30.0f;

    private bool wasForceApplied = false;
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
            if(wasForceApplied == false)
            {
                rb.freezeRotation = false;
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                wasForceApplied = true;
                GameManager.isGameOver = true;
            }
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
                ShootBullet(gameObject, 1.7f);
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



}
