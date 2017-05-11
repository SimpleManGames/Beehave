using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLayerValueManager : Singleton<DebugLayerValueManager>
{
    [HideInInspector]
    public List<DebugLayerValue> canvas = new List<DebugLayerValue>();
    public bool displayWorldCanvas = false;
    public LayerType layerType;

    [HideInInspector]
    public Text layerTextDebug;

    private void Start()
    {
        layerTextDebug = GameObject.Find("Debug Layer Value Text").GetComponent<Text>();
    }

    public void ManualUpdate()
    {
        layerTextDebug.enabled = displayWorldCanvas;
        layerTextDebug.text = layerType.ToString();
        canvas.ForEach(c => c.Change());
    }
}
