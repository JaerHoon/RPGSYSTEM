using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModel : MonoBehaviour
{
    public Menu menu;
    public Image dragImage;
    public MenuData menuData;

    public virtual void SetMenuModel(Menu menu)
    {
        this.menu = menu;
        this.menu.SetDragImage(dragImage);
    }

    public virtual void OnClickMenu(int num)
    {
        if(menuData != null)
        {
            menu.boards[num].SetBoardData(menuData.boaedDatas[num]);
            menu.boards[num].OnBoard();
        }
        else
        {
            print("menuData없음");
        }
        
    }

    public virtual void SetMenuData(MenuData menuData)
    {
        this.menuData = menuData;

        //각 데이터를 받아오는 함수 오버라이드 해서 써야함.
        //각 데이터 클래스에 데이터 더미를 만들고 Get해서 써야함.
        // BoardElenmnet 개수만큼 List<SlotData>리스트 요소가 존재하는 것. 
    }
}
