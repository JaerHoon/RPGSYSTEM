using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPGSYSTEM.UI;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(View), true)] // View Ŭ������ ��ü�� �����Ϳ� ����, �� �ڽ�Ŭ�������� ����
public class ViewEditor : Editor
{
    // ViewModel�� �ڽ� Ŭ������ Ÿ���� ��� ���� �迭
    private Type[] derivedtype;
    View viewtarget;

    private void OnEnable()
    {
        // ViewModel�� ��ӹ޴� ��� Ŭ������ ã�Ƽ� �迭�� ����
        derivedtype = Assembly.GetAssembly(typeof(ViewModel)).GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ViewModel)))
            .ToArray();
    }

    public override void OnInspectorGUI()
    {
        viewtarget = (View)target; // �����͸� ������ ����� ���Ѵ�.

        viewtarget.viewController = (ViewController)EditorGUILayout.ObjectField("ViewController", viewtarget.viewController, typeof(ViewController), true);

        //ViewController.UIType �̳��� �ν����Ϳ� ǥ���ض�."UI_Type" ��� �̸�����...
        viewtarget.uIType = (ViewController.UIType)EditorGUILayout.EnumPopup("UI_Type", viewtarget.uIType);

        //ViewModel.ReferenceType �̳��� �ν����Ϳ� ǥ���ض�."Reference_type"��� �̸�����...
        viewtarget.referenceType = (ViewModel.ReferenceType)EditorGUILayout.EnumPopup("Reference_type", viewtarget.referenceType);

        //viewtarget.uIType���� ���õ� �̳� ���� �̸��� ���� Ŭ������ ã�Ƽ� Type�� ����
        Type selectedtype = derivedtype.FirstOrDefault(t => t.Name == viewtarget.uIType.ToString());
        Type basetype = selectedtype.BaseType;

        if (selectedtype != null)
        {
            // viewtarget.referenceType���� ���õ� ���� ��Ʈ������ ��ȯ
            string na = viewtarget.referenceType.ToString();
            System.Type enumType;
            if (viewtarget.referenceType == ViewModel.ReferenceType.Field)
            {
                //viewtarget.referenceType���� ���õ� ���� ������ selectedtype Ŭ���� �ȿ� �ִ� �ʵ带 ã�´�.
                enumType = selectedtype.GetNestedType(na);
            }
            else
            {
                enumType = basetype.GetNestedType(na);
            }

           

            if (enumType != null && enumType.IsEnum) // ���� enumType �ʵ尡 �̳��̸�...
            {
                // �̳��� ��� ����� �̸��� �迭�� ����
                var enumnames = Enum.GetNames(enumType);
                //���� viewtarget.SetectedValue���� ���õ� ���� �ε����� ã�´�.
                int selectedIndex = Array.IndexOf(enumnames, viewtarget.SetectedValue);
                if (selectedIndex == -1) selectedIndex = 0; //���� ���õ� ���� ���ٸ� �ε��� ��ȣ �ʱ�ȭ

                //���õ� �̳� ���� ������ "ValueName"���� ������ �ν����� â��  ��Ÿ����.
                selectedIndex = EditorGUILayout.Popup("ValueName", selectedIndex, enumnames);

                //�ν����� â���� ���õ� ���� viewtarget.SetectedValue���� �����Ѵ�.
                viewtarget.SetectedValue = enumnames[selectedIndex];

            }


            object obj = viewtarget.viewController.GetValue(viewtarget.uIType, viewtarget.SetectedValue);

            viewtarget.value = obj;
            string val = default;
            if (viewtarget.referenceType == ViewModel.ReferenceType.Field) 
            {
              
                val = obj.ToString(); 
            }
            else
            {
                Delegate action = (Delegate)obj;

                val = action.Method.Name.ToString();
                   
            }
           
                //ValueName ���� ���� ���� Ŭ������ �����ؼ� ���� �޾Ƽ� string���� �����´�.


                viewtarget.valueText = EditorGUILayout.TextField("Value", val);
                viewtarget.valueText = val;

                //viewtarget.ChainMethod();

        }

       
        if (GUI.changed)
        {
            EditorUtility.SetDirty(viewtarget);
        }
    
    }

    
    
}
