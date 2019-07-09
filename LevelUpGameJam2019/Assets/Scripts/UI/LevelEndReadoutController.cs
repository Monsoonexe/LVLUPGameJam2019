using UnityEngine;
using TMPro;

public class LevelEndReadoutController : MonoBehaviour
{
    private ScoreManager scoreManager;

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
    
    private void Awake()
    {
        GatherReferences();
    }

    private void OnEnable()
    {
        GatherReferences();

        LoadTallyData();
        ReadLeaderboardData();
    }

    private void GatherReferences()
    {
        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }

    }

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

    [ContextMenu("Load Tally Data")]
    private void LoadTallyData()
    {
        if (!scoreManager) Debug.Log("FUCK ALL!");
        value_customersSatisfied.text = scoreManager.GetTallyCustomersSatisfied().ToString();
        value_customersMissed.text = scoreManager.GetTallyMissedOrders().ToString();
        value_customersHit.text = scoreManager.GetTallyCustomersHit().ToString();
        value_wrongOrders.text = scoreManager.GetTallyIncorrectOrders().ToString();
        value_sharksFed.text = scoreManager.GetTallySharksFed().ToString();
        value_piesFired.text = scoreManager.GetTallyShotsFired().ToString();
    }

    [ContextMenu("Read Leaderboard Data")]
    private void ReadLeaderboardData()
    {
        LoadLeaderboardEntry(entry1_name, entry1_score, scoreManager.GetEntry(0));
        LoadLeaderboardEntry(entry2_name, entry2_score, scoreManager.GetEntry(1));
        LoadLeaderboardEntry(entry3_name, entry3_score, scoreManager.GetEntry(2));
        LoadLeaderboardEntry(entry4_name, entry4_score, scoreManager.GetEntry(3));
        LoadLeaderboardEntry(entry5_name, entry5_score, scoreManager.GetEntry(4));
    }

    ///// <summary>
    ///// Called Externally.
    ///// </summary>
    ///// <param name="customersSatisfied"></param>
    ///// <param name="customersMissed"></param>
    ///// <param name="customersHit"></param>
    ///// <param name="wrongOrders"></param>
    ///// <param name="sharksFed"></param>
    ///// <param name="shotsFired"></param>
    //public void LoadTallyData(int customersSatisfied, int customersMissed, int customersHit, int wrongOrders, int sharksFed, int shotsFired)
    //{
    //    this.gameObject.SetActive(true);

    //    value_customersSatisfied.text = customersSatisfied.ToString();
    //    value_customersMissed.text = customersMissed.ToString();
    //    value_customersHit.text = customersHit.ToString();
    //    value_wrongOrders.text =wrongOrders.ToString();
    //    value_sharksFed.text = sharksFed.ToString();
    //    value_piesFired.text = shotsFired.ToString();
    //}

    ///// <summary>
    ///// Called Externally.
    ///// </summary>
    ///// <param name="leaderboardData"></param>
    //public void ReadLeaderboard(LeaderboardSO leaderboardData)
    //{
    //    LoadLeaderboardEntry(entry1_name, entry1_score, leaderboardData.GetEntry(0));
    //    LoadLeaderboardEntry(entry2_name, entry2_score, leaderboardData.GetEntry(1));
    //    LoadLeaderboardEntry(entry3_name, entry3_score, leaderboardData.GetEntry(2));
    //    LoadLeaderboardEntry(entry4_name, entry4_score, leaderboardData.GetEntry(3));
    //    LoadLeaderboardEntry(entry5_name, entry5_score, leaderboardData.GetEntry(4));
    //}

}
