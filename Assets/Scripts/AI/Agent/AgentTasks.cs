using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTask
{
    public AgentTask(Tasks _type, AgentBase _attachedBase, float _weight = 0)
    {
        type = _type;
        weight = _weight;
        attachedBase = _attachedBase;

        switch(type)
        {
            case Tasks.Eat:
                layer = LayerType.Honey;
                break;
            case Tasks.Sleep:
                layer = LayerType.Sleep;
                break;
            case Tasks.GatherPollen:
                layer = LayerType.Pollen;
                break;
            case Tasks.StorePollen:
                layer = LayerType.Pollen_Storge;
                break;
        }
    }

    private AgentBase attachedBase;
    public Tasks type { get; private set; }
    public LayerType layer { get; private set; }
    public float weight { get; private set; }

    public void SetWeight(float _weight)
    { 
        weight = _weight;
    }

    public void DoTask()
    {
        switch (type)
        {
            case Tasks.Eat:
                TradeResources(AgentType.Food, PropertyType.Hunger, PropertyType.Food, 10);
                break;
            case Tasks.Sleep:
                TradeResources(AgentType.House, PropertyType.Energy, PropertyType.Rest, 10);
                break;
            case Tasks.GatherPollen:
                TradeResources(AgentType.Plant, PropertyType.Pollen, PropertyType.Pollen, 10);
                break;
            case Tasks.StorePollen:
                TradeResources(AgentType.Plant, PropertyType.Pollen, PropertyType.Pollen, 10);
                break;
            default:
                break;
        }
    }

    private void TradeResources(AgentType wantedAgentType, PropertyType giveProperty, PropertyType takeProperty, int amountToTrade, bool isGiving = false)
    {
        int?[] agentsOnTile = ReverseLookup.Instance.AgentsOnTile(attachedBase);

        if (agentsOnTile == null)
        {
            Debug.Log("There's no Task Object on this Tile with AgentID " + attachedBase.ID);
            return;
        }

        foreach (var agent in agentsOnTile)
        {
            AgentBase tradingAgent = Simulation.Instance.GetAgent((int)agent);
            if (tradingAgent.type != wantedAgentType)
            {
                continue;
            }

            AgentProperty givingProperty;
            AgentProperty takingProperty;

            if (isGiving)
            {
                givingProperty = attachedBase.GetPropertyofType(giveProperty);
                takingProperty = tradingAgent.GetPropertyofType(takeProperty);
            }
            else
            {
                givingProperty = tradingAgent.GetPropertyofType(giveProperty);
                takingProperty = attachedBase.GetPropertyofType(takeProperty);
            }

            takingProperty.TakeWeight(givingProperty.GiveWeight(amountToTrade));

            if (giveProperty == PropertyType.Pollen && isGiving)
            {
                GlobalResources.Instance.TotalPollen += amountToTrade;
            }

            return;
        }

        Debug.Log("Something went wrong completing the task for AgentID" + attachedBase.ID);
    }
}
