using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM;
using RPGSYSTEM.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuUIModel 
{
    public Menu menu;
    public int menuIndex;

    public virtual void OnClick(int num, List<List<SlotData>> slotDatas, Image image)// 메뉴페널이 처음 열렸을때 호출하는 함수
    {
    }
    public virtual void SetBoardUIMode(List<BoardUIModel> boardUIModels)
    {
     
    }
    public virtual void SetSlotDatas(int num, List<List<SlotData>> slotdatas, Image image)//각 데이터를 받아오는 함수.오버라이드 해서 써야함.
    {
      
    }
}

public class BoardUIModel 
{
    public Board board;
   
    
    public virtual void SetBoardElements(List<BoardUIElement> uIElements)
    {
       board.BoardElements.AddRange(uIElements);
    }

    public virtual void SetElementData(List<List<SlotData>> datas, Image image)
    {
        for(int i = 0; i < datas.Count; i++)
        {
           board.BoardElements[i].SetSlotdata(datas[i], image);
        }
    }


    public virtual void OnBoard()
    {
       board.myboard.SetActive(true);
    }

    public virtual void OffBoard()
    {
       board.myboard.SetActive(false);
    }
}

public class BoardUIElement
{
    protected int dragIndex;
    int BeforSlotNum;

    public Element element;
    public Image dragImage;

    public virtual void SetSlots(List<Slot> slotlist)
    {
        element.slots.AddRange(slotlist);
    }

    public virtual void SetSlotdata(List<SlotData> data, Image image)
    {
        dragImage = image;

        element.datas.AddRange(data);

        for(int i=0; i < element.datas.Count; i++)
        {
            element.slots[i].SetSlotData(element.datas[i]);
        }

        NoneSetSlotData();
    }

    public virtual void NoneSetSlotData()
    {
        for(int i=0; i < element.slots.Count; i++)
        {
            if(element.slots[i].slotData == null)
            {
                element.slots[i].DefaultSet();
            }
                
        }
    }

    public virtual void SetDragNDrop(ref UIView.DargNDorpEvent Ondrag, ref UIView.DargNDorpEvent drag, ref UIView.DargNDorpEvent offdrag, ref UIView.DargNDorpEvent drop)
    {
        Ondrag += OnDrag;
        drag += Dragging;
        offdrag += OffDrag;
        drop += Drop;
    }
    protected virtual void OnDrag(PointerEventData eventData, int slotnum)
    {
      

    }

    protected virtual void Dragging(PointerEventData eventData, int slotnum)
    {
        
    }


    protected virtual void OffDrag(PointerEventData eventData, int slotnum)
    {
       
    }

    protected virtual void Drop(PointerEventData eventData, int slotnum)
    {
        
    }

    protected virtual void ChageSLot()
    {
        //드래그 앤 드롭으로 슬롯의 이미지가 빠져나갔을때 처리하는 함수.
    }

    public virtual void Setting()
    {
        //슬롯과 데이터 간에 연동하는 함수
        // 데이터 수 만큼 세팅을 해주고, 데이터 수에 없는건? 디폴트로 해준다. 혹은 비활성화 한다.
    }

    public virtual void OnClick(int num)
    {
        //데이터[num]을 인포로 넘겨준다.(info에 함수 만들기)
    }



}
