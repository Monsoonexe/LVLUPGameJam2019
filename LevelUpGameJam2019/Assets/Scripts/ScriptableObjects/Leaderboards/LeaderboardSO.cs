using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leaderboard_", menuName = "ScriptableObjects/New Leaderboard")]
public class LeaderboardSO : ScriptableObject
{
    [SerializeField]
    private int highScore = 0;

    /// <summary>
    /// [Alpha]
    /// </summary>
    [Header("***Leaderboard***")]
    [Tooltip("[Alpha]")]
    [SerializeField]
    private LeaderboardEntry[] leaderboardScores = new LeaderboardEntry[5];

    /// <summary>
    /// Set a new high score.
    /// </summary>
    /// <param name="newScore"></param>
    public void SetNewHighScore(int newScore)
    {
        if(newScore > highScore)
        {
            highScore = newScore;
        }
        else
        {
            Debug.LogError("What are you trying to pull? Haxx!");
        }
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public override string ToString()
    {
        return base.ToString();
        //do more here.
    }

    public void SubmitNewScore(int newScore)
    {
        //determine if new score belongs on leaderboard
        //determine which position
        //add this to proper place and move all other names lower
        //remove lowest name

    }
}
