using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A custom serializable class that is used to populate the leaderboard
/// for this game
/// </summary>
[System.Serializable]
public class Score
{
    public string username;
    public float score;

    public Score(string username, float score)
    {
        this.username = username;
        this.score = score;
    }
}
