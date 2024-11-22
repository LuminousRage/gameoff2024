using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
    // Level -> (Avatar, Zone) -> Computer
    public Dictionary<Level2D, Dictionary<(byte, Globals.Zone), Computer>> computerLookUp;

    void Start()
    {
        BuildComputerLookUp();
    }

    void BuildComputerLookUp()
    {
        var computers = GetComponentsInChildren<Computer>();
        computerLookUp = computers.Aggregate(
            new Dictionary<Level2D, Dictionary<(byte, Globals.Zone), Computer>>(),
            (acc, computer) =>
            {
                if (!acc.ContainsKey(computer.level))
                {
                    acc[computer.level] = new Dictionary<(byte, Globals.Zone), Computer>();
                }

                acc[computer.level][(computer.avatar, computer.zone)] = computer;

                return acc;
            }
        );
    }
}
