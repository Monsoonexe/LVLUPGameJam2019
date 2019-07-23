using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invokes Event when Player enters Trigger Volume.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerVolume : MonoBehaviour
{
    /// <summary>
    /// Event that triggers when the Player enters this volume.
    /// </summary>
    [Header("---Events---")]
    [SerializeField]//set in Inspector
    [Tooltip("Event that triggers when the Player enters this volume.")]
    private UnityEvent triggerEvent = new UnityEvent();

    public UnityEvent Event { get { return triggerEvent; } }//publicly accessible, readonly.

    //external refs
    private static GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("Player hit ending volume.  ending the level.", this);
            Event.Invoke();
        }
    }
}
