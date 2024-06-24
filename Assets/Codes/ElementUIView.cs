using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ElementUIView : MonoBehaviour
{
    public int elementIndex;

    public virtual Element GetElement()
    {
        Element element = new Element();
        List<ComponentUIView> ElementComponent = Utility.FindAllComponentsInChildren<ComponentUIView>(this.gameObject.transform);
        ElementComponent = ElementComponent.Where(element => element.uIClass == ComponentUIView.UIClass.Element).ToList();
        for(int i=0; i < ElementComponent.Count; i++)
        {
            switch (ElementComponent[i].elementcomponrntType)
            {
                case ComponentUIView.ElementUI_Component.ElementIcon:
                    ElementComponent[i].TryGetComponent<Image>(out element.ElementIcon); break;
                case ComponentUIView.ElementUI_Component.ElemnetNotice:
                    ElementComponent[i].TryGetComponent<TextMeshProUGUI>(out element.ElemnetNotice); break;
                case ComponentUIView.ElementUI_Component.ElementboardName:
                    ElementComponent[i].TryGetComponent<TextMeshProUGUI>(out element.ElementboardName); break;

            }
        }

        element.slots = GetSlotList();

        return element;
        
    }

    public virtual List<Slot> GetSlotList()
    {
        List<Slot> slots = new List<Slot>();
        List<SlotUIView> slotviews = Utility.FindAllComponentsInChildren<SlotUIView>(this.transform);
        foreach(SlotUIView slotUI in slotviews)
        {
            slotUI.SetSlot();
            slots.Add(slotUI.slot);
        }
        slots = slots.OrderBy(Slot => Slot.Slotnum).ToList();

        return slots;
    }
}
