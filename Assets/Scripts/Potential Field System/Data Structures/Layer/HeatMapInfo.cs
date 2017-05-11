using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeatMapInfo : Singleton<HeatMapInfo>
{
    /// <summary>
    /// Private Member Variable that holds a 1D array of TileData
    /// </summary>
    private TileData[] m_tileMap;
    /// <summary>
    /// Property for the an array of TileData
    /// </summary>
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

    private int m_MapWidth;
    /// <summary>
    /// Property containing information about the width of the map
    /// </summary>
    public int MapWidth
    {
        get { return m_MapWidth; }
        set { m_MapWidth = value; }
    }
    private int m_MapHeight;
    /// <summary>
    /// Property containing information about the height of the map
    /// </summary>
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
    public void AddToTileMap(int x, int y)
    {
        TileMap[y * MapWidth + x] = new TileData(y * MapWidth + x);
    }

    public void AddToTileMap(int index)
    {
        TileMap[index] = new TileData(index);
    }

    #region Obsolete


    [Obsolete]
    public HeatMapInteraction heatMapInteraction;

    /// <summary>
    /// Gets the 6 tiles surrounding the index'd tile
    /// </summary>
    /// <param name="index">The index of the tile you want to know about</param>
    /// <returns>
    /// If found a matching index; returns the TileData for it.
    /// Else; returns null
    /// </returns>

    [Obsolete]
    public TileData GetTileData(int index)
    {
        for (int y = 0; y < MapHeight; y++)
            for (int x = 0; x < MapWidth; x++)
                if (TileMap[y * m_MapWidth + x].index == index)
                    return TileMap[y * m_MapWidth + x];

        return new TileData(int.MinValue);
    }

    /// <summary> 
    /// Returns the neighbors of the tile index passed in
    /// </summary>
    /// <param name="currentIndex">The tile's index that you want to check around</param>
    /// <returns>An array of 6 TileData's</returns>
    [Obsolete]
    public TileData[] AdjacenyList(int currentIndex)
    {
        return new TileData[6] {
            GetTileData(currentIndex - 1),
            GetTileData(currentIndex + 1),
            GetTileData(currentIndex + MapWidth), // This is what's giving the index 16 when on tile 6 for adjacency
            GetTileData(currentIndex + MapWidth - 1),
            GetTileData(currentIndex - MapWidth),
            GetTileData(currentIndex - MapWidth + 1) };
    }

    [Obsolete]
    public struct LayerData
    {
        /// <summary>
        /// 1D Array holding the float values for the heatmap
        /// </summary>
        public float[,] data;

        public LayerData(float[,] data)
        {
            this.data = data;
        }
    }
    [Obsolete]
    private Dictionary<LayerType, LayerData> m_HeatMap;
    [Obsolete]
    /// <summary>
    /// To set new heatmap values, pass in a new Dictionary with the changed values and sloths
    /// </summary>
    public Dictionary<LayerType, LayerData> HeatMap
    {
        get
        {
            if (m_HeatMap == null)
            {
                m_HeatMap = new Dictionary<LayerType, LayerData>(Enum.GetValues(typeof(LayerType)).Length);
                GenerateBaseHeatMap();
            }

            return m_HeatMap;
        }
        set
        {
            for (int i = 0; i < value.Count; i++)
            {
                var currentLayerEnum = value.Keys.ElementAt(i);
                if (m_HeatMap.ContainsKey(currentLayerEnum))
                    m_HeatMap[currentLayerEnum] = new LayerData(value[currentLayerEnum].data);
            }
        }
    }
    [Obsolete]
    private void GenerateBaseHeatMap()
    {
        for (int i = 0; i < HeatMap.Count; i++)
        {
            m_HeatMap[(LayerType)i] = new LayerData(new float[MapWidth, MapHeight]);
        }
    }
    #endregion
}