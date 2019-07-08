﻿using UnityEngine;

[CreateAssetMenu(fileName = "Leaderboard_", menuName = "ScriptableObjects/New Leaderboard")]
public class LeaderboardSO : ScriptableObject
{
    private const int leaderboardSize = 5;

    public int highScore { get { return leaderboardScores[0].score; } }

    /// <summary>
    /// [Alpha]
    /// </summary>
    [Header("***Leaderboard***")]
    [Tooltip("[Alpha]")]
    [SerializeField]
    private LeaderboardEntry[] leaderboardScores = new LeaderboardEntry[leaderboardSize];

    [ContextMenu("WIPE ALL SCORES")]
    private void ResetAllScores()
    {
        leaderboardScores = new LeaderboardEntry[leaderboardSize];
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
        if(index > 0 && index < leaderboardScores.Length)
        {
            return leaderboardScores[index];
        }
        else
        {
            Debug.LogError("ERROR! Request leaderboard Entry out of array index!");
            //gotta return something.
            return new LeaderboardEntry("", 0);
        }
    }
}
