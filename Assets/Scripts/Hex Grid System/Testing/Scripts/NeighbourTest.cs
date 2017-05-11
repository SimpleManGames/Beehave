using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeighbourTest : MonoBehaviour
{
    public int tileIndex = 25;

    public List<Hex> neighbourObjects = new List<Hex>();

    public void DoNeighbourStuff()
    {
        neighbourObjects.ForEach(h => Grid.FindHexObject(h.cubeCoords).transform.Find("hex_tile").GetComponentInChildren<Renderer>().material.color = Color.white);

        neighbourObjects = Hex.Neighbours(Grid.FindHexObject(tileIndex).hex).ToList();
        neighbourObjects.ForEach(h =>
        {
            if (Grid.FindHexObject(h.cubeCoords) != null)
                Grid.FindHexObject(h.cubeCoords).transform.Find("hex_tile").GetComponentInChildren<Renderer>().material.color = Color.magenta;
        });
    }
}
