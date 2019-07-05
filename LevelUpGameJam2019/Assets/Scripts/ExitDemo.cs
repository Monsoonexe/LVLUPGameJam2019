using UnityEngine;

public class ExitDemo : MonoBehaviour
{
    private static GameController gameControllerInstance;

    [SerializeField]
    private Transform EndGameWhenThisObjectEntersVolume;

    private void Start()
    {
        //gather references
        if (!gameControllerInstance)
        {
            gameControllerInstance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(EndGameWhenThisObjectEntersVolume == other.gameObject)
        {
            Debug.Log("Player hit end collider.  Ending the game.");
            gameControllerInstance.OnLevelsEnd();//tell GO this is so and move on to next phase
        }
    }
}
