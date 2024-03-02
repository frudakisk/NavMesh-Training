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
        //no highscore set yet

        //no score algirithm set

        WaveText.text = "Waves Survived\n" + gameManager.waveNumber;
        accuracyText.text = "Accuracy\n" + gameManager.accuracy.ToString("0.00") + "%";

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
