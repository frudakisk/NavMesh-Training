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
    private float accuracy;


    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI accuracyText;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        spawnNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        enemiesRemainingText.text = "Enemies: " + enemyCount;
        if(enemyCount == 0)
        {
            SpawnEnemyWave(spawnNumber);
            waveNumberText.text = "Wave: " + spawnNumber;
            spawnNumber++;
        }

        accuracy = Accuracy(shotsThatHitEnemy, totalShots);
        //update some sort of UI element
        accuracyText.text = "Accuracy: " + accuracy.ToString("0.00") + "%";
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

    private void SpawnEnemyWave(int num)
    {
        for(int i = 0; i < num; i++)
        {
            Vector3 spawnPos = SpawnLocation();
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }

    private Vector3 SpawnLocation()
    {
        Vector3 spawnPos;
        do
        {
            float range = NavMeshSurfaceRange();
            float xPos = Random.Range(-range, range);
            float zPos = Random.Range(-range, range);
            spawnPos = new Vector3(xPos, 0, zPos);
        } while (!IsPositionSpawnable(spawnPos));
        return spawnPos;
        
        
    }

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

    public float Accuracy(int shotsHit, int totalShots)
    {
        float accuracy = (float)shotsHit / totalShots * 100;
        Debug.Log($"Accuracy = {accuracy}");
        if(float.IsNaN(accuracy))
        {
            return 0f;
        }
        return accuracy;
    }
}
