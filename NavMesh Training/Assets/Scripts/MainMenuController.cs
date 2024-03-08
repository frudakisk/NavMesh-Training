using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public GameObject leaderboardPanel;
    public TMP_InputField nameField;
    public Button playButton;

    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI communityKillsText;

    public TextMeshProUGUI leaderboardText;

    // Start is called before the first frame update
    void Start()
    {
        playButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayButtonInteraction();

        //assign stat values
        if (DataManager.Instance.leaderboard.Count > 0)
        {
            Score topHighscore = DataManager.Instance.leaderboard[0];
            highscoreText.text = "Current Highscore\n" + topHighscore.username +
                " " + topHighscore.score;
        }
        else
        {
            highscoreText.text = "Current Highscore\nnone";
        }
        
        communityKillsText.text = "Total Enemies Killed\n" + FormatCommunityKills();
    }

    /// <summary>
    /// Sets the play button active if the input field text has 3 characters
    /// and sets it to false if it doesnt
    /// </summary>
    private void PlayButtonInteraction()
    {
        if (nameField.text.Length == 3)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    /// <summary>
    /// toggles the active setting of the how to playe panel
    /// </summary>
    public void ToggleHowToPlayPanel()
    {
        howToPlayPanel.SetActive(!howToPlayPanel.activeSelf);
    }

    /// <summary>
    /// toggles the active setting of the leaderboard panel and creates
    /// the leaderboard if the panel is set to active
    /// </summary>
    public void ToggleLeaderboardPanel()
    {
        leaderboardPanel.SetActive(!leaderboardPanel.activeSelf);

        if (leaderboardPanel.activeSelf == true)
        {
            //create the leaderboard text
            CreateLeaderboardText();
        }
    }

    /// <summary>
    /// starts the game by moving to the next scene and saving the players
    /// username
    /// </summary>
    public void StartGame()
    {
        DataManager.Instance.username = nameField.text;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quits the application based on the platform it is running on
    /// </summary>
    public void QuitGame()
    {
        DataManager.Instance.SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Formats the community kills so the number does overflow
    /// using K M B T as suffixes
    /// </summary>
    /// <returns>a string representation of large numbers</returns>
    string FormatCommunityKills()
    {
        string[] suffixes = { "", "K", "M", "B", "T" };
        double communityKills = DataManager.Instance.communityKills;
        if(communityKills > 0)
        {
            //find magnitude
            int mag = (int)Math.Floor(Math.Log10(Math.Abs(communityKills)) / 3);
            //calculate the abbreviated value
            double abbreviatedValue = communityKills / Math.Pow(10, mag * 3);
            //Format the number with a specific number of decimal places
            string formattedNumber = abbreviatedValue.ToString("F2");
            //Append the appropriate suffix
            string suffix = suffixes[mag];
            return formattedNumber + suffix;
        }
        return "0";
        
    }

    /// <summary>
    /// Creates the text that goes into the leaderboard panel by reading from
    /// the saved leaderboard
    /// </summary>
    void CreateLeaderboardText()
    {
        List<Score> leaderboard = DataManager.Instance.leaderboard;
        int position = 1;
        string boardString = "";
        foreach(Score score in leaderboard)
        {
            boardString += $"{position}. {score.username} {score.score}\n";
            position++;
        }
        leaderboardText.text = boardString;
    }

}
