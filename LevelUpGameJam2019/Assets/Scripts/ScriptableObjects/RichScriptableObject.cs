using UnityEngine;

/// <summary>
/// Gives every SO a description the developers can write notes in.
/// </summary>
public abstract class RichScriptableObject : ScriptableObject
{
#pragma warning disable 0414
    [Header("= Game Event Base =")]
    [SerializeField]
    [TextArea(1, 5)]
    private string developerDescription = "Enter a description!";
#pragma warning restore
}
