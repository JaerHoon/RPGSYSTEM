using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPGSYSTEM.UI;
using System.Reflection;
using System.Linq;


public class ViewController : MonoBehaviour
{
    public enum UIType { PlayerInfo, SkillInfo }
    public List<ViewModel> viewModels = new List<ViewModel>();
    
    public PlayerInfo playerInfo;
    public SkillInfo skillInfo;

    public delegate void Eventchain();
    

    public void ChainMethod(View view, UIType uIType )
    {
        int num = (int)uIType;
        view.ButtonPressed += viewModels[num].OnClick;
        view.Ondrag += viewModels[num].OnDrag;
        view.Dragging += viewModels[num].Dragging;
        view.OffDrag += viewModels[num].OffDrag;
        view.Drop += viewModels[num].Drop;
     
    }

   public virtual object GetValue(UIType uIType, string valueName)
    {
       
        object value = default;
        object classins = default;
        string typeName = uIType.ToString();
        Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == typeName);
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == type)
            {
                classins = field.GetValue(this);
                break; 
            }
        }
        
        if (classins != null)
        {
         
            FieldInfo field = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                  .FirstOrDefault(f => f.Name == valueName);
           

            if (field != null)
            {
                value = field.GetValue(classins);
            }
        }

        return value;
    }

 
}