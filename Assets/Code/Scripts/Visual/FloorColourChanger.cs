using NUnit.Framework;
using UnityEngine;

public class FloorColourChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var meshRenderer_ = GetComponent<MeshRenderer>();
        Assert.IsNotNull(meshRenderer_, "Unable to find MeshRenderer in FloorColourChanger.");
        var dimensionInfo = GetComponentInParent<Dimension>();
        Assert.IsNotNull(dimensionInfo, "Unable to find Dimension in FloorColourChanger.");

        var wallBottomMaterials = FindFirstObjectByType<ColourChanger>().zoneToMaterial;
        meshRenderer_.material = wallBottomMaterials[dimensionInfo.zone];
    }
}
