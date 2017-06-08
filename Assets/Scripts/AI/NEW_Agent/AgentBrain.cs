using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBrain : MonoBehaviour
{
    List<AgentTask> availableTasks = new List<AgentTask>();
    public AgentTask currentTask { get; private set; }
    private bool taskReached = false;
    private bool taskComplete = false;

    public int targetTileIndex { get; private set; }

    private AgentSteering attachedController;
    private AgentBase attachedBase;

    private Queue<int> prevTileIndices = new Queue<int>();
    private int tileMemory = 5;

    AnimationCurve utilityCurve = new AnimationCurve();

    private void Start()
    {
        attachedController = this.GetComponent<AgentSteering>();
        attachedBase = this.GetComponent<AgentBase>();

        currentTask = new AgentTask(Tasks.Null, attachedBase);
        PrepTaskList();

        UpdateAgent();
    }

    public void Update()
    {
        if(taskComplete || taskReached || attachedController.targetReached)
        {
            Simulation.Instance.agentUpdate += new Simulation.AgentUpdateDel(UpdateAgent);
        }
    }

    public void UpdateAgent()
    {
        if(taskComplete || currentTask.type == Tasks.Null)
        {
            EvalTasks();
        }

        if(taskReached)
        {
            currentTask.DoTask();
            return;
        }

        if(attachedController.targetReached)
        {
            FindTargetTile();
        }
    }

    public void PrepTaskList()
    {
        availableTasks.Add(new AgentTask(Tasks.Eat, attachedBase));
        availableTasks.Add(new AgentTask(Tasks.Sleep, attachedBase));
        availableTasks.Add(new AgentTask(Tasks.GatherPollen, attachedBase));
        availableTasks.Add(new AgentTask(Tasks.StorePollen, attachedBase));
    }

    // TODO: Add functionality for adding tiles to previous tile queue
    private void FindTargetTile()
    {
        Hex[] adjacencyList = Hex.Neighbours(Grid.FindHexObject(attachedController.targetTileIndex).hex);
        int bestTile = 0;
        float bestWeight = 0;

        foreach (var tile in adjacencyList)
        {
            float currentWeight = 0;
            HexObject currentTile = Grid.FindHexObject(tile.cubeCoords);

            if (currentTile == null || prevTileIndices.Contains(currentTile.Index))
            {
                continue;
            }

            if(currentTask.type == Tasks.Null)
            {
                currentWeight += Random.Range(0f, 1f);
            }
            else
            {
                currentWeight += HeatMapInfo.Instance.Field[currentTask.layer][currentTile.Index];
                if (currentWeight >= 1f)
                {
                    AddPrevTile(attachedController.targetTileIndex);
                    attachedBase.SetCurrentTile(attachedController.targetTileIndex);
                    attachedController.SetTargetTile(currentTile.Index);
                    taskReached = true;
                    return;
                }
            }

            currentWeight += HeatMapInfo.Instance.Field[LayerType.Terrain][currentTile.Index];
            if (currentWeight >= bestWeight)
            {
                bestWeight = currentWeight;
                bestTile = currentTile.Index;
            }
        }

        AddPrevTile(attachedController.targetTileIndex);
        attachedBase.SetCurrentTile(attachedController.targetTileIndex);
        attachedController.SetTargetTile(bestTile);
    }

    // Add tile to prevTile Queue
    void AddPrevTile(int tileIndex)
    {
        prevTileIndices.Enqueue(tileIndex);
        if (prevTileIndices.Count > tileMemory)
        {
            prevTileIndices.Dequeue();
        }
    }

    //Detemine next best Task for the Agent to take on based on Task Utility
    private void EvalTasks()
    {
        AgentTask bestTask = new AgentTask(Tasks.Null, attachedBase);
        float bestScore = 0;

        foreach (var Task in availableTasks)
        {
            Task.SetWeight(EvalUtility(Task));

            // Innocent method of just setting the Task with highest weight to be our current Task
            if (Task.weight > bestScore)
            {
                bestTask = Task;
            }

            currentTask = bestTask;
        }
    }

    // Utility Scoring Method, uses switch to allow passing of all Tasks into one function and sorted there.
    private float EvalUtility(AgentTask evalTask)
    {
        float score = 0;

        switch (evalTask.type)
        {
            case Tasks.Eat:
                score = ScoreEat();
                break;
            case Tasks.Sleep:
                score = ScoreSleep();
                break;
            case Tasks.GatherPollen:
                score = ScoreGatherPollen();
                break;
            case Tasks.StorePollen:
                score = ScoreDepositPollen();
                break;
            default:
                break;
        }

        return score;
    }

    private float ScoreGatherPollen()
    {


        return 0f;
    }

    private float ScoreDepositPollen()
    {
        return 0f;
    }

    private float ScoreEat()
    {
        return 0f;
    }

    private float ScoreSleep()
    {
        return 0f;
    }
}
