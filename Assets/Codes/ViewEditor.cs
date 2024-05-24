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
        // �⺻ �ν����� GUI ǥ��
        base.OnInspectorGUI();

        // ��� ��ü ��������
        View view = (View)target;

        // UIType ������ ���� ��Ӵٿ� ǥ��
        view.UIType = (ViewController.UIType)EditorGUILayout.EnumPopup("UI Type", view.UIType);

        // ���õ� UIType�� ���� �̳� ���� ǥ��
        if (view.ViewController != null)
        {
            System.Type enumType = view.ViewController.GetUIEnum(view.UIType);
            if (enumType != null)
            {
                // �ش� �̳� ���� ǥ��
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