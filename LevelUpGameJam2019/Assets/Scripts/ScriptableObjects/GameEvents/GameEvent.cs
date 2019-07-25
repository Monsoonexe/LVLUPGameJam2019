using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent_", menuName = "ScriptableObjects/Game Event")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> listeners = new List<GameEventListener>();

    [SerializeField]
    [TextArea]
    private string DeveloperDescription;

    /// <summary>
    /// Call all functions in list.
    /// </summary>
    /// <remarks>Calls in reverse order to allow for listeners removing themselves.</remarks>
    public void Raise()
    {
        for(var i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].OnEventRaised();
        }
    }

    //make this work!!!
    //public void Raise(int value)
    //{
    //    for (var i = listeners.Count - 1; i >= 0; --i)
    //    {
    //        listeners[i].OnEventRaised(value);
    //    }
    //}

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
    
}
