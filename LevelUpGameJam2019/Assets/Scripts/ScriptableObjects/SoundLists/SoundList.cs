using UnityEngine;

[CreateAssetMenu(fileName = "SoundList_", menuName = "ScriptableObjects/New Sound list")]
public class SoundList : ScriptableObject
{
    [Header("---AudioClips---")]
    public AudioClip[] soundClipList;

    public AudioClip GetRandomSound()
    {
        return soundClipList[Random.Range(0, soundClipList.Length)];
    }
}
