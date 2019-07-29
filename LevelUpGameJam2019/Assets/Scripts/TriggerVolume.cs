using UnityEngine;

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
    [Tooltip("Event that triggers when the target enters this volume.")]
    private GameEvent _event;

    //external refs
    [SerializeField]
    private Transform targetXform;

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        if (!targetXform)
        {
            targetXform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == targetXform)
        {
            Debug.Log("Player hit ending volume.  Throwing Event.", this);
            _event.Raise();
        }
    }
}
