using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UtilityType
{
    Null = 0, Hunger = 1, Sleep = 2, Pollen = 3, Rest = 4, Honey = 5
}

// TODO: Add templating sugar using Enums to this for Utility Type
public class Utility<type>
{
    public Utility(int _transferRate, type _utilityType, UtilityType _stored, Tasks _grow, Tasks _deposit,float _startWeight = 0, float _growthRate = 0, bool _stat = false )
    {
        _currentweight = _startWeight;
        minWeight = 0;
        maxWeight = 100;

        transferRate = _transferRate;
        growthRate = _growthRate;

        utilityType = _utilityType;

        storedUtility = _stored;

        growTask = _grow;
        depositTask = _deposit;

        isStat = _stat;
    }

    public bool isStat { get; private set; }

    private float _currentweight;
    public float currentweight { get { return _currentweight; } set { _currentweight = Mathf.Clamp(value, minWeight, maxWeight); } }

    public float maxWeight { get; private set; }
    public float minWeight { get; private set; }

    private float growthRate;
    public float transferRate { get; private set; }

    public UtilityType storedUtility { get; private set; }

    public Tasks growTask { get; private set; }
    public Tasks depositTask { get; private set; }

    public type utilityType;

    // Stat Utilities
    // Utility<UtilityType> hungerUtility = new Utility<UtilityType>(0, UtilityType.Hunger, UtilityType.Pollen, Tasks.Eat, Tasks.Null, 100, -5, true);
    // Utility<UtilityType> sleepUtility = new Utility<UtilityType>(0, UtilityType.Sleep, UtilityType.Rest, Tasks.Sleep, Tasks.Null, 100, -1, true);

    // Utility<UtilityType> pollenUtility = new Utility<UtilityType>(2, UtilityType.Pollen, UtilityType.Pollen, Tasks.Null, Tasks.StorePollen, 0, 1);
    // Utility<UtilityType> restUtility = new Utility<UtilityType>(2, UtilityType.Rest, UtilityType.Rest, Tasks.Null, Tasks.Null, 0, 2);

    public void transferWeight(Utility<UtilityType> appliedUtility)
    {
        float amtToTransfer;

        amtToTransfer = appliedUtility.transferRate;
        if(appliedUtility.transferRate > appliedUtility.currentweight)
        {
            amtToTransfer = appliedUtility.currentweight;
        }
        appliedUtility.currentweight -= amtToTransfer;
        this.currentweight += amtToTransfer;
    }

    public void growWeight()
    {
        if (currentweight <= maxWeight && currentweight >= minWeight)
        {
            currentweight += growthRate;
        }
    }

    public void fakeThatShit(int supahCheats)
    {
        if(currentweight <= maxWeight && currentweight >= minWeight)
        {
            currentweight += supahCheats;
        }
    }

    // NOTE: Utilities will be treated as a storage container where weight is what's being stored and can be transfered between them but...
    // ...each utility will only accept weight from specified utilities (Decided upon at creation)
}