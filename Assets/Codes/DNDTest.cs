using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPGSYSTEM.UI;
using RPGSYSTEM;
using UnityEngine.UI;
using TMPro;

public class DNDTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Enums.UIType uIType;

    public enum valueType { Field, Property }
    [HideInInspector]
    public valueType value1_type;

    public UIController uIController;
    public int UIModel_Index;

    [HideInInspector]
    public UIModel uIModel;


    [HideInInspector]
    public string ValueName;


    [HideInInspector]
    public int SlotNumber;
    [HideInInspector]
    public Slot slot;
    [HideInInspector]
    public Info info;

    protected Image image;
    protected TextMeshProUGUI textMesh;
    protected Slider slider;

    public delegate void DargNDorpEvent(PointerEventData eventData, int SlotNum);
    public DargNDorpEvent OnDrag;
    public DargNDorpEvent Dragging;
    public DargNDorpEvent OffDrag;
    public DargNDorpEvent Drop;


    protected void Init()
    {
        SetComopnent(uIType);
    }

    protected void SetComopnent(Enums.UIType uIType)
    {
        switch (uIType)
        {
            case Enums.UIType.Image:
                TryGetComponent<Image>(out image);
                Set<Image>(image);
                break;
            case Enums.UIType.Text:
                TryGetComponent<TextMeshProUGUI>(out textMesh);
                Set<TextMeshProUGUI>(textMesh);
                break;
            case Enums.UIType.Slider:
                TryGetComponent<Slider>(out slider);
                Set<Slider>(slider);
                break;
            case Enums.UIType.GameObject: SetGameObject(); break;
            case Enums.UIType.Slot: SetSlot(); break;
            case Enums.UIType.Info: SetInfo(); break;
            case Enums.UIType.DragNDrop: SetDragNDrop(); break;
        }


    }

    protected void SetDragNDrop()
    {
        //uIModel.SetDragNDrop(ref OnDrag, ref Dragging, ref OffDrag, ref Drop);


    }

    protected void Set<T>(T component) where T : Component
    {
       // uIModel.SetComponent<T>(component, ValueName, value1_type);
    }


    protected virtual void SetGameObject()
    {
        //uIModel.SetGameObject(this.gameObject, ValueName, value1_type);
    }


    protected virtual void SetSlot()
    {
        slot = new Slot();
        slot.Slotnum = SlotNumber;
        slot.mySlot = this.gameObject;

        List<UIComponent> SlotComponent = Utility.FindAllComponentsInChildren<UIComponent>(this.gameObject.transform);
        for (int i = 0; i < SlotComponent.Count; i++)
        {
            switch (SlotComponent[i].componentType)
            {
                case UIComponent.UI_Component.Icon: SlotComponent[i].TryGetComponent<Image>(out slot.Icon); break;
                case UIComponent.UI_Component.Frame: SlotComponent[i].TryGetComponent<Image>(out slot.Frame); break;
                case UIComponent.UI_Component.CoolTime1: SlotComponent[i].TryGetComponent<Image>(out slot.CoolTime); break;
                case UIComponent.UI_Component.CoolTime2: SlotComponent[i].TryGetComponent<Image>(out slot.CoolTime2); break;
                case UIComponent.UI_Component.Background: SlotComponent[i].TryGetComponent<Image>(out slot.Background); break;

                case UIComponent.UI_Component.Description: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Description); break;
                case UIComponent.UI_Component.Name: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Name); break;
                case UIComponent.UI_Component.ID: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.ID); break;
                case UIComponent.UI_Component.Lv: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Lv); break;
                case UIComponent.UI_Component.Grade: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.grade); break;

                case UIComponent.UI_Component.HPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.HPbar); break;
                case UIComponent.UI_Component.MPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.MPbar); break;
                case UIComponent.UI_Component.EXPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.EXPvar); break;

            }
        }

        uIModel.SetSLot(ValueName, slot);
    }

    protected virtual void SetInfo()
    {
        info = new Info();
        info.myObject = this.gameObject;
        List<UIComponent> SlotComponent = Utility.FindAllComponentsInChildren<UIComponent>(this.gameObject.transform);
        for (int i = 0; i < SlotComponent.Count; i++)
        {
            switch (SlotComponent[i].componentType)
            {
                case UIComponent.UI_Component.Icon: SlotComponent[i].TryGetComponent<Image>(out info.Icon); break;
                case UIComponent.UI_Component.Frame: SlotComponent[i].TryGetComponent<Image>(out info.Frame); break;
                case UIComponent.UI_Component.CoolTime1: SlotComponent[i].TryGetComponent<Image>(out info.CoolTime); break;
                case UIComponent.UI_Component.CoolTime2: SlotComponent[i].TryGetComponent<Image>(out info.CoolTime2); break;
                case UIComponent.UI_Component.Background: SlotComponent[i].TryGetComponent<Image>(out info.Background); break;

                case UIComponent.UI_Component.Description: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.Description); break;
                case UIComponent.UI_Component.Name: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.Name); break;
                case UIComponent.UI_Component.ID: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.ID); break;
                case UIComponent.UI_Component.Lv: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.Lv); break;
                case UIComponent.UI_Component.Grade: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.grade); break;

                case UIComponent.UI_Component.HPbar: SlotComponent[i].TryGetComponent<Slider>(out info.HPbar); break;
                case UIComponent.UI_Component.MPbar: SlotComponent[i].TryGetComponent<Slider>(out info.MPbar); break;
                case UIComponent.UI_Component.EXPbar: SlotComponent[i].TryGetComponent<Slider>(out info.EXPvar); break;

            }
        }

        uIModel.SetInfo(ValueName, info);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("드래그 시작");
        OnDrag?.Invoke(eventData, SlotNumber);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        print("드래그");
        Dragging?.Invoke(eventData, SlotNumber);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        print("드래그끝");
        OffDrag?.Invoke(eventData, SlotNumber);
    }

    public void OnDrop(PointerEventData eventData)
    {
        print("드롭");
    }
}
