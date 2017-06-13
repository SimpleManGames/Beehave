using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ReverseLookup : Singleton<ReverseLookup>
{
    // List of Lists that hold Agent Index's for Looking up Agents that share a tile
    private List<TileAgents> TileLookup = new List<TileAgents>();

    public void MoveAgent(AgentBase agent, int previousTile)
    {
        TileLookup[previousTile].DeleteAgent(agent);
        TileLookup[agent.currentTileIndex].AddAgent(agent);
    }

    public void AddAgent(AgentBase agent)
    {
        TileLookup[agent.currentTileIndex].AddAgent(agent);
    }

    public void DeleteAgent(AgentBase agent)
    {
        TileLookup[agent.currentTileIndex].DeleteAgent(agent);
    }

    public int?[] AgentsOnTile(AgentBase agent)
    {
       return TileLookup[agent.currentTileIndex].AgentsOnTile();
    }

    override public void Awake()
    {
        base.Awake();
        for(int i = 0; i <= Grid.Instance.Hexes.Count; i++)
        {
            TileLookup.Add(new TileAgents());
        }
    }
}
