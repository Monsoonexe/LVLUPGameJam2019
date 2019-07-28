using UnityEngine;

public abstract class GameEventBaseObject : ScriptableObject
{
#pragma warning disable 0414
    [Header("=GameEventBase=")]
    [SerializeField]
    [TextArea(1, 3)]
    private string developerDescription = "Enter a description!";
#pragma warning restore
}
