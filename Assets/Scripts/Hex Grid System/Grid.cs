using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contains information about a hex grid and helper functions
/// </summary>
public class Grid : Singleton<Grid>
{
    [SerializeField]
    [Tooltip("Prefabs of the tiles to be randomly selected during generation")]
    private List<GameObject> hexPrefabs = new List<GameObject>();

    [SerializeField]
    [Tooltip("The building that will be spawned at generation as the main hub")]
    public GameObject thronePrefab = null;

    [SerializeField]
    [Tooltip("Layer Mask that the terrain use")]
    private LayerMask terrainLayerMask;

    [SerializeField]
    [Tooltip("Layer Mask that the hexes use")]
    public LayerMask hexLayerMask;

    #region Properties

    private Renderer hexObjectRenderer;
    /// <summary>
    /// Returns a Renderer component two children down from the hexPrefabs[0]
    /// </summary>
    public Renderer HexObjectRenderer
    {
        get
        {
            if (hexObjectRenderer == null && hexPrefabs[0] != null)
                hexObjectRenderer = hexPrefabs[0].transform.FindChild("hex_tile").GetComponentInChildren<Renderer>();

            return hexObjectRenderer;
        }
    }

    private MeshFilter hexObjectFilter;
    /// <summary>
    /// Returns a MeshFilter component two children down fromm the hexPrefabs[0]
    /// </summary>
    public MeshFilter HexObjectFilter
    {
        get
        {
            if (hexObjectFilter == null && hexPrefabs[0] != null)
                hexObjectFilter = hexPrefabs[0].transform.FindChild("hex_tile").GetComponentInChildren<MeshFilter>();

            return hexObjectFilter;
        }
    }

    private float hexWidth = -1337;
    /// <summary>
    /// The width of the Renderer for hexPrefabs[0]
    /// </summary>
    public float HexWidth
    {
        get
        {
            if (hexWidth == -1337 && HexObjectRenderer != null)
                hexWidth = HexObjectRenderer.bounds.size.x;

            return hexWidth;
        }
        set
        {
            hexWidth = value;
        }
    }
    private float hexHeight = -1337;
    /// <summary>
    /// The height of the Renderer for hexPrefabs[0]
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

    private HashSet<HexObject> hexes = new HashSet<HexObject>();
    /// <summary>
    /// Handles the list of hexes
    /// </summary>
    public HashSet<HexObject> Hexes
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

    #region Generation

    /// <summary>
    /// Figures out the position of the corner
    /// </summary>
    /// <returns>Vector3 were it's the top left corner</returns>
    private Vector3 CalculateInitPosition()
    {
        return new Vector3(-MapWidth / 2 + HexWidth, 0, MapHeight / 2 - HexHeight / 2);
    }

    /// <summary>
    /// Figures out the size of the map in hexes
    /// </summary>
    /// <returns></returns>
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
    /// Figures out the world position
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <returns>Vector3 were it's world coord is at</returns>
    private Vector3 CalculateWorldPosition(float x, float y)
    {
        return CalculateWorldPosition(new Vector2(x, y));
    }

    /// <summary>
    /// Figures out the world position
    /// </summary>
    /// <param name="coord">The local offset coord</param>
    /// <returns>Vector3 were it's world coord is at</returns>
    private Vector3 CalculateWorldPosition(OffsetCoord coord)
    {
        return CalculateWorldPosition(coord.Column, coord.Row);
    }


    /// <summary>
    /// Figures out the world position
    /// </summary>
    /// <param name="position">The local position</param>
    /// <returns>Vector3 were it's world coord is at</returns>
    private Vector3 CalculateWorldPosition(Vector2 position)
    {
        Vector3 initPos = CalculateInitPosition();
        float offset = 0;
        if (position.y % 2 != 0)
            offset = HexWidth / 2;

        float x = initPos.x + offset + position.x * HexWidth;
        float z = initPos.z - position.y * HexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    /// <summary>
    /// Instantiates a new tile at the coord passed in
    /// </summary>
    /// <param name="coord">The coord to be used</param>
    private void PlaceTile(OffsetCoord coord)
    {
        // Create the object
        var hexPrefabObject = hexPrefabs[Random.Range(0, hexPrefabs.Count)];
        GameObject hex = Instantiate(
            hexPrefabObject,
            transform.position + CalculateWorldPosition(new Vector2(coord.Column, coord.Row)),
            Quaternion.Euler(new Vector3(0, Random.Range(0, 5) * 60, 0)),
            this.transform);

        hex.name = string.Format("Hex {0} {1}", coord.Column, coord.Row);

        // Set up the object
        HexObject hexObject = hex.GetComponent<HexObject>();
        hexObject.hex = new Hex(CubeCoord.OddRowToCube(coord));
        hexObject.Index = Instance.Hexes.Count;
        Instance.Hexes.Add(hexObject);
    }

    /// <summary>
    /// Main function for creating the grid of hexes
    /// </summary>
    private void CreateGrid()
    {
        Vector2 gridSize = CalculateGridSize();

        for (int y = 0; y < gridSize.y; y++)
        {
            float sizeX = gridSize.x;

            if (y % 2 != 0 && (gridSize.x + 0.5f) * HexWidth > MapWidth)
                sizeX--;

            for (int x = 0; x < sizeX; x++)
                PlaceTile(new OffsetCoord(x, y));
        }
    }

    #endregion

    [System.Obsolete]
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
                HeatMapInfo.Instance.Field[LayerType.Pollen][collide.GetComponentInParent<HexObject>().Index] = 1;

                var editList = Instance.Hexes.Where(h => HeatMapInfo.Instance.Field[LayerType.Terrain][h.Index] == 0 && HeatMapInfo.Instance.Field[LayerType.Pollen][h.Index] != 1);
                editList.ToList().ForEach(edit =>
                {
                    float maxDistance = float.MinValue;
                    Instance.Hexes.ToList().ForEach(h =>
                    {
                        if (Hex.Distance(h.hex, edit.hex) > maxDistance)
                            maxDistance = Hex.Distance(h.hex, edit.hex);
                    });

                    float distLinear = 1 - (Hex.Distance(Instance.Hexes.ElementAt(collide.GetComponentInParent<HexObject>().Index).hex, edit.hex) / maxDistance);

                    try
                    {
                        if (HeatMapInfo.Instance.Field[LayerType.Pollen][edit.Index] < distLinear)
                            HeatMapInfo.Instance.Field[LayerType.Pollen][edit.Index] = distLinear;
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

    [System.Obsolete]
    public static void EditHeatMapData(int index, LayerType layer)
    {
        Instance.StartCoroutine(CoEditHeatMapData(index, layer));
    }

    [System.Obsolete]
    public static IEnumerator CoEditHeatMapData(int index, LayerType layer)
    {
        var editedList = Instance.Hexes.Where(h => HeatMapInfo.Instance.Field[LayerType.Terrain][h.Index] == 0 && HeatMapInfo.Instance.Field[layer][h.Index] != 1);

        HeatMapInfo.Instance.Field[layer][index] = 1;

        foreach (var hex in editedList)
        {
            float maxDistance = float.MinValue;
            Instance.Hexes.ToList().ForEach(h =>
            {
                if (Hex.Distance(hex.hex, h.hex) > maxDistance)
                    maxDistance = Hex.Distance(hex.hex, h.hex);
            });

            float distLinear = 1 - (Hex.Distance(Instance.Hexes.ElementAt(index).hex, hex.hex) / maxDistance);
            try
            {
                if (HeatMapInfo.Instance.Field[layer][hex.Index] < distLinear)
                    HeatMapInfo.Instance.Field[layer][hex.Index] = distLinear;
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

    /// <summary>
    /// Spawns 7 tiles around the index passed in and then places a throne building the index hex
    /// </summary>
    /// <param name="index">The middle hex index</param>
    private void SetStartTiles(int index = 302)
    {
        Hexes.ElementAt(index).IsCreep = true;
        RaycastHit hit;
        Physics.Raycast(FindHexObject(Hexes.ElementAt(index).hex.cubeCoords).transform.position - Vector3.up, Vector3.down, out hit, 2f);

        GameObject throne = Instantiate(thronePrefab);
        throne.transform.position = new Vector3(FindHexObject(Hexes.ElementAt(index).hex.cubeCoords).transform.position.x, hit.point.y, FindHexObject(Hexes.ElementAt(index).hex.cubeCoords).transform.position.z);

        Hex.Neighbours(Hexes.ElementAt(index).hex).ToList().ForEach(h => FindHexObject(h.cubeCoords).IsCreep = true);
    }

    #endregion

    #region Statics

    /// <summary>
    /// Gets a HexObject based off of it's CubeCoords in the hex that's passed in
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static HexObject FindHexObject(Hex hex)
    {
        return FindHexObject(hex.cubeCoords);
    }

    /// <summary>
    /// Gets a HexObject based off it's index
    /// </summary>
    /// <param name="index">Index of the wanted HexObject</para>
    /// <returns>HexObject with the requested Index</returns>
    public static HexObject FindHexObject(int index)
    {
        //return Instance.Hexes.Where(h => h.Index == index).FirstOrDefault();
        return Instance.Hexes.ToList().Find(h => h.Index == index);
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