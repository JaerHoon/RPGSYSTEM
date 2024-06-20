using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SlotText : UISlotModel
{
    public List<Image> images = new List<Image>(); 
    public Image DragImage;

    int BeforeSlotNum;
    int AfterSlotNum;

    private void Start()
    {
        DragImage.gameObject.SetActive(false);
    }

    protected override void OnDrag(PointerEventData eventData, int slotnum)
    {
        BeforeSlotNum = slotnum;
        DragImage.sprite = images[BeforeSlotNum].sprite;
        images[BeforeSlotNum].sprite = default;
        DragImage.gameObject.transform.position = eventData.position;
        DragImage.gameObject.SetActive(true);
    }

    protected override void Dragging(PointerEventData eventData, int slotnum)
    {
        
        DragImage.gameObject.transform.position = eventData.position;
    }


    protected override void OffDrag(PointerEventData eventData, int slotnum)
    {
     
        DragImage.gameObject.SetActive(false);
    }

    protected override void Drop(PointerEventData eventData, int slotnum)
    {
        
        images[slotnum].sprite = DragImage.sprite;
        
    }
}
