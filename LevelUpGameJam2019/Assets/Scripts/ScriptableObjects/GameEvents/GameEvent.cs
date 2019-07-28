using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : GameEvent, IGameEvent<T>
{
    private readonly List<IGameEventListener<T>> typedListeners = new List<IGameEventListener<T>>();

    [SerializeField]
    protected T debugValue = default;//like saying "null", but for generics

    public void AddListener(IGameEventListener<T> listener)
    {
        throw new System.NotImplementedException();
    }

    public void Raise(T value)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveAll()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveListener(IGameEventListener<T> listener)
    {
        throw new System.NotImplementedException();
    }
}

public abstract class GameEvent : GameEventBase, IGameEvent
{
    /// <summary>
    /// Collection of listeners.
    /// </summary>
    protected readonly List<IGameEventListener> listeners = new List<IGameEventListener>();//
    protected readonly List<System.Action> actions = new List<System.Action>();//function pointers. can handle actions, too

    /// <summary>
    /// Subscribe a new listener.
    /// </summary>
    /// <param name="listener"></param>
    public void AddListener(IGameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    /// <summary>
    /// Subscribe a new action.
    /// </summary>
    /// <param name="action"></param>
    public void AddListener(System.Action action)
    {
        if (!actions.Contains(action))
            actions.Add(action);
    }

    /// <summary>
    /// Call all events that are subscribed to event.
    /// </summary>
    public void Raise()
    {
        //TODO add stack trace
        for (var i = listeners.Count - 1; i >= 0; --i)//iterate backwards for safely removing self
            listeners[i].OnEventRaised();//call listener
        for (var i = actions.Count - 1; i >= 0; --i)
            actions[i]();//call action
    }

    /// <summary>
    /// Remove all actions and listeners.
    /// </summary>
    public void RemoveAll()
    {
        listeners.RemoveRange(0, listeners.Count);
        actions.RemoveRange(0, actions.Count);
    }

    /// <summary>
    /// Remove given listener.
    /// </summary>
    /// <param name="listener"></param>
    public void RemoveListener(IGameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

    /// <summary>
    /// Removed given action.
    /// </summary>
    /// <param name="action"></param>
    public void RemoveListener(System.Action action)
    {
        if (actions.Contains(action))
            actions.Remove(action);
    }
}
