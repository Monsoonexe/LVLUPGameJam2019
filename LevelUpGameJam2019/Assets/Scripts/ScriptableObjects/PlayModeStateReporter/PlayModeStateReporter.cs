using UnityEngine;
using UnityEditor;

/// <summary>
/// SO that fires events based on clicking Play, Pause, and Exit Button. I SUCK AND I DONT WORK!
/// </summary>
[CreateAssetMenu(fileName = "PlayModeStateReporter", menuName = "ScriptableObjects/PlayMode State Reporter")]
public class PlayModeStateReporter : RichScriptableObject
{
    [SerializeField]
    private PlayModeStateChange currentState;

    [Header("---Game Events---")]
    [SerializeField]
    private GameEvent enteredPlayModeEvent;

    [SerializeField]
    private GameEvent exitPlayModeEvent;
    
    [ContextMenu("Initialize Object")]
    private void Initialize()
    {
        Debug.Log(name.ToString() + " has been initialized.  Will only work once.", this);
        EditorApplication.playModeStateChanged += ModeChanged;
    }

    public void ModeChanged(PlayModeStateChange newState)
    {
        Debug.Log("Changed state from: " + currentState + " to: " + newState);
        currentState = newState;
        Initialize();//reload

        switch (currentState)
        {
            case PlayModeStateChange.EnteredEditMode:
                exitPlayModeEvent.Raise();
                break;
            case PlayModeStateChange.EnteredPlayMode:
                enteredPlayModeEvent.Raise();
                break;
        }
    }

}
