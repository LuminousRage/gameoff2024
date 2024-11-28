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
        D,
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
        H,
        I,
        J,
        K,
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
        { FloppyDiskID.H, new Color(0.6f, 0.4f, 0.1f) },
        { FloppyDiskID.I, new Color(0.0f, 0.4f, 0.8f) },
        { FloppyDiskID.J, new Color(1f, 0.0f, 1f) },
        { FloppyDiskID.K, new Color(0.5f, 0.2f, 0f) },
    };

    public static Color GetFloppyColor(FloppyDiskID id)
    {
        Assert.IsTrue(
            FloppyColourMap.ContainsKey(id),
            $"No colour available for Floppy Disk ID {id}"
        );

        return FloppyColourMap[id];
    }
}
