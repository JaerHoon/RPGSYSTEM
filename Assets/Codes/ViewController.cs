using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPGSYSTEM.UI;

public class ViewController : MonoBehaviour
{
    public enum UIType { PlayerInfo, SkillInfo }
    public PlayerInfo playerInfo;
    public SkillInfo skillInfo;


    public System.Type GetUIEnum(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.PlayerInfo:
                return typeof(PlayerInfo.FieldName);
            case UIType.SkillInfo:
                return typeof(SkillInfo.FieldName);
            default:
                Debug.LogError("Unknown UIType");
                return null;
        }
    }
}