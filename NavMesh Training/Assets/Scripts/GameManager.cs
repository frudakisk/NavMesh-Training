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

    public GameObject[] enemys; //0 is normal, 1 is boss so far
    public int enemyCount;
    public int bossSpawnNumber;
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
    public TextMeshProUGUI largeWaveNumberText;

    public int waveNumber;

    private GameObject floor;

    public float score; //accuracy, wave #, health per wave, # enemies killed, time
    private int time;
    private int totalEnemiesSpawned;
    private PlayerController player;
    private int playerHealthOverTime;

    private bool spawningInAction;
    private void Awake()
    {
        floor = GameObject.Find("Mesh Floor");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyCount = 0;
        bossSpawnNumber = 1;
        spawnNumber = 1;
        waveNumber = 0;
        isGameOver = false;
        isGameOverActive = false;
        score = 0;
        time = 0;
        totalEnemiesSpawned = 0;
        playerHealthOverTime = 0;
        spawningInAction = false;

        StartCoroutine(Timer());

    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        enemiesRemainingText.text = "Enemies: " + enemyCount;
        if(enemyCount == 0 && spawningInAction == false)
        {
            waveNumber++;
            waveNumberText.text = "Wave: " + waveNumber;
            playerHealthOverTime += player.health;
            StartCoroutine(SpawnEnemyRoutine());
        }

        accuracy = Accuracy(shotsThatHitEnemy, totalShots);
        //update some sort of UI element
        accuracyText.text = "Accuracy: " + accuracy.ToString("0.00") + "%";

        if(isGameOver && !isGameOverActive)
        {
            isGameOverActive = true;
            score = CalculateScore();
            //check if this score is worth putting in our leaderboard (top 10)
            Score playerScore = new Score(DataManager.Instance.username, score);
            CompareScoreToLeaderboard(playerScore);
            AddToCommunityKills();
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
        float distanceToEdge = floor.transform.localScale.x * 0.5f * floor.GetComponent<BoxCollider>().size.x - 1;
        return distanceToEdge;

    }


    /// <summary>
    /// Spawns enemies in safe random location
    /// </summary>
    /// <param name="num">number of enemies to spawn</param>
    private void SpawnEnemyWave(int num, GameObject enemyType)
    {
        for(int i = 0; i < num; i++)
        {
            Vector3 spawnPos = SpawnLocation();
            Instantiate(enemyType, spawnPos, Quaternion.identity);
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

    IEnumerator Timer()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(1.0f);
            time++;
        }
    }

    float CalculateScore()
    {
        int enemiesKilled = totalEnemiesSpawned - enemyCount;
        Debug.Log($"accuracy = {accuracy}\nwaveNumber = {waveNumber}\nenemiesKilled = {enemiesKilled}\nplayerHealthOverTime = {playerHealthOverTime}\ntime = {time}");
        //Score=(Accuracy×AccuracyWeight)+(WavesSurvived×WavesWeight)+(EnemiesKilled×KillsWeight)+(HealthAfterRound×HealthWeight)+(TimePassed×TimeWeight)
        float score = ((waveNumber - 1) * 100f * 0.5f) + (accuracy * 0.25f) +  + (enemiesKilled * 0.15f) + (playerHealthOverTime * 0.1f);
        Debug.Log("Score = " + score);
        if (score < 0) return 0f;
        return score;
    }


    private IEnumerator SpawnEnemyRoutine()
    {
        spawningInAction = true;
        //wait a couple of seconds before spawning enemies
        largeWaveNumberText.gameObject.SetActive(!largeWaveNumberText.gameObject.activeSelf);
        largeWaveNumberText.text = "Wave " + waveNumber;
        yield return new WaitForSeconds(3.0f);
        if (waveNumber % 5 == 0)
        {
            SpawnEnemyWave(bossSpawnNumber, enemys[1]);
            totalEnemiesSpawned += bossSpawnNumber;
            bossSpawnNumber++;
        }
        else
        {
            SpawnEnemyWave(spawnNumber, enemys[0]);
            totalEnemiesSpawned += spawnNumber;
            spawnNumber += 2;
        }
        largeWaveNumberText.gameObject.SetActive(!largeWaveNumberText.gameObject.activeSelf);
        spawningInAction = false;
    }

    /// <summary>
    /// Takes the amount of kills we did during out game and adds it to
    /// the total kills for the community
    /// </summary>
    void AddToCommunityKills()
    {
        int currentKills = totalEnemiesSpawned - enemyCount;
        DataManager.Instance.communityKills += currentKills;
    }

    /// <summary>
    /// Checks to see if the players score is worth putting into
    /// the leaderboard of top 10 scores
    /// </summary>
    /// <param name="playerScore">of type Score that contains
    /// the players name and the score they got after losing</param>
    void CompareScoreToLeaderboard(Score playerScore)
    {
        List<Score> leaderboard = DataManager.Instance.leaderboard;
        leaderboard.Add(playerScore);

        if(leaderboard.Count <= 10)
        {
            //if there is less than 10 scores in our leaderboard
            //just add in and sort
            leaderboard.Sort((x, y) => y.score.CompareTo(x.score));
        }
        else
        {
            //if there is 10 or more scores in our leaderboard, do this
            //maybe i can still add and sort but then remove last item?
            leaderboard.Sort((x, y) => y.score.CompareTo(x.score));
            leaderboard.RemoveAt(leaderboard.Count - 1);
        }

        
        //update the persistent data
        DataManager.Instance.leaderboard = leaderboard;
    }
}
