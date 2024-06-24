using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPGSYSTEM;

public class Slot 
{
    public SlotData slotData;
    public Enums.SlotType slotType;
    public GameObject mySlot;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI ID;
    public TextMeshProUGUI grade;
    public Image Icon;
    public Image Frame;
    public Image CoolTime;
    public Image CoolTime2;
    public Image Background;
    public TextMeshProUGUI Lv;
    public TextMeshProUGUI Description;
    public Slider HPbar;
    public Slider MPbar;
    public Slider EXPbar;
    public int Slotnum;

    public virtual void SetSlotData(SlotData data)
    {
        if(Name !=null) Name.text = data.Name;
        if (ID != null) ID.text = data.ID;
        if (grade!= null) grade.text = data.grade;
        if (Icon != null) Icon.sprite = data.Icon;
        if (Frame != null) Frame.sprite = data.Frame;
        if (Background != null) Background.sprite = data.Background;
        if (Lv != null) Lv.text = data.Lv;
        if (Description != null) Description.text = data.Description;
        if (HPbar != null) HPbar.maxValue = data.HPbar.MaxValue;
        if (HPbar != null) HPbar.value = data.HPbar.CurValue;
        if (MPbar != null) MPbar.maxValue = data.MPbar.MaxValue;
        if (MPbar != null) MPbar.value = data.MPbar.CurValue;
        if (EXPbar != null) EXPbar.maxValue = data.EXPbar.MaxValue;
        if (EXPbar != null) EXPbar.value = data.EXPbar.CurValue;
    }

    public virtual void DefaultSet()
    {
       //아무데이터도 담지 않은 슬롯에 대한 처리.. 
       // 게임오브젝트를 비활성화 할수도 있고
       //디폴트 상태를 정의해서 그렇게 만들어 줄 수도 있다.
    }

}

public class SlotData
{
    public string Name;
    public string ID;
    public string grade;
    public Sprite Icon;
    public Sprite Frame;
    public Sprite Background;
    public string Lv;
    public string Description;
    public SliderBar HPbar;
    public SliderBar MPbar;
    public SliderBar EXPbar;

}

public struct SliderBar
{
    public int MaxValue;
    public int CurValue;
}


public class Board
{
    public GameObject myboard;
    public List<BoardUIElement> BoardElements = new List<BoardUIElement>();
    public List<Element> elements = new List<Element>();
  
    public int BoardIndex;
    public Image BoardIcon;
    public TextMeshProUGUI boardNotice;
    public TextMeshProUGUI boardName;


    public void SetDragImage(Image image)
    {
       for(int i=0; i < elements.Count; i++)
        {
            elements[i].SetDragImage(image);
        }
    }
    public void SetBoardData(BoaedData data)
    {
        BoardIcon.sprite = data.BoardIcon;
        boardNotice.text = data.boardNotice;
        boardName.text = data.boardName;
        

        for(int i = 0; i < data.elementDatas.Count; i++)
        {
            elements[i].SetElementData(data.elementDatas[i]);
        }
    }

    public virtual void OnBoard()
    {
        myboard.SetActive(true);
    }

    public virtual void OffBoard()
    {
        myboard.SetActive(false);
    }
}

public class BoaedData
{
    public List<ElementData> elementDatas = new List<ElementData>();
    public Sprite BoardIcon;
    public string boardNotice;
    public string boardName;

   
}

public class ElementData
{
    public List<SlotData> slotDatas = new List<SlotData>();
    public Sprite ElementIcon;
    public string ElemnetNotice;
    public string ElementboardName;

}

public class Element
{
    public Image dragImage;
    public Image ElementIcon;
    public TextMeshProUGUI ElemnetNotice;
    public TextMeshProUGUI ElementboardName;
    public int elementIndex;
    public List<Slot> slots = new List<Slot>();
    public List<SlotData> datas = new List<SlotData>();

    public void SetElementData(ElementData data)
    {
        ElementIcon.sprite = data.ElementIcon;
        ElemnetNotice.text = data.ElemnetNotice;
        ElementboardName.text = data.ElementboardName;

        for(int i=0; i < data.slotDatas.Count; i++)
        {
            slots[i].SetSlotData(data.slotDatas[i]);   
        }
    }

    public void SetDragImage(Image image)
    {
        dragImage = image;
    }
    
}

public class Menu
{
    public List<Board> boards = new List<Board>();

    public void SetDragImage(Image image)
    {
        for(int i=0; i < boards.Count; i++)
        {
            boards[i].SetDragImage(image);
        }
    }
       
}

public class MenuData
{
    public List<BoaedData> boaedDatas = new List<BoaedData>();
}

   