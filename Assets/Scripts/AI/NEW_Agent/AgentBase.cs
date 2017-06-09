using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentType { Bee, House, Storage, Plant, Food }

public class AgentBase : MonoBehaviour
{
    //Set in the Unity Editor based on the Prefab Type
    public AgentType type { get; private set; }
    public LayerType layerType { get; private set; }
    public int ID { get; private set; }
    public int currentTileIndex { get; private set; }
    List<AgentProperty> properties = new List<AgentProperty>();

    public void Start()
    {
        SetProperties();
        ID = Simulation.Instance.AddAgent(this);
    }

    public void GrowAgentProperties()
    {
        foreach(var property in properties)
        {
            property.GrowWeight();
        }
    }

    public AgentProperty GetPropertyofType(PropertyType wantedProperty)
    {
        foreach(var property in properties)
        {
            if(property.type == wantedProperty)
            {
                return property;
            }
        }

        Debug.Log("Expected AgentProperty not found");
        return null;
    }

    private void SetProperties()
    {
        switch(type)
        {
            case AgentType.Bee:
                properties.Add(new AgentProperty(PropertyType.Hunger, 100, -3));
                properties.Add(new AgentProperty(PropertyType.Energy, 100, -2));
                properties.Add(new AgentProperty(PropertyType.Pollen, 100, 0));
                layerType = LayerType.None;
                break;
            case AgentType.Storage:
                properties.Add(new AgentProperty(PropertyType.Pollen, 10000));
                layerType = LayerType.Pollen_Storge;
                break;
            case AgentType.House:
                properties.Add(new AgentProperty(PropertyType.Rest, 1000, 10, 1000));
                layerType = LayerType.Pollen_Storge;
                break;
            case AgentType.Food:
                properties.Add(new AgentProperty(PropertyType.Food, 1000, 4, 100));
                layerType = LayerType.Pollen_Storge;
                break;
            case AgentType.Plant:
                properties.Add(new AgentProperty(PropertyType.Pollen, 1000, 10, 1000));
                layerType = LayerType.Pollen_Storge;
                break;
        }
    }

    public void SetCurrentTile(int tileIndex)
    {
        currentTileIndex = tileIndex;
    }
}
