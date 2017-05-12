using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeatMapInfo : Singleton<HeatMapInfo>
{
    public enum DisperseSetting { None, Linear, MinValue }

    public struct LayerSettings
    {
        public LayerType type;
        public DisperseSetting settings;
    }

    private SortedDictionary<LayerType, float[]> field = new SortedDictionary<LayerType, float[]>();
    public SortedDictionary<LayerType, float[]> Field { get { return field; } }

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

            if (Instance.Field[settings.type][otherHexObj.Index] < linearDistance)
                Instance.Field[settings.type][otherHexObj.Index] = linearDistance;
        }
    }

    public void AddPointToLayer(int index, LayerSettings settings)
    {
        var list = Grid.Instance.Hexes.Where(h => Instance.Field[LayerType.Terrain][h.Index] == 0);

        Instance.Field[settings.type][index] = 1;

        foreach (var hex in list)
            CalculateLinear(Grid.FindHexObject(index).hex, settings);
    }
}