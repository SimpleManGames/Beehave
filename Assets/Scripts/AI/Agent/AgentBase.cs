using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum AgentType { Bee, House, Storage, Plant, Food, Incubator, Throne }

public class AgentBase : MonoBehaviour
{
    //Set in the Unity Editor based on the Prefab Type

    [SerializeField]
    private AgentType Type;

    public AgentType type
    {
        get { return Type; }
        private set { Type = value; }
    }

    public bool isActive { get; private set; }

    public LayerType layerType { get; private set; }
    public int ID { get; private set; }
    public int currentTileIndex { get; private set; }
    List<AgentProperty> properties = new List<AgentProperty>();

    public AgentInfo info { get; private set; }

    public void Start()
    {
        SetProperties();
        ID = Simulation.Instance.AddAgent(this);
        info = new AgentInfo(type);
    }

    public void GrowAgentProperties()
    {
        if(!isActive)
        {
            return;
        }

        foreach(var property in properties)
        {
            property.GrowWeight();
        }
    }

    public AgentProperty GetPropertyofType(PropertyType wantedProperty)
    {
        if(!isActive)
        {
            Debug.Log("Agent is Not Active");
            return null;
        }

        foreach(AgentProperty property in properties)
        {
            if(property.type == wantedProperty)
            {
                Debug.Log("PropertyFound" + " " + property.type);
                return property;
            }
        }
        return null;
    }

    public float GetPropertyWeight(PropertyType wantedPropertyWeight)
    {
        if (!isActive)
        {
            Debug.Log("Agent is Not Active");
            return -1;
        }

        foreach (AgentProperty property in properties)
        {
            if (property.type == wantedPropertyWeight)
            {
                Debug.Log("PropertyFound" + " " + property.type);
                Debug.Log("Property Weight" + "=" + property.weight);
                return property.weight;
            }
        }

        Debug.Log("Could not find Property Weight for " + wantedPropertyWeight);
        return 0;
    }

    private void SetProperties()
    {
        switch(type)
        {
            case AgentType.Bee:
                Debug.Log("Adding Properties to Bee");
                properties.Add(new AgentProperty(PropertyType.Hunger, 100, 0, 100));
                properties.Add(new AgentProperty(PropertyType.Energy, 100, 0, 100));
                properties.Add(new AgentProperty(PropertyType.Pollen, 100, 0, 0));
                isActive = true;
                layerType = LayerType.None;
                break;
            case AgentType.Storage:
                properties.Add(new AgentProperty(PropertyType.Pollen, 10000));
                layerType = LayerType.Pollen_Storge;
                isActive = false;
                break;
            case AgentType.House:
                properties.Add(new AgentProperty(PropertyType.Rest, 1000, 10, 1000));
                layerType = LayerType.Pollen_Storge;
                isActive = false;
                break;
            case AgentType.Food:
                properties.Add(new AgentProperty(PropertyType.Food, 1000, 4, 100));
                layerType = LayerType.Pollen_Storge;
                isActive = false;
                break;
            case AgentType.Plant:
                properties.Add(new AgentProperty(PropertyType.Pollen, 1000, 10, 1000));
                layerType = LayerType.Pollen_Storge;
                isActive = true;
                break;
            case AgentType.Incubator:
                properties.Clear();
                layerType = LayerType.None;
                isActive = false;
                break;
            case AgentType.Throne:
                properties.Clear();
                layerType = LayerType.Throne;
                isActive = true;
                break;
        }

        Debug.Log("Properties are Set");
    }

    public void SetCurrentTile(int tileIndex)
    {
        currentTileIndex = tileIndex;
        ReverseLookup.Instance.AddAgent(this);
    }

    public void SetActiveState(bool _isActive)
    {
        isActive = _isActive;
    }
}
