﻿using UnityEngine;
using UnityEngine.Events;

public abstract class BaseGameEventListener<TType, TEvent, TResponse> : MonoBehaviour, IGameEventListener<TType>
    where TEvent : GameEventBase<TType>
    where TResponse : UnityEvent<TType>
{
    protected ScriptableObject GameEvent { get { return gameEvent; } }
    protected UnityEventBase Response { get { return response; } }

    [Header("= Listener Base =")]
    [SerializeField]
    private TEvent gameEvent = default;

    [SerializeField]
    private TResponse response = default;

    private void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventRaised(TType value)
    {
        response.Invoke(value);
    }
}

public abstract class GameEventListenerBase<TEvent, TResponse> : MonoBehaviour, IGameEventListener
    where TEvent : GameEventBase
    where TResponse : UnityEvent
{
    [Header("= Listener Base =")]
    [SerializeField]
    private TEvent gameEvent = default;

    protected virtual ScriptableObject GameEvent { get { return gameEvent; } }

    [SerializeField]
    private TResponse response = default;

    protected virtual UnityEventBase Response { get { return response; } }

    private void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}