using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighScores_", menuName = "ScriptableObjects/New High Score")]
public class LeaderboardSO : ScriptableObject
{
    [SerializeField]
    private int highScore = 0;

    /// <summary>
    /// [Alpha]
    /// </summary>
    [Header("***Leaderboard***")]
    [Tooltip("Alpha")]
    [SerializeField]
    private int[] leaderboardScores = new int[5];

    public void SetNewHighScore(int newScore)
    {
        if(newScore > highScore)
        {
            highScore = newScore;
        }
        else
        {
            Debug.LogError("What are you trying to pull? Hax!");
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
}
