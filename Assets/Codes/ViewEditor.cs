using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPGSYSTEM.UI;

[CustomEditor(typeof(View), true)]
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
        view.SelectType = (ViewModel.ReferenceType)EditorGUILayout.EnumPopup("Valuetype", view.SelectType);
        //System.Type enType = view.SelectValue.GetType();
        // SelectValue의 형식을 기반으로 EnumPopup 표시
        //view.SelectValue = EditorGUILayout.EnumPopup("Select Value", view.SelectValue, enType);

        // 선택된 UIType에 따라서 이넘 값을 표시
        if (view.ViewController != null)
        {
            System.Type enumType = view.ViewController.GetUIEnum(view.UIType);
            if (enumType != null)
            {
                //System.Type enT = view.SelectValue.GetType();
                // 해당 이넘 값을 표시
                //view.SelectType = EditorGUILayout.EnumPopup("Enum Value", view.SelectType);
               // view.SelectValue =  EditorGUILayout.EnumPopup("Select Value", view.SelectValue);
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
