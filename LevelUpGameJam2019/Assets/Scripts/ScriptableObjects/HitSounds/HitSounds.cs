using UnityEngine;

[CreateAssetMenu(fileName = "HitSounds_", menuName = "ScriptableObjects/New Hit Sounds")]
public class HitSounds : ScriptableObject
{
    [Header("---AudioClips---")]
    [SerializeField]
    private AudioClip[] hitSounds;

}
