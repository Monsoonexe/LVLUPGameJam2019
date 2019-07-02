using UnityEngine;

[CreateAssetMenu(fileName = "CustomerProfile_", menuName = "ScriptableObjects/New Customer Profile")]
public class CustomerProfile : ScriptableObject
{
    [Header("---Audio---")]
    public SoundList hitWithPizzaSounds;

    public SoundList badOrderSounds;

    public SoundList customerSatisfiedSounds;

    [Header("---Visual---")]
    public Mesh characterVisualMesh;

    public MaterialList materialVariations;

    public Material GetRandomMaterial()
    {
        return materialVariations.materialList[Random.Range(0, materialVariations.materialList.Length)];
    }
}
