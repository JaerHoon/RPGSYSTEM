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
        return typeof( ViewModel.ReferenceType);// 필드인지 메소드인지 반환

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

    public virtual System.Type GetTypeEnum(UIType uiType, System.Enum _enum) // 매개변수로 필드인지 메소드인지 받아옴.
    {
        int num = (int)uiType;
        System.Type type = viewModels[num].GettypeEnums(_enum); // 필드 혹은 메서드 이넘을 받아온다.
        return type; // 필드 혹은 메서드 이넘 반환.
    }

}