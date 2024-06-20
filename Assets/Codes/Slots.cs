using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : UISlotModel
{
    public Image DragImage;

    private void Start()
    {
        DragImage.gameObject.SetActive(false);
    }

    protected override void OnDrag(PointerEventData eventData, int slotnum)
    {
        DragImage.sprite = eventData.pointerPress.gameObject.GetComponent<Image>().sprite;
        DragImage.gameObject.transform.position = eventData.position;
    }

    protected override void Dragging(PointerEventData eventData, int slotnum)
    {
        DragImage.gameObject.transform.position = eventData.position;
    }


    protected override void OffDrag(PointerEventData eventData, int slotnum)
    {

    }

    protected override void Drop(PointerEventData eventData, int slotnum)
    {
        eventData.pointerPress.gameObject.GetComponent<Image>().sprite = DragImage.sprite;
        DragImage.gameObject.SetActive(false);
    }

}
