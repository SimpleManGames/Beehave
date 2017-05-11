using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentObject : MonoBehaviour
{
    public int agentIndex;

    public Agent agent { get { return Simulation.Instance.GetAgent(agentIndex); } private set { } }

    public Tasks agentCurrentTask { get { return Simulation.Instance.GetAgent(agentIndex).currentTask; } private set { } }

    public AgentInfo info;

    public void CreateAgent(int startTileIndex, Tasks startTask)
    {
        GlobalResources.Instance.TotalPopulation += 1;
        agentIndex = Simulation.Instance.MakeAgent(startTask, AgentFactory.Instance.CreateLivingUtilities(), startTileIndex, true);
        info = new AgentInfo();
        info.genLivingAgentInfo();
    }
}
