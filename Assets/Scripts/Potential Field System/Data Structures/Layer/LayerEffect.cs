using UnityEngine;

[System.Serializable]
public class LayerEffect
{
    [SerializeField]
    private LayerType _layer;

    public LayerType Layer {
        get { return _layer; }
        set { _layer = value; }
    }

    [SerializeField]
    private float _rangeOfEffect;

    public float RangEffect {
        get { return _rangeOfEffect; }
        set { _rangeOfEffect = value; }
    }
}
