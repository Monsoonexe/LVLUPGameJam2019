﻿using UnityEngine;

[CreateAssetMenu(fileName = "Leaderboard_", menuName = "ScriptableObjects/New Leaderboard")]
public class LeaderboardSO : RichScriptableObject
{
    private const int defaultLeaderboardSize = 5;

    public int HighScore { get { return leaderboardScores[0].score; } }//readonly

    /// <summary>
    /// 
    /// </summary>
    [Header("***Leaderboard***")]
    [SerializeField]
    private LeaderboardEntry[] leaderboardScores = new LeaderboardEntry[defaultLeaderboardSize];

    [ContextMenu("WIPE ALL SCORES")]
    private void ResetAllScores()
    {
        leaderboardScores = new LeaderboardEntry[defaultLeaderboardSize];
    }

    public override string ToString()
    {
        return base.ToString();
        //do more here.
    }

    public void SubmitNewScore(LeaderboardEntry newScoreEntry)
    {
        //determine if new score belongs on leaderboard
        for(var i = 0; i < leaderboardScores.Length; ++i)
        {
            if(leaderboardScores[i].score < newScoreEntry.score)
            {
                var temp = leaderboardScores[i];
                leaderboardScores[i] = newScoreEntry;
                newScoreEntry = temp;
            }
        }
    }
    
    public LeaderboardEntry GetEntry(int index)
    {
        if(index >= 0 && index < leaderboardScores.Length)
        {
            return leaderboardScores[index];
        }
        else
        {
            Debug.LogError("ERROR! Request leaderboard Entry out of array index: " + index.ToString());
            //gotta return something.
            return new LeaderboardEntry("", 0);
        }
    }

    /// <summary>
    /// Is the given score in top?
    /// </summary>
    /// <param name="playerScore"></param>
    /// <returns></returns>
    public bool IsScoreOnLeaderboard(int playerScore)
    {
        //is player score greater than lowest score?
        return playerScore > leaderboardScores[leaderboardScores.Length - 1].score;
    }
}
