using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    //do like a leaderboard thing where its the name and highscore. 
    public string username;
    public string highscoreUsername; //dont really need anymore
    public float highscore; //dont really need anymore
    public double communityKills; //this is a community number so every kill here counts
    public List<Score> leaderboard = new List<Score>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
