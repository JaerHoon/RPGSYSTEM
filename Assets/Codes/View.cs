using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM.UI;
using System;
using System.Reflection;
using UnityEditor;
public class View : MonoBehaviour
{
    [SerializeField]
    protected ViewController viewController;

    public ViewController ViewController
    {
        get { return viewController; }
        set { viewController = value; }
    }

    #if UNITY_EDITOR
    [HideInInspector]
    [SerializeField]
    protected ViewController.UIType uIType;// 어느 클래스에 속하는지

    public ViewController.UIType UIType
    {
        get { return uIType; }
        set
        {
            uIType = value;
            OnUITypeChanged(); // UIType이 변경될 때마다 호출
        }
    }

    [HideInInspector]
    [SerializeField]
    protected System.Enum valueType; // 어떤 타입을 가져올건지 필드값인지 메소드값인지

    public System.Enum SelectType
    {
        get { return valueType; }
        set
        { 
            valueType = value;
            OnUITypeChanged();
        }
    }

    [HideInInspector]
    [SerializeField]
    protected System.Enum Value; // 필드면 필드 값중에 뭔지, 메소드면 메소드 값중에 뭔지

    public System.Enum SelectValue
    { 

        get { return Value; }
        set { Value = value; }
    }


    private void OnValidate()
    {
        if (viewController != null) 
        {
            //viewController를 통해서 선택한 이넘이름의 클래스 타입을 가져온다.
            System.Type enumType = viewController.GetUIEnum(uIType);  
            if (enumType != null)
            {
                // 이넘의 클래스 안에 있는 타입의 모든 요소를 배열로 가져온다.
                System.Array enumValues = System.Enum.GetValues(enumType);
                if (enumValues.Length > 0)
                {
                    // 배열을 포함한 0번째 요소로를 할당한다. 필드인지 메소드인지
                    valueType = (System.Enum)enumValues.GetValue(0);
                }
            }
          
            // 필드혹은 메소드 이넘 받아옴
            System.Type enType = viewController.GetTypeEnum(uIType, valueType);
            if (enumType != null)
            {
                // 이넘의 클래스 안에 있는 타입의 모든 요소를 배열로 가져온다.
                System.Array enumValue = System.Enum.GetValues(enType);
                if (enumValue.Length > 0)
                {
                    // 배열을 포함한 0번째 요소로를 할당한다. 필드인지 메소드인지
                    valueType = (System.Enum)enumValue.GetValue(0);
                }
            }

        }
      

    }

    private void OnUITypeChanged()
    {
        if (viewController != null)
        {
            System.Type enumType = viewController.GetUIEnum(uIType);
            if (enumType != null)
            {
                System.Array enumValues = System.Enum.GetValues(enumType);
                if (enumValues.Length > 0)
                {
                    valueType = (System.Enum)enumValues.GetValue(0);
                }
            }
        }
    }

    public System.Type Getvaluetype()
    {
        System.Type enumType = viewController.GetUIEnum(uIType);

        return enumType;
    }
#endif
}