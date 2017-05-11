using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexObject))]
public class Hex_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty hex = serializedObject.FindProperty("hex");
        SerializedProperty cubeCoords = hex.FindPropertyRelative("cubeCoords");

        int indexDisplay = serializedObject.FindProperty("index").intValue;
        Vector3 cubeCoordDisplay = new Vector3(cubeCoords.FindPropertyRelative("q").floatValue, cubeCoords.FindPropertyRelative("r").floatValue, cubeCoords.FindPropertyRelative("s").floatValue);

        EditorGUILayout.IntField("Index", indexDisplay);
        EditorGUILayout.Vector3Field("CubeCoord", cubeCoordDisplay);
    }
}
