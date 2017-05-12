using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulation : Singleton<Simulation>
{
    private List<Agent> agents = new List<Agent>();

    public Agent GetAgent(int id)
    {
        return agents.Where(a => a.ID == id).FirstOrDefault();
    }

    public int MakeAgent(Tasks startTask, List<Utility<UtilityType>> _agentUtilities, int startTile, bool isLiving)
    {
        int agentIndex = agents.Count();

        Agent newAgent = new Agent(agentIndex, startTask, _agentUtilities, startTile, isLiving);

        ReverseLookup.Instance.AddAgent(newAgent);

        agents.Add(newAgent);

        return newAgent.ID;
    }

    private float accumilatedTime = 0f;
    [SerializeField]
    private float frameLength = 0.75f;
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

    private void GameFrameTurn()
    {
        // TODO: See if this runs better as an event or foreach loop
        GlobalResources.Instance.TotalHoney += 10;
        agents.ForEach(a => a.Evaluate());
        currentFrame++;
    }
}