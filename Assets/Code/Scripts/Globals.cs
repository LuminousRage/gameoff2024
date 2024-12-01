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
        { FloppyDiskID.A, new Color(191 / 255, 48 / 255, 146 / 255) },
        // cyan
        { FloppyDiskID.B, new Color(49 / 255, 189 / 255, 170 / 255) },
        // eyllow
        { FloppyDiskID.C, new Color(255 / 255, 200 / 255, 0 / 255) },
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
