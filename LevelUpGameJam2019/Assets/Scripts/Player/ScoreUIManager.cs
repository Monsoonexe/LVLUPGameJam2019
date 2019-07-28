using UnityEngine;
using UnityEngine.UI;
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
    private GameObject newHighScoreWindow;
    
    [Space(5)]
    [Header("---UI Elements---")]
    [SerializeField]
    private TextMeshProUGUI scoreTMPro;

    [SerializeField]
    private TextMeshProUGUI highScoreTMPro;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TMP_InputField nameInputText;

    [SerializeField]
    private Button okayButton;

    //external Mono References
    private LevelManager levelManager;

    // Start is called before the first frame update
    private void Start()
    {
        OnLevelBegin();
    }

    private void UpdateHighScoreText()
    {
        highScoreTMPro.text = leaderboard.HighScore.ToString();
    }

    /// <summary>
    /// Update UI visuals with current values.  Must be called after any changes to values are made.
    /// </summary>
    public void UpdatePlayerScoreText()
    {
        scoreTMPro.text = scoreData.PlayerScore.ToString();
    }

    public void OnLevelBegin()
    {
        UpdatePlayerScoreText();
        UpdateHighScoreText();
        levelEndReadoutController.gameObject.SetActive(false);
    }

    /// <summary>
    /// Procedure to follow at the end of the Level.
    /// </summary>
    public void OnLevelsEnd()
    {
        //check if Player got a new high score
        if (leaderboard.IsScoreOnLeaderboard(scoreData.PlayerScore))
        {
            newHighScoreWindow.gameObject.SetActive(true);
            scoreText.text = scoreData.PlayerScore.ToString();
        }

        else
        {
            levelEndReadoutController.gameObject.SetActive(true);
            levelEndReadoutController.LoadTallyData(scoreData);
            levelEndReadoutController.ReadLeaderboardData(leaderboard);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerName"></param>
    public void ConfirmNewHighScoreName()
    {
        var newName = nameInputText.text;//read name from UI element
        leaderboard.SubmitNewScore(new LeaderboardEntry(newName, scoreData.PlayerScore));
        levelEndReadoutController.gameObject.SetActive(true);
        levelEndReadoutController.LoadTallyData(scoreData);
        levelEndReadoutController.ReadLeaderboardData(leaderboard);
        UpdateHighScoreText();
    }
}
