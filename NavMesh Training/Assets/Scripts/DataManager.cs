using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    //do like a leaderboard thing where its the name and highscore. 
    public string username;
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
        DontDestroyOnLoad(gameObject);
        LoadData();

        if(leaderboard == null)
        {
            leaderboard = new List<Score>();
        }
    }


    public void SaveData()
    {
        GameData data = new GameData();
        data.communityKills = communityKills;
        data.leaderboard = leaderboard;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
            communityKills = data.communityKills;
            leaderboard = data.leaderboard;
        }
    }

    [System.Serializable]
    public class GameData
    {
        public double communityKills;
        public List<Score> leaderboard;
    }

}
