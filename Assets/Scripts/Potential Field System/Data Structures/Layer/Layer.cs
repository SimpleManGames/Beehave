using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Layer
{
    [SerializeField]
    private LayerType _layerType;

    public LayerType LayerType {
        get { return _layerType; }
        set { _layerType = value; }
    }

    [SerializeField]
    private List<LayerEffect> _layerRangeOfEffect;

    public List<LayerEffect> LayerEffect {
        get { return _layerRangeOfEffect; }
        set { _layerRangeOfEffect = value; }
    }
}
