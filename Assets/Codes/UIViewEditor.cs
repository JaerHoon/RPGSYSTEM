using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MenuUIView), true)]
public class UIViewEditor : Editor
{
    MenuUIView menu;

    public override void OnInspectorGUI()
    {
        menu = (MenuUIView)target;

        menu.GetUIcontroller();
        if (menu.UIController != null)
        {
            EditorGUILayout.LabelField("UIController Class Name", menu.UIController.GetType().Name);
        }
        else
        {
            EditorGUILayout.LabelField("UIController Class Name", "UIController is not assigned");
        }

        menu.MenuIndex = EditorGUILayout.IntField("MenuIndex", menu.MenuIndex);

        menu.UIModel = menu.UIController.menuUIModels[menu.MenuIndex];

        menu.SetChildBoard();

        menu.SetMenuUIModel();


        if (GUI.changed)
        {
            EditorUtility.SetDirty(menu);
        }
    }
}
