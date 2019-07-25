using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

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

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
    
}
