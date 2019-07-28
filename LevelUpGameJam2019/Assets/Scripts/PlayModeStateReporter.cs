using UnityEngine;

/// <summary>
/// Fires events based on clicking Play, Pause, and Exit Button.
/// </summary>
public class PlayModeStateReporter : MonoBehaviour
{
    [Header("= Developer Description =")]
    [SerializeField]
    [TextArea]
    private string description = "Fires events based on clicking Play, Pause, and Exit Button.  Good for demo / testing.";

    [Header("---Game Events---")]
    [SerializeField]
    private GameEvent enteredPlayModeEvent;

    [SerializeField]
    private GameEvent exitPlayModeEvent;
    
    private void Awake()
    {
        enteredPlayModeEvent.Raise();
    }

    public void OnApplicationQuit()
    {
        exitPlayModeEvent.Raise();
    }

}
