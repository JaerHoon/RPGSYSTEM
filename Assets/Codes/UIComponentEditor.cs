using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentUIView), true)]

public class UIComponentEditor : Editor
{
    ComponentUIView View;

    public override void OnInspectorGUI()
    {
        View = (ComponentUIView)target;

        View.uIClass = (ComponentUIView.UIClass)EditorGUILayout.EnumPopup("UIClass", View.uIClass);

        if (View.uIClass == ComponentUIView.UIClass.Board)
        {
            View.boardcomponentType = (ComponentUIView.BoardUI_Component)EditorGUILayout.EnumPopup("BoardComponent", View.boardcomponentType);
        }
        else if(View.uIClass == ComponentUIView.UIClass.Element)
        {
            View.elementcomponrntType = (ComponentUIView.ElementUI_Component)EditorGUILayout.EnumPopup("ElementComponent", View.elementcomponrntType);
        }
        else
        {
            View.slotcomponentType = (ComponentUIView.SlotUI_Component)EditorGUILayout.EnumPopup("SlotComponent", View.slotcomponentType);
        }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(View);
        }
    }


}
