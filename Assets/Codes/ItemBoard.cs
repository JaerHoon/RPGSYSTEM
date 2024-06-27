using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoard : SlotBoard<ItemData>
{
    public List<Slot<ItemData>> invenSlots = new List<Slot<ItemData>>();
    public List<Slot<ItemData>> equipSlots = new List<Slot<ItemData>>();

    public string[] boardFields = { "Name", "Icon", "CharacterName" };
    public string[] slotFields = { "Icon", "Frame", "BackGround", "Lv", "Name" };

    public Sprite Icon;
    public Sprite Frame;
    public Sprite BackGround;
    public string Lv;
    public string Name;

    private void Start()
    {
        Init();

        for(int i=0; i< invenSlots.Count; i++)
        {
            invenSlots[i].SetValue(slotFields[0], Icon);
            invenSlots[i].SetValue(slotFields[1], Frame);
            invenSlots[i].SetValue(slotFields[2], BackGround);
            invenSlots[i].SetValue(slotFields[3], Lv);
            invenSlots[i].SetValue(slotFields[4], Name);
        }
    }

}
