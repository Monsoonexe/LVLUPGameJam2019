using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewHighScoreWindowManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TMP_InputField nameInputText;

    [SerializeField]
    private Button okayButton;

    //External Mono References
    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Awake()
    {
        GatherReferences();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        scoreText.text = scoreManager.GetPlayerScore().ToString();
    }

    private void GatherReferences()
    {
        if (!scoreManager)
        {
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
        }
    }

    public void ConfirmInput()
    {
        var newName = nameInputText.text;
        scoreManager.ConfirmNewHighScoreName(newName);
        this.gameObject.SetActive(false);
    }
}
