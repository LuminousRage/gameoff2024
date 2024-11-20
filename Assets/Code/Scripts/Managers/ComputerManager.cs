using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
    // Level -> (Avatar, Zone) -> Computer
    public Dictionary<Level2D, Dictionary<(byte, int), Computer>> computerLookUp;

    void BuildComputerLookUp()
    {
        var computers = GetComponentsInChildren<Computer>();
        computers.Aggregate(
            computerLookUp,
            (acc, computer) =>
            {
                if (!acc.ContainsKey(computer.level))
                {
                    acc[computer.level] = new Dictionary<(byte, int), Computer>();
                }

                // acc[computer.level][(computer.avatar, computer.zone)] = computer;

                return acc;
            }
        );
    }
}
