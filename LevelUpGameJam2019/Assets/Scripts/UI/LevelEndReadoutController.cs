using UnityEngine;
using TMPro;

public class LevelEndReadoutController : MonoBehaviour
{
    [Header("---Tally Labels---")]
    [SerializeField]
    private TextMeshProUGUI label_customersSatisfied;
    
    [SerializeField]
    private TextMeshProUGUI label_customersMissed;
    
    [SerializeField]
    private TextMeshProUGUI label_customersHit;
    
    [SerializeField]
    private TextMeshProUGUI label_wrongOrders;
    
    [SerializeField]
    private TextMeshProUGUI label_sharksFed;
    
    [SerializeField]
    private TextMeshProUGUI label_piesFired;

    [Space(5)]
    [Header("---Tally Values---")]
    [SerializeField]
    private TextMeshProUGUI value_customersSatisfied;
    
    [SerializeField]
    private TextMeshProUGUI value_customersMissed;
    
    [SerializeField]
    private TextMeshProUGUI value_customersHit;
    
    [SerializeField]
    private TextMeshProUGUI value_wrongOrders;
    
    [SerializeField]
    private TextMeshProUGUI value_sharksFed;
    
    [SerializeField]
    private TextMeshProUGUI value_piesFired;

    [Space(5)]
    [Header("---Leaderboard Scores---")]
    [SerializeField]
    private TextMeshProUGUI entry1_score;

    [SerializeField]
    private TextMeshProUGUI entry2_score;

    [SerializeField]
    private TextMeshProUGUI entry3_score;

    [SerializeField]
    private TextMeshProUGUI entry4_score;

    [SerializeField]
    private TextMeshProUGUI entry5_score;

    [Space(5)]
    [Header("---Leaderboard Initials---")]
    [SerializeField]
    private TextMeshProUGUI entry1_name;

    [SerializeField]
    private TextMeshProUGUI entry2_name;

    [SerializeField]
    private TextMeshProUGUI entry3_name;

    [SerializeField]
    private TextMeshProUGUI entry4_name;

    [SerializeField]
    private TextMeshProUGUI entry5_name;
    
    /// <summary>
    /// Load data into TMPro Texts.
    /// </summary>
    /// <param name="nameElement"></param>
    /// <param name="scoreElement"></param>
    /// <param name="entryData"></param>
    private static void LoadLeaderboardEntry(TextMeshProUGUI nameElement, TextMeshProUGUI scoreElement, LeaderboardEntry entryData)
    {
        nameElement.text = entryData.name.ToString();
        scoreElement.text = entryData.score.ToString();
    }

    public void LoadTallyData(ScoreData scoreData)
    { 
        value_customersSatisfied.text = scoreData.CustomersSatisfied.ToString();
        value_customersMissed.text = scoreData.MissedOrders.ToString();
        value_customersHit.text = scoreData.CustomersHit.ToString();
        value_wrongOrders.text = scoreData.IncorrectOrders.ToString();
        value_sharksFed.text = scoreData.SharksFed.ToString();
        value_piesFired.text = scoreData.ShotsFired.ToString();
    }

    public void ReadLeaderboardData(LeaderboardSO leaderboard)
    {
        LoadLeaderboardEntry(entry1_name, entry1_score, leaderboard.GetEntry(0));
        LoadLeaderboardEntry(entry2_name, entry2_score, leaderboard.GetEntry(1));
        LoadLeaderboardEntry(entry3_name, entry3_score, leaderboard.GetEntry(2));
        LoadLeaderboardEntry(entry4_name, entry4_score, leaderboard.GetEntry(3));
        LoadLeaderboardEntry(entry5_name, entry5_score, leaderboard.GetEntry(4));
    }
}
