using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM.UI;
using System;
public class PlayerInfo : ViewModel
{
    public enum Field { Hp, Mp }

    public int Hp;
    public int Mp;

    public override void OnClick()
    {
        print("HI!! Onclick");
    }


    public override void OnDrag()
    {
        print("OnDrag");
    }
    public override void Dragging()
    {
        print("Dragging");
    }

    public override void Drop()
    {
        print("Drop");
    }

    public override void OffDrag()
    {
        print("OffDrag");
    }
}