using UnityEngine;

public class PizzaShipController : MonoBehaviour
{
    [SerializeField]//can only set in Inspector
    private Transform cannonSpawnPoint;
    public Transform CannonSpawnPoint { get { return cannonSpawnPoint; } }//easy to get, only the Inspector can set.
            
}
