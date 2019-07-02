using UnityEngine;

[CreateAssetMenu(fileName = "MaterialList_", menuName = "ScriptableObjects/New Material List")]
public class MaterialList : ScriptableObject
{
    public Material[] materialList;
}
