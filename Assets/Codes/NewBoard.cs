using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Reflection;
using System;
using UnityEditor;

public class NewBoard : MonoBehaviour
{
    public ViewElement boardInfo = new ViewElement();

    public virtual void SetBoardComponent()
    {
        //사실은 모든 자식을 검사해야함.
        List<Components> myComponents = Utility.FindAllComponentsInChildren<Components>(this.transform)
                                        .Where(co => co.hierarchyType == Components.HierarchyType.BoardInfo).ToList();

        foreach (Components com in myComponents)
        {
            switch (com.componentType)
            {
                case Components.ComponentType.Image:
                    Image image = com.GetComponent<Image>();
                    boardInfo.ImageComponents.Add(com.KeyName, image);
                    break;
                case Components.ComponentType.TextMeshProUGUI:
                    TextMeshProUGUI text = com.GetComponent<TextMeshProUGUI>();
                    boardInfo.textComopnent.Add(com.KeyName, text);
                    break;
                case Components.ComponentType.Slider:
                    Slider slider = com.GetComponent<Slider>();
                    boardInfo.sliderComponent.Add(com.KeyName, slider);
                    break;
            }
        }
    }
}

public class SlotBoard<T> : NewBoard
{
    public virtual void SetSlots()//예는 스타트에서 불러야 할듯?
    {
        SlotGroup slotGroups = GetComponentInChildren<SlotGroup>();
        List<ViewElement> viewElements = slotGroups.GetSlotcomponents();

        Type type = this.GetType();
        FieldInfo field = type.GetField(slotGroups.listName, BindingFlags.Public | BindingFlags.Instance);

        if (field != null)
        {
            var fieldValue = field.GetValue(this) as List<Slot<T>>;

            if (fieldValue != null)
            {
                fieldValue.Clear(); // 기존 리스트 초기화
                fieldValue.Capacity = viewElements.Count; // 리스트 용량 설정

                for (int i = 0; i < viewElements.Count; i++)
                {
                    Slot<T> slot = new Slot<T>();
                    slot.slotElement = viewElements[i];
                    fieldValue.Add(slot);
                }
            }
        }

    }

    public virtual void SetSlotData(T data)
    {
        
    }
}



public class ViewElement
{
    public Dictionary<string, Image> ImageComponents = new Dictionary<string, Image>();
    public Dictionary<string, TextMeshProUGUI> textComopnent = new Dictionary<string, TextMeshProUGUI>();
    public Dictionary<string, Slider> sliderComponent = new Dictionary<string, Slider>();
}

public class Components : MonoBehaviour
{
    public enum HierarchyType { BoardInfo, Slot, Card}
    [HideInInspector]
    public HierarchyType hierarchyType;

    public enum ComponentType { Image, TextMeshProUGUI, Slider}
    [HideInInspector]
    public ComponentType componentType;
    [HideInInspector]
    public NewBoard board;
    [HideInInspector]
    public SlotView slot;
    [HideInInspector]
    public string KeyName;
    [HideInInspector]
    public int SlotIndex;
}

[CustomEditor(typeof(Components), true)]
public class ComponentsViewEditor : Editor
{
    Components components;

    public override void OnInspectorGUI()
    {

        components = (Components)target;

        components.hierarchyType = (Components.HierarchyType)EditorGUILayout.EnumPopup("HierarchyType", components.hierarchyType);

        components.componentType = (Components.ComponentType)EditorGUILayout.EnumPopup("ComponentType", components.componentType);

        if (components.hierarchyType == Components.HierarchyType.Slot)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("SlotIndex", components.SlotIndex);
            EditorGUI.EndDisabledGroup();

            if (components.board != null)
            {
                Type type = components.board.GetType();

                string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(fi => fi.FieldType.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) || fi.FieldType.IsArray)
                                    .Select(fi => fi.Name).ToArray();

                if(fieldNames.Length > 0)
                {
                    int index = Array.IndexOf(fieldNames, components.KeyName);
                    if (index == -1) index = 0;
                    index = EditorGUILayout.Popup("KeyName", index ,fieldNames);

                    components.KeyName = fieldNames[index];
                }
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(components);
        }
    }
}


public class SlotGroup : MonoBehaviour
{
    [HideInInspector]
    public NewBoard board;
    [HideInInspector]
    public string listName;
    List<SlotView> slots = new List<SlotView>();

    public List<ViewElement> GetSlotcomponents()
    {
        List<ViewElement> slotsComponents = new List<ViewElement>();
        foreach(SlotView slot in slots)
        {
            slotsComponents.Add(slot.SlotComponents);
        }

        return slotsComponents;

    }

    public virtual void SetSlots()
    {
        for(int i=0; i < transform.childCount; i++)
        {
            SlotView slot = transform.GetChild(i).gameObject.AddComponent<SlotView>();
            slot.myBoard = board;
            slots.Add(slot);
        }

        slots = slots.OrderBy(slot => slot.SlotIndex).ToList();

    }

}

[CustomEditor(typeof(SlotGroup), true)]
public class SlotGroupEditor : Editor
{
    SlotGroup slotGroup;
    public override void OnInspectorGUI()
    {
        slotGroup = (SlotGroup)target;

        slotGroup.board = (NewBoard)EditorGUILayout.ObjectField("Board", slotGroup.board, typeof(NewBoard), true);

        if (slotGroup.board != null)
        {
            if (GUILayout.Button("SetSlots"))
            {
                slotGroup.SetSlots();
            }
        }

      

        if (GUI.changed)
        {
            EditorUtility.SetDirty(slotGroup);
        }
    }
}



public class SlotView : MonoBehaviour
{
    public NewBoard myBoard;
    public ViewElement SlotComponents = new ViewElement();
    public int SlotIndex;
    List<Components> components = new List<Components>();
   
    public virtual void SetSlotComponent()
    {
        components = Utility.FindAllComponentsInChildren<Components>(this.transform)
                                     .Where(co => co.hierarchyType == Components.HierarchyType.Slot).ToList();

        foreach (Components com in components)
        {
            com.slot = this;
            com.board = myBoard;
            com.SlotIndex = SlotIndex;
        }
    }
  
    public virtual void GetSlotComponent()
    {
        foreach (Components com in components)
        {
           
            switch (com.componentType)
            {
                case Components.ComponentType.Image:
                    Image image = com.GetComponent<Image>();
                    SlotComponents.ImageComponents.Add(com.KeyName, image);
                    break;
                case Components.ComponentType.TextMeshProUGUI:
                    TextMeshProUGUI text = com.GetComponent<TextMeshProUGUI>();
                    SlotComponents.textComopnent.Add(com.KeyName, text);
                    break;
                case Components.ComponentType.Slider:
                    Slider slider = com.GetComponent<Slider>();
                    SlotComponents.sliderComponent.Add(com.KeyName, slider);
                    break;
            }
        }
    }

    
}

[CustomEditor(typeof(SlotView), true)]
public class SlotViewEditor : Editor
{
    SlotView slot;

    public override void OnInspectorGUI()
    {
        slot = (SlotView)target;

        slot.SlotIndex = EditorGUILayout.IntField("SlotIdex", slot.SlotIndex);

        if (GUILayout.Button("SetComponent"))
        {
            slot.SetSlotComponent();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(slot);
        }
    }

}

public class Slot<T>
{
    public ViewElement slotElement = new ViewElement();
    T data;

    public virtual void SetData(T data)
    {
        this.data = data;
    }

    public virtual void SetValue(string Key, Sprite sprite)
    {
        if (slotElement.ImageComponents.TryGetValue(Key, out Image image))
        {
            // 키가 존재할 경우 이미지 설정
            image.sprite = sprite;
        }
    }
    public virtual void SetValue(string Key, string value)
    {
        if (slotElement.textComopnent.TryGetValue(Key, out TextMeshProUGUI tx))
        {
            // 키가 존재할 경우 이미지 설정
            tx.text = value;
        }
    }
    public virtual void SetValue(string Key, int maxValue, int curValue)
    {
        if (slotElement.sliderComponent.TryGetValue(Key, out Slider slider))
        {
            // 키가 존재할 경우 이미지 설정
            slider.maxValue = maxValue;
            slider.value = curValue;
        }
    }

    public virtual void SetValue(string Key, float maxValue, float curValue)
    {
        if (slotElement.sliderComponent.TryGetValue(Key, out Slider slider))
        {
            // 키가 존재할 경우 이미지 설정
            slider.maxValue = maxValue;
            slider.value = curValue;
        }

    }
}

public class Utility
{
    public static List<GameObject> FindChildrenWithTag(GameObject parent, string tag)
    {
        List<GameObject> taggedChildren = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                taggedChildren.Add(child.gameObject);
            }

            // Recursively search in the child's children
            taggedChildren.AddRange(FindChildrenWithTag(child.gameObject, tag));
        }
        return taggedChildren;
    }

    public static List<string> EnumToStringList<T>() where T : Enum
    {
        // Convert the array returned by Enum.GetNames to a list
        List<string> enumList = new List<string>(Enum.GetNames(typeof(T)));
        return enumList;
    }

    public static List<T> FindAllComponentsInChildren<T>(Transform parent) where T : class
    {
        List<T> components = new List<T>();

        // 현재 부모의 자식들을 순회합니다.
        foreach (Transform child in parent)
        {
            // 현재 child에서 T 타입의 컴포넌트를 찾습니다.
            T component = child.GetComponent<T>();

            // 컴포넌트가 있다면 리스트에 추가합니다.
            if (component != null)
            {
                components.Add(component);
            }

            // 재귀적으로 자식 오브젝트를 순회합니다.
            components.AddRange(FindAllComponentsInChildren<T>(child));
        }

        return components;
    }
}

