using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDebug : MonoBehaviour
{
    int debugLayerInt;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            debugLayerInt++;
            debugLayerInt = Mathf.Clamp(debugLayerInt, 0, Enum.GetNames(typeof(LayerType)).Length - 1);
            DebugLayerValueManager.Instance.layerType = (LayerType)debugLayerInt;
            DebugLayerValueManager.Instance.ManualUpdate();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            debugLayerInt--;
            debugLayerInt = Mathf.Clamp(debugLayerInt, 0, Enum.GetNames(typeof(LayerType)).Length - 1);
            DebugLayerValueManager.Instance.layerType = (LayerType)debugLayerInt;
            DebugLayerValueManager.Instance.ManualUpdate();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            DebugLayerValueManager.Instance.displayWorldCanvas = !DebugLayerValueManager.Instance.displayWorldCanvas;
            DebugLayerValueManager.Instance.ManualUpdate();
        }
    }
}
