using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM.UI;
using UnityEngine.UI;
using System;
using System.Reflection;

public class View : MonoBehaviour
{
    public enum ViewType { Image, ImageSlot, Text, TextSlot, Button, ButtonSlot, DragNDrop, DragNDropSlot }
    [HideInInspector]
    public ViewType viewType;
    [HideInInspector]
    public int slotNumber;

    public ViewController viewController;
    
    [HideInInspector]
    public ViewController.UIType uIType;
    
    [HideInInspector]
    public string SetectedValue;
    [HideInInspector]
    public string SetectedValue1;
    [HideInInspector]
    public string SetectedValue2;
    [HideInInspector]
    public string SetectedValue3;
    [HideInInspector]
    public string valueText;
    [HideInInspector]
    public object value;
    [HideInInspector]
    public string SelectedValue1;


    public ViewController.Eventchain ButtonPressed;
    public ViewController.Eventchain Ondrag;
    public ViewController.Eventchain Dragging;
    public ViewController.Eventchain OffDrag;
    public ViewController.Eventchain Drop;

    public ViewController.SlotEventchain SlotButtonPressed;
    public ViewController.SlotEventchain SlotOndrag;
    public ViewController.SlotEventchain SlotDragging;
    public ViewController.SlotEventchain SlotOffDarg;
    public ViewController.SlotEventchain SlotDrop;

    public void printdelegate()
    {
        PrintDelegateMethods("Button" ,ButtonPressed);
        PrintDelegateMethods("OnDrag" ,Ondrag);
        PrintDelegateMethods("Dragging",Dragging);
        PrintDelegateMethods("OffDrag",OffDrag);
        PrintDelegateMethods("Drop",Drop);
    }

    void PrintDelegateMethods(string name,ViewController.Eventchain eventchain)
    {
        if (eventchain != null)
        {
            Delegate[] invocationList = eventchain.GetInvocationList();
            foreach (Delegate del in invocationList)
            {
                Debug.Log(name+ " : " + del.Method.Name + " from " + del.Target);
            }
        }
        else
        {
            Debug.Log("Delegate is null or has no methods.");
        }
    }

    public void Onclick()
    {
        ButtonPressed?.Invoke();
    }

    public void OnDrag()
    {
        Ondrag?.Invoke();
    }

    public void DraggingMethod()
    {
        Dragging?.Invoke();
    }

    public void offDragMethod()
    {
        OffDrag.Invoke();
    }

    public void DropMethod()
    {
        Drop?.Invoke();
    }

   

}

