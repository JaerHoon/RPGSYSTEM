using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM;

public class ComponentUIView : MonoBehaviour
{
    public enum UIClass { Board, Element, Slot}
    [HideInInspector]
    public UIClass uIClass;

    
    public enum SlotUI_Component { Name, ID, Icon, Frame, Lv, CoolTime1, CoolTime2, Description, Background, Grade, HPbar, MPbar, EXPbar }
    [HideInInspector]
    public SlotUI_Component slotcomponentType;

    public enum BoardUI_Component { BoardIcon, boardNotice, boardName }
    [HideInInspector]
    public BoardUI_Component boardcomponentType;

    public enum ElementUI_Component { ElementIcon, ElemnetNotice, ElementboardName }
    [HideInInspector]
    public ElementUI_Component elementcomponrntType;
}
