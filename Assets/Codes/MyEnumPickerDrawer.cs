using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MyScript))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the 'select' enum dropdown
        EditorGUILayout.PropertyField(serializedObject.FindProperty("select"));

        // Get the selected enum value
        MyScript.Select selectedEnum = (MyScript.Select)serializedObject.FindProperty("select").enumValueIndex;

        // Draw the appropriate enum dropdown based on the selected 'select' enum
        if (selectedEnum == MyScript.Select.A)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enumA"), true);
        }
        else if (selectedEnum == MyScript.Select.B)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enumB"), true);
        }

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}