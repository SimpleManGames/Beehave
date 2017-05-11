using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Create Heat Map Info")]
public class HeatMapInteraction : ScriptableObject
{
    [SerializeField]
    private List<Layer> _layers;

    public List<Layer> Layers {
        get { return _layers; }
        set { _layers = value; }
    }
}