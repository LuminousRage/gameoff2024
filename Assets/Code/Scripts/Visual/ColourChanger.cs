using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public ZoneToMaterial[] materials;

    [Serializable]
    public struct ZoneToMaterial
    {
        public Globals.Zone zone;
        public Material material;
    }

    [HideInInspector]
    public Dictionary<Globals.Zone, Material> zoneToMaterial =
        new Dictionary<Globals.Zone, Material>();

    void MakeLookUpTable()
    {
        zoneToMaterial = materials
            .ToList()
            .Aggregate(
                zoneToMaterial,
                (acc, zm) =>
                {
                    acc[zm.zone] = zm.material;
                    return acc;
                }
            );
    }

    void Start()
    {
        MakeLookUpTable();
    }
}
