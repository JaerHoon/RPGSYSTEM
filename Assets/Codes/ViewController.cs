using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPGSYSTEM.UI;
using System.Reflection;


public class ViewController : MonoBehaviour
{
    public enum UIType { PlayerInfo, SkillInfo }
    public List<ViewModel> viewModels = new List<ViewModel>();
 
    public PlayerInfo playerInfo;
    public SkillInfo skillInfo;

    [SerializeField]
    protected System.Enum ViewModeEnums;

    public virtual System.Type GetUIEnum(UIType uiType)
    {
        return typeof( ViewModel.ReferenceType);// �ʵ����� �޼ҵ����� ��ȯ

        /*switch (uiType)
        {
            case UIType.PlayerInfo:
                return typeof(PlayerInfo.ReferenceType);
            case UIType.SkillInfo:
                return typeof(SkillInfo.FieldName);
            default:
                Debug.LogError("Unknown UIType");
                return null;
        }*/
    }

    public virtual System.Type GetTypeEnum(UIType uiType, System.Enum _enum) // �Ű������� �ʵ����� �޼ҵ����� �޾ƿ�.
    {
        int num = (int)uiType;
        System.Type type = viewModels[num].GettypeEnums(_enum); // �ʵ� Ȥ�� �޼��� �̳��� �޾ƿ´�.
        return type; // �ʵ� Ȥ�� �޼��� �̳� ��ȯ.
    }

}