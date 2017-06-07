using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentType { Bee, House, Storage, Plant, Food }

public class AgentBase : MonoBehaviour
{
    //Set in the Unity Editor based on the Prefab Type
    AgentType type;
    public int ID { get; private set; }
    public int currentTile { get; private set; }
    List<AgentProperty> properties = new List<AgentProperty>();

    public void Start()
    {
        SetProperties();
    }

    void SetProperties()
    {
        switch(type)
        {
            case AgentType.Bee:
                properties.Add(new AgentProperty(PropertyType.Hunger, 100, -3));
                properties.Add(new AgentProperty(PropertyType.Energy, 100, -2));
                properties.Add(new AgentProperty(PropertyType.Pollen, 100, 0));
                break;
            case AgentType.Storage:
                properties.Add(new AgentProperty(PropertyType.Pollen, 10000));
                break;
            case AgentType.House:
                properties.Add(new AgentProperty(PropertyType.Rest, 1000, 10, 1000));
                break;
            case AgentType.Food:
                properties.Add(new AgentProperty(PropertyType.Food, 1000, 4, 100));
                break;
            case AgentType.Plant:
                properties.Add(new AgentProperty(PropertyType.Pollen, 1000, 10, 1000));
                break;
        }
    }
}
