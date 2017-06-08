using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Rail))]
public class RailEditor : Editor
{
    private Rail rail
    {
        get { return target as Rail; }
    }
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("nodes"), true, true, false, false);
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += 2;
            string indexName = serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", "nodes", index.ToString())).objectReferenceValue.name;
            EditorGUI.LabelField(rect, indexName);
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Nodes");
        };
        list.onReorderCallback = (ReorderableList list) =>
        {
            for (int i = 0; i < list.count; i++)
            {
                var child = serializedObject.FindProperty(string.Format("{0}.Array.data[{1}]", "nodes", i.ToString())).objectReferenceValue as Transform;
                child.SetSiblingIndex(i);
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}