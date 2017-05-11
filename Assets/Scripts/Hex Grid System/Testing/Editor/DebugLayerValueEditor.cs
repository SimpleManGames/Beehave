using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugLayerValueManager))]
public class DebugLayerValueEditor : Editor
{
    DebugLayerValueManager manager;

    private void OnEnable()
    {
        manager = target as DebugLayerValueManager;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        manager.displayWorldCanvas = EditorGUILayout.Toggle("Debug Canvas", manager.displayWorldCanvas);
        manager.layerType = (LayerType)EditorGUILayout.EnumPopup("Debug Layer", manager.layerType);
        if (EditorGUI.EndChangeCheck())
        {
            foreach (var hex in Grid.Instance.Hexes)
                hex.transform.FindChild("Canvas").GetComponentInChildren<DebugLayerValue>().Change();

            manager.layerTextDebug.enabled = manager.displayWorldCanvas;
        }
    }
}
