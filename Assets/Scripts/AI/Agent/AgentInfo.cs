using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInfo
{
    public string name { get; private set; }
    public string trait { get; private set; }
    public string description { get; private set; }
    public int capacity { get; private set; }

    public AgentInfo(AgentType attachedType)
    {
        if(attachedType == AgentType.Bee)
        {
            GenBeeInfo();
        }
        else
        {
            GenBuildingInfo(attachedType);
            trait = "BuildingsDontHaveTraits";
            capacity = Random.Range(0, 10);
        }
    }

    private void GenBeeInfo()
    {
        name = BeeTraits.beeNames[Random.Range(0, BeeTraits.beeNames.Count)];
        trait = BeeTraits.physicalTraits[Random.Range(0, BeeTraits.physicalTraits.Count)];
        description = null;
        capacity = Random.Range(1, 10);
    }

    private void GenBuildingInfo(AgentType buildingType)
    {
        switch(buildingType)
        {
            case AgentType.Food:
                name = "HoneyHub";
                description = BeeTraits.buildingDescriptions[LayerType.Honey];
                break;
            case AgentType.House:
                name = "Dwelling";
                description = BeeTraits.buildingDescriptions[LayerType.Sleep];
                break;
            case AgentType.Plant:
                name = "Flower";
                description = BeeTraits.buildingDescriptions[LayerType.Pollen];
                break;
            case AgentType.Storage:
                name = "PollenStorage";
                description = BeeTraits.buildingDescriptions[LayerType.Pollen_Storge];
                break;
            case AgentType.Incubator:
                name = "Incubator";
                description = "Boop";
                break;
        }
    }
}
