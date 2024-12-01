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
    }

    public static readonly Dictionary<FloppyDiskID, Color> FloppyColourMap = new Dictionary<
        FloppyDiskID,
        Color
    >()
    {
        // magenta
        { FloppyDiskID.A, new Color(191 / 255f, 48 / 255f, 146 / 255f) },
        // cyan
        { FloppyDiskID.B, new Color(49 / 255f, 189 / 255f, 170 / 255f) },
        // yellow
        { FloppyDiskID.C, new Color(255 / 255f, 200 / 255f, 0 / 255f) },
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
