using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGSYSTEM;
using TMPro;
using System.Linq;

public class SlotUIView : MonoBehaviour
{
    public Slot slot;
    public int SlotIndex;
    public Enums.SlotType slotType;

    public virtual void SetSlot()
    {
        slot = new Slot();
        slot.Slotnum = SlotIndex;
        slot.slotType = this.slotType;
        slot.mySlot = this.gameObject;

        List<ComponentUIView> SlotComponent = Utility.FindAllComponentsInChildren<ComponentUIView>(this.gameObject.transform);
        SlotComponent = SlotComponent.Where(slot => slot.uIClass == ComponentUIView.UIClass.Slot).ToList();

        for (int i = 0; i < SlotComponent.Count; i++)
        {
            switch (SlotComponent[i].slotcomponentType)
            {
                case ComponentUIView.SlotUI_Component.Icon: SlotComponent[i].TryGetComponent<Image>(out slot.Icon); break;
                case ComponentUIView.SlotUI_Component.Frame: SlotComponent[i].TryGetComponent<Image>(out slot.Frame); break;
                case ComponentUIView.SlotUI_Component.CoolTime1: SlotComponent[i].TryGetComponent<Image>(out slot.CoolTime); break;
                case ComponentUIView.SlotUI_Component.CoolTime2: SlotComponent[i].TryGetComponent<Image>(out slot.CoolTime2); break;
                case ComponentUIView.SlotUI_Component.Background: SlotComponent[i].TryGetComponent<Image>(out slot.Background); break;

                case ComponentUIView.SlotUI_Component.Description: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Description); break;
                case ComponentUIView.SlotUI_Component.Name: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Name); break;
                case ComponentUIView.SlotUI_Component.ID: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.ID); break;
                case ComponentUIView.SlotUI_Component.Lv: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Lv); break;
                case ComponentUIView.SlotUI_Component.Grade: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.grade); break;

                case ComponentUIView.SlotUI_Component.HPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.HPbar); break;
                case ComponentUIView.SlotUI_Component.MPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.MPbar); break;
                case ComponentUIView.SlotUI_Component.EXPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.EXPbar); break;

            }
        }

       
    }
}


