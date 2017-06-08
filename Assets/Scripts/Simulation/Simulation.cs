using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulation : Singleton<Simulation>
{
    public delegate void AgentUpdateDel();
    public AgentUpdateDel agentUpdate;

    /// <summary>
    /// List of all the agents
    /// </summary>
    private List<AgentBase> agents = new List<AgentBase>();

    /// <summary>
    /// Gets the agent with the ID that's passed in
    /// </summary>
    /// <param name="id">The agent's ID that is to be gotten</param>
    /// <returns>Returns an agent object with the requested ID</returns>
    public AgentBase GetAgent(int id)
    {
        return agents.Where(a => a.ID == id).FirstOrDefault();
    }

    /// <summary>
    /// Gets all the agents that simulation currently has
    /// </summary>
    /// <returns>Returns all agents as an array</returns>
    public AgentBase[] GetAllAgents()
    {
        return agents.ToArray();
    }

    public int AddAgent(AgentBase newAgent)
    {
        int agentIndex = agents.Count();

        ReverseLookup.Instance.AddAgent(newAgent);

        agents.Add(newAgent);

        return agentIndex;
    }

    // Time from when we last changed frame
    private float accumilatedTime = 0f;
    [SerializeField]
    [Tooltip("How long inbetween frames should be.")]
    private float frameLength = 0.75f;
    // Stores the current frame we are on
    private int currentFrame = 0;

    public void Update()
    {
        accumilatedTime = accumilatedTime + Time.deltaTime;

        while (accumilatedTime > frameLength)
        {
            GameFrameTurn();
            accumilatedTime = accumilatedTime - frameLength;
        }
    }

    /// <summary>
    /// Simulation loop
    /// </summary>
    private void GameFrameTurn()
    {
        if(agentUpdate != null)
        {
            agentUpdate();
            agentUpdate = delegate { };
        }

        currentFrame++;
    }
}