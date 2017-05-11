using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReverseLookup : Singleton<ReverseLookup>
{
    // List of Lists that hold Agent Index's for Looking up Agents that share a tile
    // TODO: Fixing to add a class instead of internal lists
    private List<TileAgents> TileLookup = new List<TileAgents>();

    public void MoveAgent(Agent agent)
    {
        TileLookup[agent.GetLastTile()].DeleteAgent(agent);
        TileLookup[agent.currentTileIndex].AddAgent(agent);
    }

    public void AddAgent(Agent agent)
    {
        TileLookup[agent.currentTileIndex].AddAgent(agent);
    }

    public void DeleteAgent(Agent agent)
    {
        TileLookup[agent.currentTileIndex].DeleteAgent(agent);
    }

    public int?[] AgentsOnTile(Agent agent)
    {
       return TileLookup[agent.currentTileIndex].AgentsOnTile();
    }

    public void Start()
    {
        for(int i = 0; i <= Grid.Instance.Hexes.Count; i++)
        {
            TileLookup.Add(new TileAgents());
        }
    }
}
