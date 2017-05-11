using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RangeTest))]
public class RangeTestInspector : Editor {

    public override void OnInspectorGUI()
    {
        RangeTest test = target as RangeTest;

        base.OnInspectorGUI();

        if (GUILayout.Button("Range"))
            test.DoRangeStuff();
    }
}
