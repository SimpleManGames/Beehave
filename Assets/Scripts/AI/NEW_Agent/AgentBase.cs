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
                break;
        }
    }

    void GenBeeProperties()
    {

    }

    
}
