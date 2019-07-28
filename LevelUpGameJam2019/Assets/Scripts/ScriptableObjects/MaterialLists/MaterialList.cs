using UnityEngine;

[CreateAssetMenu(fileName = "MaterialList_", menuName = "ScriptableObjects/New Material List")]
public class MaterialList : ScriptableObject
{
    [Header("= Material List =")]
    [SerializeField]
    private Material[] materialList;

    public Material[] MatList {  get { return materialList; } }//public readonly
}
