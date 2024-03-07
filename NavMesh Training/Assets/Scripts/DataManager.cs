using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    //do like a leaderboard thing where its the name and highscore. 
    public string username;
    public string highscoreUsername;
    public float highscore;
    public double communityKills; //this is a community number so every kill here counts
    public List<Score> leaderboard;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //creating a fake leaderboard...later
        leaderboard = new List<Score>();
        leaderboard.Add(new Score("KAF", 120));
        leaderboard.Add(new Score("JMB", 230));
        leaderboard.Add(new Score("KAF", 4));
        leaderboard.Add(new Score("GNF", 53));
        leaderboard.Add(new Score("GNF", 95));
        leaderboard.Add(new Score("KAF", 111));
        leaderboard.Add(new Score("JMB", 507));
        leaderboard.Add(new Score("KAF", 123));
        leaderboard.Add(new Score("XXX", 72));
        DontDestroyOnLoad(gameObject);
    }


}
