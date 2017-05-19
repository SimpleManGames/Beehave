﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTasks
{
    AgentTasks(Tasks _task, float _weight = 0)
    {
        task = _task;
        weight = _weight;
    }

    public Tasks task { get; private set; }
    public float weight { get; private set; }

    public void SetWeight(float _weight)
    {
        weight = _weight;
    }
}