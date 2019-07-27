using UnityEngine;

[CreateAssetMenu(fileName = "Dialog_", menuName = "ScriptableObjects/New Dialog")]
public class TutorialDialogSO : ScriptableObject
{
    [SerializeField]
    [TextArea]
    private string[] sentences;
    private int index;
}
