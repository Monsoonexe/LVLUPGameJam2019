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

    private void Start()
    {
        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }
    }

    private static void LoadLeaderboardEntry(TextMeshProUGUI nameElement, TextMeshProUGUI scoreElement, LeaderboardEntry entryData)
    {
        nameElement.text = entryData.name.ToString();
        scoreElement.text = entryData.score.ToString();
    }

    [ContextMenu("Load Tally Data")]
    public void LoadTallyData()
    {
        this.gameObject.SetActive(true);

        value_customersSatisfied.text = scoreManager.GetTallyCustomersSatisfied().ToString();
        value_customersMissed.text = scoreManager.GetTallyMissedOrders().ToString();
        value_customersHit.text = scoreManager.GetTallyCustomersHit().ToString();
        value_wrongOrders.text = scoreManager.GetTallyIncorrectOrders().ToString();
        value_sharksFed.text = scoreManager.GetTallySharksFed().ToString();
        value_piesFired.text = scoreManager.GetTallyShotsFired().ToString();
    }

    public void LoadTallyData(int customersSatisfied, int customersMissed, int customersHit, int wrongOrders, int sharksFed, int shotsFired)
    {
        this.gameObject.SetActive(true);

        value_customersSatisfied.text = customersSatisfied.ToString();
        value_customersMissed.text = customersMissed.ToString();
        value_customersHit.text = customersHit.ToString();
        value_wrongOrders.text =wrongOrders.ToString();
        value_sharksFed.text = sharksFed.ToString();
        value_piesFired.text = shotsFired.ToString();
    }


    public void ReadLeaderboard(LeaderboardSO leaderboardData)
    {
        
    }

}
