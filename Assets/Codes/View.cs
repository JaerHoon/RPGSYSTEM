using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM.UI;
using UnityEngine.UI;
using System;
using System.Reflection;

public class View : MonoBehaviour
{
    public enum ViewType { Image, TextMeshProGUGI, Button, DragNDrop }
    [HideInInspector]
    public ViewType viewType;

    public ViewController viewController;
    
    [HideInInspector]
    public ViewController.UIType uIType;
    [HideInInspector]
    public ViewModel.ReferenceType referenceType;
    [HideInInspector]
    public string SetectedValue;
    [HideInInspector]
    public string valueText;
    [HideInInspector]
    public object value;


    public ViewController.Eventchain ButtonPressed;
    public ViewController.Eventchain Ondrag;
    public ViewController.Eventchain Dragging;
    public ViewController.Eventchain OffDrag;
    public ViewController.Eventchain Drop;

    public virtual void ChainMethod()
    {
        ButtonPressed = null;
        Ondrag = null;
        Dragging = null;
        OffDrag = null;
        Drop = null;
        viewController.ChainMethod(this, uIType);
    }

}

