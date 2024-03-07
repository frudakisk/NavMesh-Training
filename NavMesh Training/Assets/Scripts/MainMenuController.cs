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
    public TMP_InputField nameField;
    public Button playButton;

    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI communityKillsText;

    // Start is called before the first frame update
    void Start()
    {
        playButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(nameField.text.Length == 3)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }

        //assign stat values
        highscoreText.text = "Current Highscore\n" + DataManager.Instance.highscoreUsername +
            " " + DataManager.Instance.highscore;
        communityKillsText.text = "Total Enemies Killed\n" + FormatCommunityKills();
    }

    public void ToggleHowToPlayPanel()
    {
        howToPlayPanel.SetActive(!howToPlayPanel.activeSelf);
    }

    public void StartGame()
    {
        DataManager.Instance.username = nameField.text;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
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

}
