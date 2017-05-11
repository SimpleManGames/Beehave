using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contains information about a hex grid and helper functions
/// </summary>
public class Grid : Singleton<Grid>
{
    public GameObject hexObjectPrefab = null;
    public GameObject thronePrefab = null;

    public LayerMask terrainLayerMask;
    public LayerMask hexLayerMask;

    #region Properties

    private Renderer hexObjectRenderer;
    /// <summary>
    /// Returns a Renderer component two children down from the hexObjectPrefab
    /// </summary>
    public Renderer HexObjectRenderer
    {
        get
        {
            if (hexObjectRenderer == null && hexObjectPrefab != null)
                hexObjectRenderer = hexObjectPrefab.transform.FindChild("hex_tile").GetComponentInChildren<Renderer>();

            return hexObjectRenderer;
        }
    }

    private MeshFilter hexObjectFilter;
    /// <summary>
    /// Returns a MeshFilter component two children down form the hexObjectPrefab
    /// </summary>
    public MeshFilter HexObjectFilter
    {
        get
        {
            if (hexObjectFilter == null && hexObjectPrefab != null)
                hexObjectFilter = hexObjectPrefab.transform.FindChild("hex_tile").GetComponentInChildren<MeshFilter>();

            return hexObjectFilter;
        }
    }

    private float hexWidth = -1337;
    /// <summary>
    /// The width of the Renderer for hexObjectPrefab
    /// </summary>
    public float HexWidth
    {
        get
        {
            if (hexWidth == -1337 && HexObjectRenderer != null)
                hexWidth = HexObjectRenderer.bounds.size.x;

            return hexWidth;
        }
    }
    private float hexHeight = -1337;
    /// <summary>
    /// The height of the Renderer for hexObjectPrefab
    /// </summary>
    public float HexHeight
    {
        get
        {
            if (hexHeight == -1337 && HexObjectRenderer != null)
                hexHeight = HexObjectRenderer.bounds.size.z;

            return hexHeight;
        }

        set
        {
            hexHeight = value;
        }
    }

    private List<HexObject> hexes = new List<HexObject>();
    /// <summary>
    /// Handles the list of hexes
    /// </summary>
    public List<HexObject> Hexes
    {
        get { return hexes; }
        set { hexes = value; }
    }

    [SerializeField]
    [Tooltip("The wanted width for the map")]
    private int mapWidth;
    /// <summary>
    /// The desired width of the map
    /// </summary>
    public int MapWidth
    {
        get { return mapWidth; }
        set { mapWidth = value; }
    }

    [SerializeField]
    [Tooltip("The wanted height for the map")]
    private int mapHeight;
    /// <summary>
    /// The desired height of the map
    /// </summary>
    public int MapHeight
    {
        get { return mapHeight; }
        set { mapHeight = value; }
    }

    #endregion

    #region Privates

    private Vector3 CalculateInitPosition()
    {
        return new Vector3(-MapWidth / 2 + HexWidth, 0, MapHeight / 2 - HexHeight / 2);
    }

    /// <summary>
    /// The top function for creating the Hex Map
    /// </summary>
    private void CreateGrid()
    {
        Vector2 gridSize = CalculateGridSize();

        HeatMapInfo.Instance.MapHeight = (int)gridSize.y;

        int index = 0;

        for (int y = 0; y < gridSize.y; y++)
        {
            float sizeX = gridSize.x;
            if (y % 2 != 0 && (gridSize.x + 0.5f) * HexWidth > MapWidth)
                sizeX--;

            HeatMapInfo.Instance.MapWidth = (int)sizeX;

            for (int x = 0; x < sizeX; x++)
            {
                AddTileToHexGrid(new OffsetCoord(x, y), index);
                index++;
            }
        }
    }

    /// <summary>
    /// Calculates the size of the map in hexes based on 'hexObjectPrefab'
    /// </summary>
    /// <returns>Vector2 holding info about the size of the map in hexes</returns>
    private Vector2 CalculateGridSize()
    {
        float sideLength = HexHeight / 2;
        int numOfSides = (int)(MapHeight / sideLength);
        int gridHeightInHexes = (int)(numOfSides * 2 / 3);

        if (gridHeightInHexes % 2 == 0 && (numOfSides + 0.5f) * sideLength > MapHeight)
            gridHeightInHexes--;

        return new Vector2((int)(MapWidth / HexWidth), gridHeightInHexes);
    }

    /// <summary>
    /// Creates and modifies its cube coordinates; Then adds a new tile to the Hex Map
    /// </summary>
    /// <param name="coord">The [X, Y] position of where to place the new tile</param>
    private void AddTileToHexGrid(OffsetCoord coord, int index)
    {
        GameObject newHex = (GameObject)Instantiate(hexObjectPrefab);
        Vector2 gridPos = new Vector2(coord.Column, coord.Row);

        newHex.name = "Hex " + coord.Column + " " + coord.Row;
        newHex.transform.position = transform.position + CalculateWorldCoord(gridPos);
        newHex.transform.parent = transform;

        HexObject newHexObject = newHex.GetComponent<HexObject>();
        newHexObject.Index = index /*coord.Row * Grid.Instance.MapWidth + coord.Column*/;
        newHexObject.hex = new Hex(CubeCoord.OddRowToCube(coord));
        Hexes.Add(newHexObject);

        HeatMapInfo.Instance.AddToTileMap(index);
    }

    /// <summary>
    /// Calculates the locate a tile will be placed at in world space
    /// </summary>
    /// <param name="pos">Its [X, Y] position in a Offset coordinate system</param>
    /// <returns></returns>
    private Vector3 CalculateWorldCoord(Vector2 pos)
    {
        Vector3 initPos = CalculateInitPosition();
        float offset = 0;
        if (pos.y % 2 != 0)
            offset = HexWidth / 2;

        float x = initPos.x + offset + pos.x * HexWidth;
        float z = initPos.z - pos.y * hexHeight * 0.75f;

        return new Vector3(x, transform.position.y, z);
    }

    /// <summary>
    /// Checks agaisnt all hex's colliders for Terrain and sets the potential of the hex
    /// </summary>
    private void SetStartupPotentials()
    {
        int indexInList = 0;
        foreach (var hex in Hexes)
        {
            GameObject model;
            if (model = hex.transform.FindChild("hex_tile").FindChild("Model").gameObject)
            {
                if (model.GetComponent<Collider>() == null)
                    model.AddComponent<Collider>();

                var collide = Physics.OverlapSphere(model.transform.position, .5f, terrainLayerMask);

                if (collide.Length > 0)
                    HeatMapInfo.Instance.TileMap[indexInList].potential[LayerType.Terrain] = float.MinValue;
            }
            indexInList++;
        }
    }

    void SetUpPollen()
    {
        List<GameObject> flowers = GameObject.FindGameObjectsWithTag("Pollen Spot").ToList();

        flowers.ForEach(obj =>
        {
            Vector3 startPoint = obj.GetComponent<Collider>().bounds.center + new Vector3(0, obj.GetComponent<Collider>().bounds.extents.y, 0);
            RaycastHit hit;
            if (Physics.Raycast(new Ray(startPoint, Vector3.down), out hit, 5f, hexLayerMask))
            {
                var collide = hit.transform.gameObject;
                HeatMapInfo.Instance.TileMap[collide.GetComponentInParent<HexObject>().Index].potential[LayerType.Pollen] = 1;

                var editList = Instance.Hexes.Where(h => HeatMapInfo.Instance.TileMap[h.Index].potential[LayerType.Terrain] == 0 && HeatMapInfo.Instance.TileMap[h.Index].potential[LayerType.Pollen] != 1);
                editList.ToList().ForEach(edit =>
                {
                    float maxDistance = float.MinValue;
                    Instance.Hexes.ForEach(h =>
                    {
                        if (Hex.Distance(h.hex, edit.hex) > maxDistance)
                            maxDistance = Hex.Distance(h.hex, edit.hex);
                    });

                    float distLinear = 1 - (Hex.Distance(Instance.Hexes[collide.GetComponentInParent<HexObject>().Index].hex, edit.hex) / maxDistance);

                    try
                    {
                        if (HeatMapInfo.Instance.TileMap[edit.Index].potential[LayerType.Pollen] < distLinear)
                            HeatMapInfo.Instance.TileMap[edit.Index].potential[LayerType.Pollen] = distLinear;
                    }
                    catch
                    {
#if UNITY_EDITOR
                        Debug.Log("Null: " + edit.Index);
#endif
                    }
                });
            }
        });


    }

    public static void EditHeatMapData(int index, LayerType layer)
    {
        Instance.StartCoroutine(CoEditHeatMapData(index, layer));
    }

    public static IEnumerator CoEditHeatMapData(int index, LayerType layer)
    {
        var editedList = Instance.Hexes.Where(h => HeatMapInfo.Instance.TileMap[h.Index].potential[LayerType.Terrain] == 0 && HeatMapInfo.Instance.TileMap[h.Index].potential[layer] != 1);

        HeatMapInfo.Instance.TileMap[index].potential[layer] = 1;

        foreach (var hex in editedList)
        {
            float maxDistance = float.MinValue;
            Instance.Hexes.ForEach(h =>
            {
                if (Hex.Distance(hex.hex, h.hex) > maxDistance)
                    maxDistance = Hex.Distance(hex.hex, h.hex);
            });

            float distLinear = 1 - (Hex.Distance(Instance.hexes[index].hex, hex.hex) / maxDistance);
            try
            {
                if (HeatMapInfo.Instance.TileMap[hex.Index].potential[layer] < distLinear)
                    HeatMapInfo.Instance.TileMap[hex.Index].potential[layer] = distLinear;
            }
            catch
            {
#if UNITY_EDITOR
                Debug.Log(hex.Index);
#endif
            }

            hex.transform.FindChild("Canvas").GetComponentInChildren<DebugLayerValue>().Change();

            if (hex.Index % 20 == 0)
                yield return null;
        }
    }

    private void SetStartTiles(int index = 302)
    {
        Hexes[index].IsCreep = true;
        RaycastHit hit;
        Physics.Raycast(FindHexObject(Hexes[index].hex.cubeCoords).transform.position - Vector3.up, Vector3.down, out hit, 2f);

        GameObject throne = Instantiate(thronePrefab);
        throne.transform.position = new Vector3(FindHexObject(Hexes[index].hex.cubeCoords).transform.position.x, hit.point.y, FindHexObject(Hexes[index].hex.cubeCoords).transform.position.z);

        EditHeatMapData(index, LayerType.Throne);
        Hex.Neighbours(Hexes[index].hex).ToList().ForEach(h => FindHexObject(h.cubeCoords).IsCreep = true);
    }

    #endregion

    #region Statics

    /// <summary>
    /// Gets a HexObject based off it's index
    /// </summary>
    /// <param name="index">Index of the wanted HexObject</para>
    /// <returns>HexObject with the requested Index</returns>
    public static HexObject FindHexObject(int index)
    {
        return Instance.Hexes.Where(h => h.Index == index).FirstOrDefault();
    }

    /// <summary>
    /// Finds an HexObject component with the same cube coordinates
    /// </summary>
    /// <param name="c">The location of the tile you want in cubecoords</param>
    /// <returns>HexObject Component</returns>
    public static HexObject FindHexObject(CubeCoord c)
    {
        return FindHexObject(c.Q, c.R, c.S);
    }

    /// <summary>
    /// Finds an HexObject component with the same cube coorinates
    /// </summary>
    /// <param name="q">Represents the X position</param>
    /// <param name="r">Represents the Y position</param>
    /// <param name="s">Represents the Z position</param>
    /// <returns>HexObject Component</returns>
    public static HexObject FindHexObject(double q, double r, double s)
    {
        return Instance.Hexes.Where(t => t.hex.cubeCoords.Q == q && t.hex.cubeCoords.R == r && t.hex.cubeCoords.S == s).FirstOrDefault();
    }

    /// <summary>
    /// Gets all the Hexes in a range around the center
    /// </summary>
    /// <param name="center">The Hex that will be checked around</param>
    /// <param name="range">The amount of tiles away to be included</param>
    /// <returns>A List of the Hexes</returns>
    public static List<Hex> HexesInRange(Hex center, int range)
    {
        List<Hex> ret = new List<Hex>();

        CubeCoord c;

        for (int dx = -range; dx <= range; dx++)
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                c = new CubeCoord(dx, dy, -dx - dy) + center.cubeCoords;
                if (Instance.Hexes.Contains(FindHexObject(c)))
                    ret.Add(new Hex(c));
            }

        return ret;
    }

    /// <summary>
    /// Gets all the Hexes in a range around the coord
    /// </summary>
    /// <param name="coord">The position in Cube Coord to check around</param>
    /// <param name="range">The amount of tiles away to be included</param>
    /// <returns>A List of the Hexes</returns>
    public static List<Hex> HexesInRange(CubeCoord coord, int range)
    {
        return HexesInRange(new Hex(coord), range);
    }

    /// <summary>
    /// Gets all the Hexes in a range around the position in cube coord
    /// </summary>
    /// <param name="q">X value in a cube coord</param>
    /// <param name="r">Y value in a cube coord</param>
    /// <param name="s">Z value in a cube coord</param>
    /// <param name="range">The amount of tiles away to be included</param>
    /// <returns></returns>
    public static List<Hex> HexesInRange(double q, double r, double s, int range)
    {
        return HexesInRange(new Hex(q, r, s), range);
    }

    #endregion

    #region Unity Functions

    public void Start()
    {
        CreateGrid();
        SetStartupPotentials();
        SetUpPollen();
        SetStartTiles();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + CalculateInitPosition() + new Vector3(MapWidth / 2 - (HexWidth / 2), 0, -MapHeight / 2 + (HexHeight / 2)), new Vector3(MapWidth, .5f, MapHeight));

        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(HexObjectFilter.sharedMesh, transform.position + CalculateInitPosition(), Quaternion.Euler(0, 90, 0));
    }

    #endregion
}