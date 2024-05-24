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
    private ViewController viewController;

    public ViewController ViewController
    {
        get { return viewController; }
        set { viewController = value; }
    }

    [HideInInspector]
    [SerializeField]
    private ViewController.UIType uIType;

    public ViewController.UIType UIType
    {
        get { return uIType; }
        set
        {
            uIType = value;
            OnUITypeChanged(); // UIType이 변경될 때마다 호출
        }
    }

    [SerializeField]
    private System.Enum enumValue;

    public System.Enum EnumValue
    {
        get { return enumValue; }
        set { enumValue = value; }
    }

    private void OnValidate()
    {
        if (viewController != null)
        {
            System.Type enumType = viewController.GetUIEnum(uIType);
            if (enumType != null)
            {
                System.Array enumValues = System.Enum.GetValues(enumType);
                if (enumValues.Length > 0)
                {
                    enumValue = (System.Enum)enumValues.GetValue(0);
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
                    enumValue = (System.Enum)enumValues.GetValue(0);
                }
            }
        }
    }
}