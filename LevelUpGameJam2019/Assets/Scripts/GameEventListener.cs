using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{

    //[System.Serializable]
    //public class RichEvent : UnityEvent { }

    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
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
