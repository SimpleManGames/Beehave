using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyType { Hunger, Food, Energy, Rest, Pollen }

public class AgentProperty
{
    public PropertyType type { get; private set; }
    public int weight { get; private set; }
    private int maxWeight = 0;
    private int growthRate = 0;

    public AgentProperty(PropertyType _type, int _maxWeight, int growth = 0, int startWeight = 0)
    {
        type = _type;
        maxWeight = _maxWeight;
        weight = startWeight;
        growthRate = growth;
    }

    public void GrowWeight()
    {
        weight += Mathf.Min(maxWeight, (weight + growthRate));
    }

    public void TakeWeight(int amtToGive)
    {
        weight = Mathf.Min(maxWeight, (weight + amtToGive));
    }

    public int GiveWeight(int transferWeight)
    {
        int giveWeight = Mathf.Min(weight, transferWeight);
        weight -= giveWeight;
        return giveWeight;
    }
}
