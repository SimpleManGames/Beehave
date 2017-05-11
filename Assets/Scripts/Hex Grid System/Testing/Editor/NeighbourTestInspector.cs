using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NeighbourTest))]
public class NeighbourTestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        NeighbourTest test = target as NeighbourTest;

        base.OnInspectorGUI();

        if (GUILayout.Button("Neighbour"))
            test.DoNeighbourStuff();
    }
}
