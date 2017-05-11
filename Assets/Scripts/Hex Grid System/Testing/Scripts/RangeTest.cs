using System.Collections.Generic;
using UnityEngine;

public class RangeTest : MonoBehaviour
{
    public int range = 2;

    private List<Hex> rangeObjects = new List<Hex>();

    public void DoRangeStuff()
    {
        foreach (var h in Grid.Instance.Hexes)
            h.transform.FindChild("hex_tile").GetComponentInChildren<Renderer>().material.color = Color.white;

        rangeObjects = Grid.HexesInRange(new Hex(6, -12, 6), range);
        foreach (var h in rangeObjects)
        {
            Grid.FindHexObject(h.cubeCoords).transform.FindChild("hex_tile").GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }
}