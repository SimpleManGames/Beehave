using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Obsolete]
public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexPrefab = null;
    public GameObject groundObject = null;

    public LayerMask layerMask;

    private GameObject HexGridObject
    {
        get
        {
            GameObject returnObject = GameObject.Find("HexGridObject");
            if (returnObject == null)
                returnObject = new GameObject("HexGridObject");

            return returnObject;
        }
    }

    private float hexWidth;
    private float hexHeight;
    private float groundWidth;
    private float groundHeight;

    // List of gameobject to store refs to the actual unity side hex's
    private static List<HexObject> hexGridObjects = new List<HexObject>();

    public static HexObject GetHexByIndex(int index)
    {
        return hexGridObjects.Where(i => i.Index == index).First();
    }

    void Start()
    {
        CalculateBounds();
        CreateGrid();
        SetTerrainPotential();
    }

    void CalculateBounds()
    {
        hexWidth = hexPrefab.transform.FindChild("hex_tile").GetComponentInChildren<Renderer>().bounds.size.x;
        hexHeight = hexPrefab.transform.FindChild("hex_tile").GetComponentInChildren<Renderer>().bounds.size.z;
        groundWidth = groundObject.transform.FindChild("Model").GetComponent<Renderer>().bounds.size.x;
        groundHeight = groundObject.transform.FindChild("Model").GetComponent<Renderer>().bounds.size.z;
    }

    Vector3 CalculateInitPosition()
    {
        return new Vector3(-groundWidth / 2 + hexWidth, 0, groundHeight / 2 - hexWidth / 2);
    }

    Vector2 CalculateGridSize()
    {
        float sideLength = hexHeight / 2;
        int numOfSides = (int)(groundHeight / sideLength);
        int gridHeightInHexes = (int)(numOfSides * 2 / 3);

        if (gridHeightInHexes % 2 == 0 && (numOfSides + 0.5f) * sideLength > groundHeight)
            gridHeightInHexes--;

        return new Vector2((int)(groundWidth / hexWidth), gridHeightInHexes);
    }

    Vector3 CalculateWorldCoord(Vector2 gridPos)
    {
        Vector3 initPos = CalculateInitPosition();
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = initPos.x + offset + gridPos.x * hexWidth;
        float z = initPos.z + gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        Vector2 gridSize = CalculateGridSize();

        HeatMapInfo.Instance.MapHeight = (int)gridSize.y;

        for (int y = 0; y < gridSize.y; y++)
        {
            float sizeX = gridSize.x;

            if (y % 2 != 0 && (gridSize.x + 0.5f) * hexWidth > groundWidth)
                sizeX--;

            HeatMapInfo.Instance.MapWidth = (int)sizeX;

            for (int x = 0; x < sizeX; x++)
                AddTileToHexGrid(x, y);
        }
    }

    void AddTileToHexGrid(int x, int y)
    {
        GameObject hex = (GameObject)Instantiate(hexPrefab);
        Vector2 gridPos = new Vector2(x, y);
        hex.transform.position = CalculateWorldCoord(gridPos);
        hex.name = "Hex " + x + " " + y;
        hex.transform.parent = HexGridObject.transform;

        var hexObjectComp = hex.AddComponent<HexObject>();
        hexObjectComp.Index = y * Grid.Instance.MapWidth + x;

        hexGridObjects.Add(hexObjectComp);

        HeatMapInfo.Instance.AddToTileMap(x, y);
    }

    void SetTerrainPotential()
    {
        int indexInList = 0;
        // TODO: Set up terrain checks for collision with the tiles
        foreach (var hex in hexGridObjects)
        {
            GameObject model;
            if (model = hex.transform.FindChild("hex_tile").FindChild("Model").gameObject)
            {
                if (model.GetComponent<Collider>() == null)
                    model.AddComponent<Collider>();

                var collide = Physics.OverlapSphere(model.transform.position, model.GetComponent<MeshFilter>().mesh.bounds.extents.x, layerMask);

                if (collide.Length > 0)
                {
                    HeatMapInfo.Instance.TileMap[indexInList].potential[LayerType.Terrain] = float.MinValue;
                    hex.transform.FindChild("hex_tile").GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                }
            }
            indexInList++;
        }
    }

    void SetFlowerPotential()
    {
        // Hex List without the terrain tiles so we can ignore them
        var newHexList = hexGridObjects.Where(x => HeatMapInfo.Instance.TileMap[x.Index].potential[LayerType.Terrain] != float.MinValue);
    }

    [Obsolete("Pass in the LayerType also")]
    public static void AddBuildingToHeatMap(int index)
    {
        HeatMapInfo.Instance.TileMap[index].potential[LayerType.None] = float.MinValue;
    }

    [Obsolete("Use Grid.Instance")]
    public static void AddBuildingToHeatMap(int index, LayerType buildingType)
    {
        HeatMapInfo.Instance.TileMap[index].potential[buildingType] = 1;

        float linearFalloff = 1 / ((Grid.Instance.MapWidth > Grid.Instance.MapHeight) ? Grid.Instance.MapWidth : Grid.Instance.MapHeight);

        // Hex List without the terrain tiles so we can ignore them
        var newHexList = hexGridObjects.Where(x => HeatMapInfo.Instance.TileMap[x.Index].potential[LayerType.Terrain] != float.MinValue);

        Debug.Log(hexGridObjects.Count());

        foreach (var hex in hexGridObjects) {
            float distLinear = 1 - (Hex.Distance(hexGridObjects[index].hex, hex.hex) * linearFalloff);
            HeatMapInfo.Instance.TileMap[hex.Index].potential[buildingType] = distLinear;

            hex.transform.FindChild("hex_tile").GetComponentInChildren<MeshRenderer>().material.color = new Color(0, distLinear, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(new Vector3(-groundWidth / 2 + hexWidth, 0, groundHeight / 2 - hexWidth / 2), .2f);
    }
}