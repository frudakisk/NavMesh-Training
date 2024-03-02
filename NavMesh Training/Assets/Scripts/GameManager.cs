using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject bullet;
    public float bulletForwardForce = 15.0f;
    public float bulletUpwardForce = 3.0f;

    public GameObject enemy;
    public int enemyCount;
    private int spawnNumber;

    public int shotsThatHitEnemy;
    public int totalShots;
    public float accuracy; //use get and set stuff

    public static bool isGameOver;
    private bool isGameOverActive;
    public GameObject gameOverPanel;


    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI accuracyText;

    public int waveNumber;

    private GameObject floor;

    private void Awake()
    {
        floor = GameObject.Find("Mesh Floor");
    }

    // Start is called before the first frame update
    void Start()
    {
        
        enemyCount = 0;
        spawnNumber = 1;
        waveNumber = 0;
        isGameOver = false;
        isGameOverActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        enemiesRemainingText.text = "Enemies: " + enemyCount;
        if(enemyCount == 0)
        {
            SpawnEnemyWave(spawnNumber);
            waveNumber++;
            waveNumberText.text = "Wave: " + waveNumber;
            spawnNumber += 2;
        }

        accuracy = Accuracy(shotsThatHitEnemy, totalShots);
        //update some sort of UI element
        accuracyText.text = "Accuracy: " + accuracy.ToString("0.00") + "%";

        if(isGameOver && !isGameOverActive)
        {
            isGameOverActive = true;
            StartCoroutine(GameOverRoutine());
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// The surface must be a perfect square for this method. Finds the distance from
    /// the center of the square to an edge of a square
    /// </summary>
    /// <returns>float distance from center of square to edge of it</returns>
    public float NavMeshSurfaceRange()
    {
        if(floor == null)
        {
            Debug.Log("Floor reference is missing");
        }
        else
        {
            Debug.Log("We have floor reference");
        }
        float distanceToEdge = floor.transform.localScale.x * 0.5f * floor.GetComponent<BoxCollider>().size.x - 1;
        return distanceToEdge;

    }


    /// <summary>
    /// Spawns enemies in safe random location
    /// </summary>
    /// <param name="num">number of enemies to spawn</param>
    private void SpawnEnemyWave(int num)
    {
        for(int i = 0; i < num; i++)
        {
            Vector3 spawnPos = SpawnLocation();
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }


    /// <summary>
    /// Spawns an entity in a safe location that is on the nav mesh and not obstructed
    /// by obstacles at first
    /// </summary>
    /// <returns></returns>
    private Vector3 SpawnLocation()
    {
        Vector3 spawnPos;
        float range = NavMeshSurfaceRange();
        do
        {  
            float xPos = Random.Range(-range, range);
            float zPos = Random.Range(-range, range);
            spawnPos = new Vector3(xPos, 0, zPos);
        } while (!IsPositionSpawnable(spawnPos));
        return spawnPos;
        
        
    }


    /// <summary>
    /// Checks if a position is spawnable (not on top of an obstacle, in nav mesh range)
    /// </summary>
    /// <param name="position">the spawn position in question</param>
    /// <returns>true if position is safe, false otherwise</returns>
    private bool IsPositionSpawnable(Vector3 position)
    {
        NavMeshHit hit;
        bool hitSuccess = NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);
        if(!hitSuccess)
        {
            Debug.Log("Spawn location not spawnable");
        }
        else
        {
            Debug.Log("Spawn position available");
        }
        return hitSuccess;
    }


    /// <summary>
    /// Determines the accuracy of the players shots
    /// </summary>
    /// <param name="shotsHit">shot that hit an enemy</param>
    /// <param name="totalShots">all shots the player took</param>
    /// <returns>a float the represents the accuracy of the players aim</returns>
    public float Accuracy(int shotsHit, int totalShots)
    {
        float accuracy = (float)shotsHit / totalShots * 100;
        if(float.IsNaN(accuracy))
        {
            return 0f;
        }
        return accuracy;
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        gameOverPanel.SetActive(true);
    }
}
