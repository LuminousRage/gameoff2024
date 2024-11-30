using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public static class Globals
{
    public enum Zone
    {
        Broken,
        A,
        B,
        C,
    }

    public enum FloppyDiskID
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
    }

    public static readonly Dictionary<FloppyDiskID, Color> FloppyColourMap = new Dictionary<
        FloppyDiskID,
        Color
    >()
    {
        { FloppyDiskID.A, new Color(0.85f, 0.85f, 0.85f) },
        { FloppyDiskID.B, Color.blue },
        { FloppyDiskID.C, Color.green },
        { FloppyDiskID.D, Color.yellow },
        { FloppyDiskID.E, Color.cyan },
        { FloppyDiskID.F, Color.magenta },
        { FloppyDiskID.G, Color.red },
    };

    public static Color GetFloppyColor(FloppyDiskID id)
    {
        Assert.IsTrue(
            FloppyColourMap.ContainsKey(id),
            $"No colour available for Floppy Disk ID {id}"
        );

        return FloppyColourMap[id];
    }

    public static readonly Dictionary<Zone, Color> ZoneColourMap = new Dictionary<Zone, Color>()
    {
        { Zone.Broken, new Color(0.85f, 0.85f, 0.85f) },
        { Zone.B, Color.blue },
        { Zone.A, Color.green },
        { Zone.C, Color.magenta },
    };
}
