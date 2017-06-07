using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTask
{
    public AgentTask(Tasks _task, AgentBase attachedBase, float _weight = 0)
    {
        task = _task;
        weight = _weight;
        affectedAgent = attachedBase;
    }

    private AgentBase affectedAgent;
    public Tasks task { get; private set; }
    public float weight { get; private set; }

    public void SetWeight(float _weight)
    { 
        weight = _weight;
    }

    public void DoTask()
    {
        switch (task)
        {
            case Tasks.Eat:
                DoEat();
                break;
            case Tasks.Sleep:
                DoSleep();
                break;
            case Tasks.GatherPollen:
                DoGatherPollen();
                break;
            case Tasks.StorePollen:
                DoStorePollen();
                break;
            default:
                break;
        }
    }

    private void DoEat()
    {
        
    }

    private void DoSleep()
    {

    }

    private void DoGatherPollen()
    {

    }

    private void DoStorePollen()
    {

    }
}
