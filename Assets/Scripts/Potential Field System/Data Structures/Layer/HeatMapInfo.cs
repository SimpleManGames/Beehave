using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeatMapInfo : Singleton<HeatMapInfo>
{
    /// <summary>
    /// Represents how to apply the data to the heat map
    /// </summary>
    public enum DisperseSetting { None, Linear, MinValue }

    /// <summary>
    /// Wrapper struct for passing info about how to change the heat map
    /// </summary>
    public struct LayerSettings
    {
        /// <summary>
        /// The LayerType we are going to change
        /// </summary>
        public LayerType type;
        /// <summary>
        /// How we are going to modify the data
        /// </summary>
        public DisperseSetting settings;
    }

    /// <summary>
    /// A Dictionary that holds all the LayerTypes and their float array
    /// </summary>
    private SortedDictionary<LayerType, float[]> field = new SortedDictionary<LayerType, float[]>();
    public SortedDictionary<LayerType, float[]> Field { get { return field; } }

    /// <summary>
    /// A Dictionary we use to do the first setup when loading the map
    /// </summary>
    public Dictionary<LayerMask, LayerSettings> fieldSetupDictionary = new Dictionary<LayerMask, LayerSettings>();

    public override void Awake()
    {
        base.Awake();
        fieldSetupDictionary.Add(
            LayerMask.NameToLayer("Terrain"),
            new LayerSettings
            {
                type = LayerType.Terrain,
                settings = DisperseSetting.MinValue
            });

        fieldSetupDictionary.Add(
            LayerMask.NameToLayer("Flower"),
            new LayerSettings
            {
                type = LayerType.Pollen,
                settings = DisperseSetting.Linear
            });

        foreach (var type in Enum.GetValues(typeof(LayerType)))
            field.Add((LayerType)type, new float[Grid.Instance.MapWidth * Grid.Instance.MapHeight]);
    }

    /// <summary>
    /// Calculates the linear falloff at the Hex on the LayerType passed in
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="type"></param>
    public void CalculateLinear(Hex hex, LayerType type)
    {
        CalculateLinear(Grid.FindHexObject(hex), type);
    }

    /// <summary>
    /// Calculates the linear falloff at the HexObject on the LayerType passed in
    /// </summary>
    /// <param name="hexObject"></param>
    /// <param name="type"></param>
    public void CalculateLinear(HexObject hexObject, LayerType type)
    {
        var list = Grid.Instance.Hexes.Where(h => Instance.Field[LayerType.Terrain][h.Index] == 0);

        Instance.Field[type][hexObject.Index] = 1;

        foreach (var hex in Grid.Instance.Hexes)
        {
            float maxDistance = float.MinValue;
            Grid.Instance.Hexes.ForEach(h => {
                var dist = Hex.Distance(hexObject.hex, h.hex);
                if (dist > maxDistance)
                    maxDistance = dist;
            });

            float linearDistance = 1 - (Hex.Distance(hexObject.hex, hex.hex) / maxDistance);

            if (Instance.Field[type][hex.Index] < linearDistance)
                Instance.Field[type][hex.Index] = linearDistance;
        }
    }

    /// <summary>
    /// Runs a Coroutine to edit the heat map at the Hex with LayerSettings passed in
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    private IEnumerator CoCalculateLinear(Hex hex, LayerSettings settings)
    {
        const float batchCount = 20;
        float currentBatchCount = 0;

        float maxDistance = float.MinValue;

        foreach (var other in Grid.Instance.Hexes)
        {
            float dist = Hex.Distance(other.hex, hex);
            if (dist > maxDistance)
                maxDistance = dist;

            currentBatchCount++;
            if (currentBatchCount >= batchCount)
            {
                currentBatchCount = 0;
                yield return null;
            }
        }

        currentBatchCount = 0;

        foreach (var other in Grid.Instance.Hexes)
        {
            float linearDistance = 1 - (Hex.Distance(other.hex, hex) / maxDistance);

            if (Instance.Field[settings.type][other.Index] < linearDistance)
                Instance.Field[settings.type][other.Index] = linearDistance;

            currentBatchCount++;
            if (currentBatchCount >= batchCount)
            {
                currentBatchCount = 0;
                yield return null;
            }
        }

        currentBatchCount = 0;
        yield return null;
    }

    /// <summary>
    /// Edits the heat map data at the Index with the LayerSettings passed in
    /// </summary>
    /// <param name="index"></param>
    /// <param name="settings"></param>
    public void AddPointToLayer(int index, LayerSettings settings)
    {
        var list = Grid.Instance.Hexes.Where(h => Instance.Field[LayerType.Terrain][h.Index] == 0);

        Instance.Field[settings.type][index] = 1;

        foreach (var hex in list)
            CalculateLinear(Grid.FindHexObject(index), settings.type);
    }

    /// <summary>
    /// Edits the heat map data at the HexObject with the LayerSettings passed in
    /// </summary>
    /// <param name="hexObject"></param>
    /// <param name="settings"></param>
    public void AddPointToLayer(HexObject hexObject, LayerSettings settings)
    {
        switch (settings.settings)
        {
            case DisperseSetting.None:
                break;
            case DisperseSetting.Linear:
                CalculateLinear(hexObject, settings.type);
                break;
            case DisperseSetting.MinValue:
                break;
            default:
                break;
        }
    }
}