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
            print("menuData����");
        }
        
    }

    public virtual void SetMenuData(MenuData menuData)
    {
        this.menuData = menuData;

        //�� �����͸� �޾ƿ��� �Լ� �������̵� �ؼ� �����.
        //�� ������ Ŭ������ ������ ���̸� ����� Get�ؼ� �����.
        // BoardElenmnet ������ŭ List<SlotData>����Ʈ ��Ұ� �����ϴ� ��. 
    }
}
