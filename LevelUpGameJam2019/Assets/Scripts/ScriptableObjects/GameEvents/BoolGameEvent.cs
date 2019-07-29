using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BoolGameEvent_", menuName = "ScriptableObjects/Game Events/Bool Game Event")]
public class BoolGameEvent : GameEventBase<bool>
{
    //it's an event. it just exists. takes 1 bool parameter.
}
