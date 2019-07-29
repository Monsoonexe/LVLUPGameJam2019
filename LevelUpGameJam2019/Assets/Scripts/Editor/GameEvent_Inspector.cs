using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEvent_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEvent gameEvent = target as GameEvent;
        if(GUILayout.Button("Raise Event"))
        {
            gameEvent.Raise();
        }
    }

}
