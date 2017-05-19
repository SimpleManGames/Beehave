using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTasks
{
    AgentTasks(Tasks _task, float _weight)
    {
        task = _task;
        weight = _weight;
    }

    public Tasks task;
    public float weight;
}

public class AgentBrain : MonoBehaviour
{
    List<AgentTasks> availableTasks = new List<AgentTasks>();
    Tasks currentTask;

    public int targetTileIndex { get; private set; }

    private AgentSteering attachedController;

    public int globalIndex { get; private set; }

    public void Evaluate()
    {
        // Calls all Evaluation Tasks for Agent if needed (i.e. will only do FindTargetTile if it's reached it's targetTileIndex)
    }

    private void EvalTasks()
    {
        Tasks bestTask = Tasks.Null;

        // Loops through and generates weights for each Task and determines the best Task to take on

        currentTask = bestTask;
    }

    private void FindTargetTile()
    {
        // Finds Target Tile and passes it to AgentSteeringScript to be Handled

        attachedController.SetTargetTile(targetTileIndex);
    }

    private void Start()
    {
        attachedController = this.GetComponent<AgentSteering>();
        globalIndex = 0; // Should be set by "AddAgent(this)" method in Simulation when completed
    }
}
