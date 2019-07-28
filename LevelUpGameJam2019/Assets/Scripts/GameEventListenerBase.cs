using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerBase : MonoBehaviour, IGameEventListener
{

    //[System.Serializable]
    //public class RichEvent : UnityEvent { }

    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.AddListener(this);
    }

    private void OnDisable()
    {
        Event.RemoveListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }

    //MAKE THIS WORK!!!!!
    //public void OnEventRaised(int value)
    //{
    //    Response.Invoke(value)
    //}
}
