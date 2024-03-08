using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
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

    /// <summary>
    /// Saves some data that we want to be persistent through sessions
    /// </summary>
    public void SaveData()
    {
        GameData data = new GameData();
        data.communityKills = communityKills;
        data.leaderboard = leaderboard;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    /// <summary>
    /// Loads in some data that we saved from previous sessions.
    /// Checks if filepath exists
    /// </summary>
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

    /// <summary>
    /// For cleaning purposes. Removes the file that contains the saved
    /// data incase we want a fresh paper.
    /// </summary>
    public void RemoveData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.Log("There is no file to delete");
        }
    }

    /// <summary>
    /// A serializable class to make conversion to json easier.
    /// makes sure Score is also serializable
    /// </summary>
    [System.Serializable]
    public class GameData
    {
        public double communityKills;
        public List<Score> leaderboard;
    }

}
