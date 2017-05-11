using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Tasks { Null = 0, Eat = 1, Sleep = 2, GatherPollen = 3, StorePollen = 4, ProducePollen = 5, ProduceHoney = 6 }

public static class Task
{
    public static List<LayerType> taskList = new List<LayerType>()
    {
       LayerType.None, LayerType.Honey, LayerType.Sleep, LayerType.Pollen, LayerType.Pollen_Storge, LayerType.Pollen, LayerType.Pollen
    };

    public static LayerType GetTaskLayer(Tasks task)
    {
        return taskList.ElementAt((int)task);
    }
}
