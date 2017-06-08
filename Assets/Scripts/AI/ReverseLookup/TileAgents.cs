using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TileAgents
{
    const int maxTileCapacity = 40;

    int?[] agentsOnTile = new int?[maxTileCapacity];

    public void AddAgent(AgentBase newAgent)
    {
        for(int i = 0; i < agentsOnTile.Length; i++)
        {
            if(agentsOnTile[i] == null)
            {
                agentsOnTile[i] = newAgent.ID;
                return;
            }
        }
    }

    public void DeleteAgent(AgentBase movingAgent)
    {

        for(int i = 0; i < agentsOnTile.Length; i++)
        {
            if(agentsOnTile[i] == movingAgent.ID)
            {
                agentsOnTile[i] = null;
                return;
            }
        }
    }

    public int?[] AgentsOnTile()
    {
        return agentsOnTile;
    }
}
