using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityBehaviour
{
    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;
    private float mouseX;

    public float speed = 5.0f;
    public float mouseSpeed = 30.0f;


    private float floorRange;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        floorRange = gameManager.NavMeshSurfaceRange();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");

        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * mouseSpeed);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet(gameObject);
        }

        CheckBoundary();
    }

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
