using UnityEngine;

public class ExitDemo : MonoBehaviour
{
    private static LevelManager levelManager;

    [SerializeField]
    private GameObject EndGameWhenThisObjectEntersVolume;

    private void Start()
    {
        //gather references
        if (!levelManager)
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(EndGameWhenThisObjectEntersVolume == other.gameObject)
        {
            Debug.Log("Player hit end collider.  Ending the game.");
            levelManager.OnLevelsEnd();//tell GO this is so and move on to next phase
        }
    }
}
