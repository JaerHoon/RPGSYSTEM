using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RPGSYSTEM;
using UnityEngine.UI;
using TMPro;

public class BoardUIView : MonoBehaviour
{
    public List<ElementUIView> elementUIViews = new List<ElementUIView>();
    public int boardIndex;
   
  
    public virtual List<Element> GetElements()
    {
        List<Element> Elements = new List<Element>();
        foreach(ElementUIView uIView in elementUIViews)
        {
            Elements.Add(uIView.GetElement());
        }

        return Elements;
    }

    public virtual Board GetBoard()
    {
        Board board = new Board();
        board.BoardIndex = boardIndex;
        board.myboard = this.gameObject;
        List<ComponentUIView> BoardComponent = Utility.FindAllComponentsInChildren<ComponentUIView>(this.gameObject.transform);
        BoardComponent = BoardComponent.Where(board => board.uIClass == ComponentUIView.UIClass.Board).ToList();
        for(int i=0; i< BoardComponent.Count; i++)
        {
            switch (BoardComponent[i].boardcomponentType)
            {
                case ComponentUIView.BoardUI_Component.BoardIcon:
                    BoardComponent[i].TryGetComponent<Image>(out board.BoardIcon); break;
                case ComponentUIView.BoardUI_Component.boardNotice:
                    BoardComponent[i].TryGetComponent<TextMeshProUGUI>(out board.boardNotice); break;
                case ComponentUIView.BoardUI_Component.boardName:
                    BoardComponent[i].TryGetComponent<TextMeshProUGUI>(out board.boardName); break;

            }
        }
        board.elements = GetElements();

        return board;
        
    }

    public virtual void SetChildElement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ElementUIView elementUI = transform.GetChild(i).gameObject.AddComponent<ElementUIView>();
            elementUI.elementIndex = i;

            elementUIViews.Add(elementUI);
        }

        elementUIViews = elementUIViews.OrderBy(el => el.elementIndex).ToList();

    }
}
