using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeatMapInfo : Singleton<HeatMapInfo>
{
    /// <summary>
    /// Private Member Variable that holds a 1D array of TileData
    /// </summary>
    [System.Obsolete]
    private TileData[] m_tileMap;
    /// <summary>
    /// Property for the an array of TileData
    /// </summary>
    [System.Obsolete]
    public TileData[] TileMap
    {
        get
        {
            // If the tile map is not already init
            // Create it based on the generated map values
            if (m_tileMap == null)
                m_tileMap = new TileData[MapWidth * MapHeight];

            return m_tileMap;
        }
        set
        {
            // For all the values in value
            // Set the according index to the correct data
            for (int y = 0; y < value.GetLength(1); y++)
                for (int x = 0; x < value.GetLength(0); x++)
                    m_tileMap[y * m_MapWidth + x] = value[y * m_MapWidth + x];
        }
    }

    /// <summary>
    /// Holds the information about a single title
    /// </summary>
    [System.Obsolete]
    public struct TileData
    {
        /// <summary>
        /// Index in the array 
        /// </summary>
        public int index { get; private set; }

        /// <summary>
        /// Holds the data for all the layers of a single tile
        /// </summary>
        public Dictionary<LayerType, float> potential;

        /// <summary>
        /// TileData constructor
        /// </summary>
        /// <param name="index">y * m_MapWidth + x</param>
        public TileData(int index)
        {
            this.index = index;
            potential = new Dictionary<LayerType, float>(Enum.GetValues(typeof(LayerType)).Length);

            foreach (var type in Enum.GetValues(typeof(LayerType)))
                potential.Add((LayerType)type, 0);
        }
    }

    [System.Obsolete]
    private int m_MapWidth;
    /// <summary>
    /// Property containing information about the width of the map
    /// </summary>
    [System.Obsolete]
    public int MapWidth
    {
        get { return m_MapWidth; }
        set { m_MapWidth = value; }
    }
    [System.Obsolete]
    private int m_MapHeight;
    /// <summary>
    /// Property containing information about the height of the map
    /// </summary>
    [System.Obsolete]
    public int MapHeight
    {
        get { return m_MapHeight; }
        set { m_MapHeight = value; }
    }

    /// <summary>
    /// Adds a tile to the TileMap array by the x and y position when created by HexGridGenerator
    /// </summary>
    /// <param name="x">X position in the array when the hex was generated</param>
    /// <param name="y">Y position in the array when the hex was generated</param>
    /// <seealso cref="HexGridGenerator.cs"/>
    [System.Obsolete]
    public void AddToTileMap(int x, int y)
    {
        TileMap[y * MapWidth + x] = new TileData(y * MapWidth + x);
    }

    [System.Obsolete]
    public void AddToTileMap(int index)
    {
        TileMap[index] = new TileData(index);
    }

    //public enum LayerType { None, Terrain, Honey, Pollen, Sleep, Pollen_Storge, Honey_Storage, Nectar_Storage, Temple, Throne, Incubator }
    public enum DisperseSetting { None, Linear, MinValue }

    public struct LayerSettings
    {
        public LayerType type;
        public DisperseSetting settings;
    }

    private SortedDictionary<LayerType, float[]> field = new SortedDictionary<LayerType, float[]>();
    public SortedDictionary<LayerType, float[]> Field { get { return field; } }

    public Dictionary<LayerMask, LayerSettings> initFieldSetupDictionary = new Dictionary<LayerMask, LayerSettings>();

    public override void Awake()
    {
        base.Awake();
        initFieldSetupDictionary.Add(
            LayerMask.NameToLayer("Terrain"),
            new LayerSettings
            {
                type = LayerType.Terrain,
                settings = DisperseSetting.MinValue
            });

        initFieldSetupDictionary.Add(
            LayerMask.NameToLayer("Pollen"),
            new LayerSettings
            {
                type = LayerType.Pollen,
                settings = DisperseSetting.Linear
            });

        foreach (var type in Enum.GetValues(typeof(LayerType)))
            field.Add((LayerType)type, new float[Grid.Instance.MapWidth * Grid.Instance.MapHeight]);
    }

    public void CalculateLinear(Hex hex, LayerSettings settings)
    {
        float maxDistance = float.MinValue;

        foreach (var otherHexObj in Grid.Instance.Hexes)
        {
            if (Hex.Distance(otherHexObj.hex, hex) > maxDistance)
                maxDistance = Hex.Distance(otherHexObj.hex, hex);
        }

        foreach (var otherHexObj in Grid.Instance.Hexes)
        {
            float linearDistance = 1 - (Hex.Distance(otherHexObj.hex, hex) / maxDistance);

            if (HeatMapInfo.Instance.Field[settings.type][otherHexObj.Index] < linearDistance)
                HeatMapInfo.Instance.Field[settings.type][otherHexObj.Index] = linearDistance;
        }
    }
}