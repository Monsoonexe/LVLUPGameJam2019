using UnityEngine;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    [Header("---Scriptable Refs---")]
    [SerializeField]
    private ScoreData scoreData;

    [SerializeField]
    private LeaderboardSO leaderboard;

    [Header("---Windows---")]
    [SerializeField]
    private LevelEndReadoutController levelEndReadoutController;

    [SerializeField]
    private NewHighScoreWindowManager newHighScoreWindowManager;
    
    [Space(5)]
    [Header("---UI Elements---")]
    [SerializeField]
    private TextMeshProUGUI scoreTMPro;

    [SerializeField]
    private TextMeshProUGUI highScoreTMPro;

    //external Mono References
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerScoreText();
        UpdateHighScoreText();
        levelEndReadoutController.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update UI visuals with current values.  Must be called after any changes to values are made.
    /// </summary>
    private void UpdatePlayerScoreText()
    {
        scoreTMPro.text = scoreData.PlayerScore.ToString();
    }

    private void UpdateHighScoreText()
    {
        highScoreTMPro.text = leaderboard.highScore.ToString();
    }

    /// <summary>
    /// Procedure to follow at the end of the Level.
    /// </summary>
    public void OnLevelsEnd()
    {
        //check if Player got a new high score

        if (leaderboard.IsScoreOnLeaderboard(scoreData.PlayerScore))
        {
            newHighScoreWindowManager.gameObject.SetActive(true);
        }

        else
        {
            levelEndReadoutController.gameObject.SetActive(true);
            levelEndReadoutController.LoadTallyData(scoreData);
            levelEndReadoutController.ReadLeaderboardData(leaderboard);
        }
    }

    public void OnCustomerSatisfied(int numberOfIngredients)
    {

    }
}
