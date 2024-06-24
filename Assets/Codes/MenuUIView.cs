using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuUIView : MonoBehaviour
{  
    [HideInInspector]
    public UIController UIController;
    [HideInInspector]
    public int MenuIndex;
    [HideInInspector]
    public UIModel UIModel;
    [HideInInspector]
    public List<BoardUIView> boardUIViews = new List<BoardUIView>();
  
    public virtual void GetUIcontroller()
    {
        UIController = transform.parent.GetComponent<UIController>();
    }

    public virtual void SetMenuUIModel()
    {
        UIModel.SetMenuModel(GetMenu());
    }

    public virtual void SetChildBoard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BoardUIView board = transform.GetChild(i).gameObject.AddComponent<BoardUIView>();
            board.SetChildElement();
            board.boardIndex = i;
            boardUIViews.Add(board);
        }

        boardUIViews = boardUIViews.OrderBy(UI => UI.boardIndex).ToList();

    }

    public virtual List<Board> GetBoard()
    {
        List<Board> boardUIModels = new List<Board>();
        for(int i=0; i < boardUIViews.Count; i++)
        {
            Board boardUI = boardUIViews[i].GetBoard();
            boardUI.BoardIndex = boardUIViews[i].boardIndex;
            boardUIModels.Add(boardUI);
        }

        boardUIModels = boardUIModels.OrderBy(UI => UI.BoardIndex).ToList();

        return boardUIModels;
    }

    public virtual Menu GetMenu()
    {
        Menu menu = new Menu();
        menu.boards = GetBoard();

        return menu;
    }
}

