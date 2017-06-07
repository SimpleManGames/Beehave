using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTask
{
    public AgentTask(Tasks _type, AgentBase attachedBase, float _weight = 0)
    {
        type = _type;
        weight = _weight;
        affectedAgent = attachedBase;
    }

    private AgentBase affectedAgent;
    public Tasks type { get; private set; }
    public float weight { get; private set; }

    public void SetWeight(float _weight)
    { 
        weight = _weight;
    }

    public void DoTask()
    {
        switch (type)
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
