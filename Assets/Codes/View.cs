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
    protected ViewController.UIType uIType;// ��� Ŭ������ ���ϴ���

    public ViewController.UIType UIType
    {
        get { return uIType; }
        set
        {
            uIType = value;
            OnUITypeChanged(); // UIType�� ����� ������ ȣ��
        }
    }

    [HideInInspector]
    [SerializeField]
    protected System.Enum valueType; // � Ÿ���� �����ð��� �ʵ尪���� �޼ҵ尪����

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
    protected System.Enum Value; // �ʵ�� �ʵ� ���߿� ����, �޼ҵ�� �޼ҵ� ���߿� ����

    public System.Enum SelectValue
    { 

        get { return Value; }
        set { Value = value; }
    }


    private void OnValidate()
    {
        if (viewController != null) 
        {
            //viewController�� ���ؼ� ������ �̳��̸��� Ŭ���� Ÿ���� �����´�.
            System.Type enumType = viewController.GetUIEnum(uIType);  
            if (enumType != null)
            {
                // �̳��� Ŭ���� �ȿ� �ִ� Ÿ���� ��� ��Ҹ� �迭�� �����´�.
                System.Array enumValues = System.Enum.GetValues(enumType);
                if (enumValues.Length > 0)
                {
                    // �迭�� ������ 0��° ��ҷθ� �Ҵ��Ѵ�. �ʵ����� �޼ҵ�����
                    valueType = (System.Enum)enumValues.GetValue(0);
                }
            }
          
            // �ʵ�Ȥ�� �޼ҵ� �̳� �޾ƿ�
            System.Type enType = viewController.GetTypeEnum(uIType, valueType);
            if (enumType != null)
            {
                // �̳��� Ŭ���� �ȿ� �ִ� Ÿ���� ��� ��Ҹ� �迭�� �����´�.
                System.Array enumValue = System.Enum.GetValues(enType);
                if (enumValue.Length > 0)
                {
                    // �迭�� ������ 0��° ��ҷθ� �Ҵ��Ѵ�. �ʵ����� �޼ҵ�����
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