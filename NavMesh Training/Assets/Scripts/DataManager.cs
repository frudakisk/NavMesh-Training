using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    //do like a leaderboard thing where its the name and highscore. 
    public string username;
    public float highscore;
    public double communityKills; //this is a community number so every kill here counts

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //communityKills = 12312312; //testing
    }
}
