using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI WaveText;
    public TextMeshProUGUI accuracyText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DataManager.Instance.leaderboard.Count > 0)
        {
            Score topHighscore = DataManager.Instance.leaderboard[0];
            highscoreText.text = "Highscore\n" + topHighscore.username + " " + topHighscore.score;
        }
        else
        {
            highscoreText.text = "Highscore\nnone";
        }
        
        
        scoreText.text = "Score\n" + gameManager.score.ToString("0.00");
        WaveText.text = "Waves Survived\n" + (gameManager.waveNumber - 1);
        accuracyText.text = "Accuracy\n" + gameManager.accuracy.ToString("0.00") + "%";

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
