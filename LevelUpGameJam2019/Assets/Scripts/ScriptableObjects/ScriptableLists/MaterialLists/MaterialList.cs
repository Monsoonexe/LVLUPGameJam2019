using UnityEngine;

[CreateAssetMenu(fileName = "MaterialList_", menuName = "ScriptableObjects/Scriptable Lists/Material List")]
public class MaterialList : ScriptableObject
{
    [Header("= Material List =")]
    [SerializeField]
    private Material[] materialList;

    public Material[] MatList {  get { return materialList; } }//public readonly
}
