using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(View))]
public class ViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 GUI 표시
        base.OnInspectorGUI();

        // 대상 객체 가져오기
        View view = (View)target;

        // UIType 선택을 위한 드롭다운 표시
        view.UIType = (ViewController.UIType)EditorGUILayout.EnumPopup("UI Type", view.UIType);

        // 선택된 UIType에 따라서 이넘 값을 표시
        if (view.ViewController != null)
        {
            System.Type enumType = view.ViewController.GetUIEnum(view.UIType);
            if (enumType != null)
            {
                // 해당 이넘 값을 표시
                view.EnumValue = EditorGUILayout.EnumPopup("Enum Value", view.EnumValue);
            }
            else
            {
                EditorGUILayout.HelpBox("Failed to get enum type.", MessageType.Error);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("ViewController is not assigned.", MessageType.Error);
        }
    }
}