using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTasks
{
    public Tasks type;
    public float cummulativeWeight;
    public int timesAdded;

}

public class GlobalTasks : Singleton<GlobalTasks>
{
    List<GTasks> globalTasks = new List<GTasks>();

    public float GetGlobalWeight(Tasks wantedTaskWeight)
    {
        foreach (var task in globalTasks)
        {
            if (task.type == wantedTaskWeight)
            {
                return task.cummulativeWeight / task.timesAdded;
            }
        }

        return 0;
    }

}
