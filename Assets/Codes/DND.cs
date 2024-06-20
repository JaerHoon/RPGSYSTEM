using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGSYSTEM;
using RPGSYSTEM.UI;
using UnityEngine.EventSystems;
using System;
   
public class DND : UIView
{
    private void Awake()
    {
        uIType = Enums.UIType.DragNDrop;
        SetDragNDrop();
    }

   
    

}
