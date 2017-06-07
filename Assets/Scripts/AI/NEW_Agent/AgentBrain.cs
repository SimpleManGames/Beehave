﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBrain : MonoBehaviour
{
    List<AgentTask> availableTasks = new List<AgentTask>();
    public AgentTask currentTask { get; private set; }
    bool taskReached = false;
    bool taskComplete = false;

    public int targetTileIndex { get; private set; }

    private AgentSteering attachedController;
    private AgentBase attachedBase;

    public void Evaluate()
    {
        // Calls all Evaluation Tasks for Agent if needed (i.e. will only do FindTargetTile if it's reached it's targetTileIndex)
    }

    //Detemine next best Task for the Agent to take on based on Task Utility
    private void EvalTasks()
    {
        if(!taskComplete)
        {
            return;
        }

        AgentTask bestTask = new AgentTask(Tasks.Null, attachedBase);
        float bestScore = 0;

        foreach (var Task in availableTasks)
        {
            Task.SetWeight(EvalUtility(Task.task));

            // Innocent method of just setting the Task with highest weight to be our current Task
            if (Task.weight > bestScore)
            {
                bestTask = Task;
            }

            currentTask = bestTask;
        }
    }

    private void FindTargetTile()
    {
        // If CurrentTile Layer Value is 1 set TaskReached to True and return
        // Finds Target Tile and passes it to AgentSteeringScript to be Handled

        attachedController.SetTargetTile(targetTileIndex);
    }

    private void Start()
    {
        attachedController = this.GetComponent<AgentSteering>();
        attachedBase = this.GetComponent<AgentBase>();

        PrepTaskList();
    }

    public void PrepTaskList()
    {
        availableTasks.Add(new AgentTask(Tasks.Eat, attachedBase));
        availableTasks.Add(new AgentTask(Tasks.Sleep, attachedBase));
        availableTasks.Add(new AgentTask(Tasks.GatherPollen, attachedBase));
        availableTasks.Add(new AgentTask(Tasks.StorePollen, attachedBase));
    }


    // Utility Scoring Method, uses switch to allow passing of all Tasks into one function and sorted there.
    private float EvalUtility(Tasks evalTask)
    {
        float score = 0;

        switch (evalTask)
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
