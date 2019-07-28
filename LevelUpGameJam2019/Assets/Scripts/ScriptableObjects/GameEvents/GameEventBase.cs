using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventBase<T> : GameEventBase, IGameEvent<T>
{
    private readonly List<IGameEventListener<T>> typedListeners = new List<IGameEventListener<T>>();
    private readonly List<System.Action<T>> typedActions = new List<System.Action<T>>();

    [SerializeField]
    protected T debugValue = default;//like saying "null", but for generics

    public void AddListener(IGameEventListener<T> listener)
    {
        if (!typedListeners.Contains(listener))
            typedListeners.Add(listener);
    }

    public void AddListener(System.Action<T> action)
    {
        if (!typedActions.Contains(action))
            typedActions.Add(action);
    }

    public void Raise(T value)
    {
        //TODO AddStackTrace(value);

        for (var i = typedListeners.Count - 1; i >= 0; --i)
            typedListeners[i].OnEventRaised(value);

        for (var i = typedActions.Count - 1; i >= 0; --i)
            typedActions[i](value);
    }

    override public void RemoveAll()
    {
        base.RemoveAll();
        typedListeners.RemoveRange(0, typedListeners.Count);
        typedActions.RemoveRange(0, typedActions.Count);
    }

    public void RemoveListener(IGameEventListener<T> listener)
    {
        if (typedListeners.Contains(listener))
            typedListeners.Remove(listener);
    }

    public void RemoveListener(System.Action<T> action)
    {
        if (typedActions.Contains(action))
            typedActions.Remove(action);
    }
}

public abstract class GameEventBase : GameEventBaseObject, IGameEvent
{
    /// <summary>
    /// Collection of listeners.
    /// </summary>
    protected readonly List<IGameEventListener> listeners = new List<IGameEventListener>();//
    protected readonly List<System.Action> actions = new List<System.Action>();//function pointers. can handle actions, too

    #region stack trace
    //    public List<StackTraceEntry> StackTraces { get { return _stackTraces; } }
    //    private List<StackTraceEntry> _stackTraces = new List<StackTraceEntry>();

    //    public void AddStackTrace()
    //    {
    //#if UNITY_EDITOR
    //        if (SOArchitecture_Settings.Instance.EnableDebug)
    //            _stackTraces.Insert(0, StackTraceEntry.Create());
    //#endif
    //    }
    //    public void AddStackTrace(object value)
    //    {
    //#if UNITY_EDITOR
    //        if (SOArchitecture_Settings.Instance.EnableDebug)
    //            _stackTraces.Insert(0, StackTraceEntry.Create(value));
    //#endif
    //    }
    #endregion

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
    virtual public void RemoveAll()
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
