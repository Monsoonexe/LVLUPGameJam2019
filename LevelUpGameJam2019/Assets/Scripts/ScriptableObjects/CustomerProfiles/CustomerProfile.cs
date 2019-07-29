using UnityEngine;

[CreateAssetMenu(fileName = "CustomerProfile_", menuName = "ScriptableObjects/New Customer Profile")]
public class CustomerProfile : RichScriptableObject
{
    [Header("= Customer Profile =")]
    [SerializeField]
    [Tooltip("Developer-facing name of template.")]

    private string profileName = "Default";
    [Header("---Audio---")]
    public SoundList hitWithPizzaSounds;

    public SoundList badOrderSounds;

    public SoundList customerSatisfiedSounds;

    [Header("---Visual---")]
    public Mesh characterVisualMesh;

    public MaterialList materialVariations;

    public Material GetRandomMaterial()
    {
        return materialVariations.list[Random.Range(0, materialVariations.list.Count)];
    }
}
