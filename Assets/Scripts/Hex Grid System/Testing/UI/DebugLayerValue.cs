using UnityEngine;
using UnityEngine.UI;

public class DebugLayerValue : MonoBehaviour
{

    private Text text;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        DebugLayerValueManager.Instance.canvas.Add(this);
    }

    public void Change()
    {
        if (DebugLayerValueManager.Instance != null && text != null)
        {
            text.enabled = DebugLayerValueManager.Instance.displayWorldCanvas;
            text.text = HeatMapInfo.Instance.Field[DebugLayerValueManager.Instance.layerType][GetComponentInParent<HexObject>().Index].ToString();
        }
    }
}
