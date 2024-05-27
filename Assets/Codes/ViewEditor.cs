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
        // �⺻ �ν����� GUI ǥ��
        base.OnInspectorGUI();

        // ��� ��ü ��������
        View view = (View)target;

        // UIType ������ ���� ��Ӵٿ� ǥ��
        view.UIType = (ViewController.UIType)EditorGUILayout.EnumPopup("UI Type", view.UIType);
        view.SelectType = (ViewModel.ReferenceType)EditorGUILayout.EnumPopup("Valuetype", view.SelectType);
        //System.Type enType = view.SelectValue.GetType();
        // SelectValue�� ������ ������� EnumPopup ǥ��
        //view.SelectValue = EditorGUILayout.EnumPopup("Select Value", view.SelectValue, enType);

        // ���õ� UIType�� ���� �̳� ���� ǥ��
        if (view.ViewController != null)
        {
            System.Type enumType = view.ViewController.GetUIEnum(view.UIType);
            if (enumType != null)
            {
                //System.Type enT = view.SelectValue.GetType();
                // �ش� �̳� ���� ǥ��
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
