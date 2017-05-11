using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInfo
{
    // Bee Age/Building Occupancy
    private int _currentCapacity;
    public int currentCapacity { get { return _currentCapacity; } private set { _currentCapacity = Mathf.Clamp(value, minCapacity, maxCapacity); } }

    public string name;

    public string physicalTrait;
    public string socialTrait;

    protected int maxCapacity;
    protected int minCapacity;

    public string bumblrText;

    public void genLivingAgentInfo()
    {
        name = BeeTraits.beeNames[Random.Range(0, BeeTraits.beeNames.Count) - 1];
        physicalTrait = BeeTraits.physicalTraits[Random.Range(0, BeeTraits.physicalTraits.Count - 1)];
        socialTrait = BeeTraits.socialTraits[Random.Range(0, BeeTraits.socialTraits.Count - 1)];

        minCapacity = 0;
        maxCapacity = Random.Range(0, 10);
        currentCapacity = 0;

        bumblrText = "Welcome to Bumblr";
    }

    public void genPollenBuilding()
    {
        name = "PollenStorage";
        physicalTrait = null;
        socialTrait = null;

        minCapacity = 0;
        maxCapacity = Random.Range(0, 10);
        currentCapacity = 0;

        //bumblrText = BeeTraits.buildingDescriptions[LayerType.Pollen_Storge];
    }

    public void genHoneyBuilding()
    {
        name = "HoneyBuilding";
        physicalTrait = null;
        socialTrait = null;

        minCapacity = 0;
        maxCapacity = Random.Range(0, 10);
        currentCapacity = 0;

        //bumblrText = BeeTraits.buildingDescriptions[LayerType.Honey_Storage];
    }

    public void genSpawner()
    {
        name = "Incubator";
        physicalTrait = null;
        socialTrait = null;

        minCapacity = 0;
        maxCapacity = Random.Range(0, 10);
        currentCapacity = 0;
    }
}
