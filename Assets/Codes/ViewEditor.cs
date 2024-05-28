using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPGSYSTEM.UI;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(View), true)] // View 클래스의 객체의 에디터에 적용, 및 자식클래스에도 적용
public class ViewEditor : Editor
{
    // ViewModel의 자식 클래스의 타입을 모두 담을 배열
    private Type[] derivedtype;
    View viewtarget;

    private void OnEnable()
    {
        // ViewModel을 상속받는 모든 클래스를 찾아서 배열로 저장
        derivedtype = Assembly.GetAssembly(typeof(ViewModel)).GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ViewModel)))
            .ToArray();
    }

    public override void OnInspectorGUI()
    {
        viewtarget = (View)target; // 에디터를 적용할 대상을 정한다.

        viewtarget.viewController = (ViewController)EditorGUILayout.ObjectField("ViewController", viewtarget.viewController, typeof(ViewController), true);

        //ViewController.UIType 이넘을 인스팩터에 표시해라."UI_Type" 라는 이름으로...
        viewtarget.uIType = (ViewController.UIType)EditorGUILayout.EnumPopup("UI_Type", viewtarget.uIType);

        //ViewModel.ReferenceType 이넘을 인스팩터에 표시해라."Reference_type"라는 이름으로...
        viewtarget.referenceType = (ViewModel.ReferenceType)EditorGUILayout.EnumPopup("Reference_type", viewtarget.referenceType);

        //viewtarget.uIType에서 선택된 이넘 값과 이름을 같은 클래스를 찾아서 Type를 저장
        Type selectedtype = derivedtype.FirstOrDefault(t => t.Name == viewtarget.uIType.ToString());
        Type basetype = selectedtype.BaseType;

        if (selectedtype != null)
        {
            // viewtarget.referenceType에서 선택된 값을 스트링으로 전환
            string na = viewtarget.referenceType.ToString();
            System.Type enumType;
            if (viewtarget.referenceType == ViewModel.ReferenceType.Field)
            {
                //viewtarget.referenceType에서 선택된 값을 가지고 selectedtype 클래스 안에 있는 필드를 찾는다.
                enumType = selectedtype.GetNestedType(na);
            }
            else
            {
                enumType = basetype.GetNestedType(na);
            }

           

            if (enumType != null && enumType.IsEnum) // 만일 enumType 필드가 이넘이면...
            {
                // 이넘의 모든 요소의 이름을 배열로 저장
                var enumnames = Enum.GetNames(enumType);
                //현재 viewtarget.SetectedValue에서 선택된 값의 인덱스를 찾는다.
                int selectedIndex = Array.IndexOf(enumnames, viewtarget.SetectedValue);
                if (selectedIndex == -1) selectedIndex = 0; //만약 선택된 값이 없다면 인덱스 번호 초기화

                //선택된 이넘 값을 가지고 "ValueName"값을 가지고 인스팩터 창에  나타낸다.
                selectedIndex = EditorGUILayout.Popup("ValueName", selectedIndex, enumnames);

                //인스팩터 창에서 선택된 값을 viewtarget.SetectedValue으로 저장한다.
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
           
                //ValueName 값을 통해 직접 클래스에 접근해서 값을 받아서 string으로 가져온다.


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
