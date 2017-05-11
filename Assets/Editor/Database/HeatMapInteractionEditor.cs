using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(HeatMapInteraction))]
public class HeatMapInteractionEditor : Editor
{
    ReorderableList m_list;

    private SerializedProperty _listProperty;
    private SerializedProperty ListProperty
    {
        get
        {
            if (_listProperty == null)
                _listProperty = serializedObject.FindProperty("_layers");

            return _listProperty;
        }
    }

    private int? indexInList = null;
    private SerializedProperty ItemListProperty
    {
        get
        {
            if (indexInList == null)
                Debug.LogWarning("indexInList is null and trying to be accessed");

            return ListProperty.GetArrayElementAtIndex(indexInList ?? default(int));
        }
    }

    private SerializedProperty EnumProperty
    {
        get
        {
            return ItemListProperty.FindPropertyRelative("_layerType");
        }
    }

    private SerializedProperty EffectListProperty
    {
        get
        {
            return ItemListProperty.FindPropertyRelative("_layerRangeOfEffect");
        }
    }

    private List<float> heights;
    private List<float> Heights
    {
        get
        {
            if (heights == null)
                heights = new List<float>(ListProperty.arraySize);

            return heights;
        }

        set
        {
            heights = value;
        }
    }

    private float GetHeightFromArray
    {
        get
        {
            float height = 0;
            try
            {
                if (heights == null)
                    heights = new List<float>(ListProperty.arraySize);

                height = heights[indexInList ?? default(int)];
            }
            catch (ArgumentOutOfRangeException e) { Debug.LogWarning(e.Message); }
            finally
            {
                float[] floats = heights.ToArray();
                Array.Resize(ref floats, ListProperty.arraySize);
                heights = floats.ToList();
            }
            return height;
        }
    }

    public void OnEnable()
    {
        m_list = new ReorderableList(serializedObject, ListProperty, true, true, true, true);

        m_list.drawHeaderCallback = (rect) => { EditorGUI.LabelField(rect, "HeatMap Layers"); };

        m_list.drawElementCallback = (rect, index, isActive, isEnabled) =>
        {
            indexInList = index;
            int lineCount = 0;
            bool foldout = isActive;
            float height = EditorGUIUtility.singleLineHeight * 1.25f;

            EditorGUILayout.BeginHorizontal();

            EditorGUI.PropertyField(GetEnumRect(rect), EnumProperty, GUIContent.none);

            var addButtonRect = rect;
            addButtonRect.width /= 3f;
            addButtonRect.height = EditorGUI.GetPropertyHeight(EnumProperty);
            addButtonRect.x += rect.width / 2f;

            if (GUI.Button(addButtonRect, "Add Effect"))
            {
                //var newIndex = EffectListProperty.arraySize++;
            }

            EditorGUILayout.EndHorizontal();

            if (foldout)
            {
                DrawFoldOut(ref lineCount, rect);
                height = EditorGUIUtility.singleLineHeight * lineCount;
            }

            SetupHeightArray(ref height);

            float margin = height / 10;
            rect.y += margin;
            rect.height = (height / 5) * 4;
            rect.width = rect.width / 2 - margin / 2;

            rect.x += rect.width + margin;
        };

        m_list.elementHeightCallback = (index) =>
        {
            indexInList = index;
            Repaint();
            return GetHeightFromArray;
        };

        m_list.onRemoveCallback = (l) =>
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete this?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };
    }

    private void SetupHeightArray(ref float height)
    {
        try { Heights[indexInList ?? default(int)] = height; }
        catch (ArgumentOutOfRangeException e) { Debug.LogWarning(e.Message); }
        finally
        {
            float[] floats = heights.ToArray();
            Array.Resize(ref floats, ListProperty.arraySize);
            Heights = floats.ToList();
        }
    }

    private void DrawFoldOut(ref int lineCount, Rect rect)
    {
        EditorGUI.indentLevel++;

        var effectListRect = rect;
        effectListRect.y += EditorGUIUtility.singleLineHeight;

        int indexCount = 0;
        foreach (SerializedProperty element in EffectListProperty)
        {
            var layerProp = element.FindPropertyRelative("_layer");
            var rangeProp = element.FindPropertyRelative("_rangeOfEffect");

            float boxY = effectListRect.y + EditorGUIUtility.singleLineHeight * lineCount;
            float boxHeight = EditorGUI.GetPropertyHeight(layerProp);

            var lRect = new Rect(34, boxY, 100, boxHeight);

            int padding = 5;
            var eRect = new Rect((lRect.x - 14) + lRect.width + padding, boxY, 60, boxHeight);

            EditorGUILayout.BeginHorizontal();

            EditorGUI.PropertyField(lRect, layerProp, GUIContent.none);
            EditorGUI.PropertyField(eRect, rangeProp, GUIContent.none);

            var deleteRect = new Rect(eRect.x + eRect.width + padding, boxY, effectListRect.width / 4f, boxHeight);

            if (GUI.Button(deleteRect, "Delete"))
            {
                if (EditorUtility.DisplayDialog("Warning!",
                    "Are you sure you want to delete this?", "Yes", "No"))
                    EffectListProperty.DeleteArrayElementAtIndex(indexCount--);
            }

            EditorGUILayout.EndHorizontal();

            lineCount++;
            indexCount++;
        }
        lineCount++;
        EditorGUI.indentLevel--;
    }

    private Rect GetEnumRect(Rect rect)
    {
        var enumRect = rect;
        enumRect.width /= 2f;
        return enumRect;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        m_list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
