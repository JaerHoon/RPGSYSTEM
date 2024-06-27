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

    public virtual void SetSlots(SlotGroup slotGroup)
    {

    }

    public virtual void SetSlotGroups(SlotGroup slotGroup)
    {

    }
}

public class SlotBoard<T> : NewBoard
{
    public List<SlotGroup> slotGroups = new List<SlotGroup>();
   
    public virtual void Init()
    {
        if (slotGroups.Count > 0)
        {
            for(int i = 0; i < slotGroups.Count; i++)
            {
                slotGroups[i].SetSlots();
                SetSlots(slotGroups[i]);
            }
        }
        else
        {
            Debug.Log("슬롯그룹이 없습니다");
        }
    }

    public override void SetSlots(SlotGroup slotGroup)
    {
        //슬롯<T> 리스트가 여러개 일 수 있다.(예를 들어 EquipSlot이랑 invenSlot이랑.. 
        //그래서 상속받아서 사용할때 만들었는데 그 리스틀 채우기 위해서 타입을 사용했음.
        List<ViewElement> viewElements = slotGroup.GetSlotcomponents();

        Type type = this.GetType();
        FieldInfo field = type.GetField(slotGroup.listName, BindingFlags.Public | BindingFlags.Instance);

        if (field != null)
        {
            List<Slot<T>> fieldValue = field.GetValue(this) as List<Slot<T>>;

            if (fieldValue != null)
            {   
                fieldValue.Clear(); // 기존 리스트 초기화
               
                for (int i = 0; i < viewElements.Count; i++)
                {
                    Slot<T> slot = new Slot<T>();
                    slot.slotElement = viewElements[i];
                    fieldValue.Add(slot);
                }
            }

        }
    }

    public override void SetSlotGroups(SlotGroup slotGroup)
    {
        if (!slotGroups.Contains(slotGroup))
        {
            slotGroups.Add(slotGroup);
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
    public string listName;
    [HideInInspector]
    public string KeyName;
    [HideInInspector]
    public int SlotIndex;

    public void SetParents(SlotView slotView)
    {
        board = slotView.myBoard;
        slot = slotView;
        SlotIndex = slotView.SlotIndex;
    }
        
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
            SlotView slotView = Utility.FindComponentInParent<SlotView>(components.transform);

            if(slotView != null)
            {
                components.SetParents(slotView);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("SlotIndex", components.SlotIndex);
                EditorGUI.EndDisabledGroup();

                if (components.board != null)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField("BoardName", components.board.name);
                    EditorGUI.EndDisabledGroup();

                    Type type = components.board.GetType();

                    string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(fi => fi.FieldType.IsArray)
                                        .Select(fi => fi.Name).ToArray();

                    if (fieldNames.Length > 0)
                    {
                        int index = Array.IndexOf(fieldNames, components.listName);
                        if (index == -1) index = 0;
                        index = EditorGUILayout.Popup("FieldArray", index, fieldNames);

                        components.listName = fieldNames[index];

                        FieldInfo fieldInfo = type.GetField(fieldNames[index]);

                        if (fieldInfo != null)
                        {
                            var obj = fieldInfo.GetValue(components.board);
                            string[] fields = obj as string[];

                            if (fields.Length > 0)
                            {
                                int index2 = Array.IndexOf(fields, components.KeyName);
                                if (index2 == -1) index2 = 0;
                                index2 = EditorGUILayout.Popup("KeyName", index2, fields);

                                components.KeyName = fields[index2];
                            }
                        }
                    }
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
    public List<SlotView> slots = new List<SlotView>();


    public List<ViewElement> GetSlotcomponents()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetSlotComponent();
        }

        List<ViewElement> slotsComponents = new List<ViewElement>();
        foreach(SlotView slot in slots)
        {
            slotsComponents.Add(slot.SlotComponents);
        }

        return slotsComponents;

    }

    public virtual void SetSlots()
    {
        slots.Clear();
        for(int i=0; i < transform.childCount; i++)
        {
            SlotView slot = transform.GetChild(i).GetComponent<SlotView>();
            if(slot == null)
            {
                slot = transform.GetChild(i).gameObject.AddComponent<SlotView>();
            }

            slot.myBoard = board;
            slot.SlotIndex = i;
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

        slotGroup.board = Utility.FindComponentInParent<NewBoard>(slotGroup.transform);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("BoardName", slotGroup.board.name);
        EditorGUI.EndDisabledGroup();

        slotGroup.board.SetSlotGroups(slotGroup);

        if (slotGroup.board != null)
        {
            if (GUILayout.Button("SetSlots"))
            {
                slotGroup.SetSlots();

            }

            Type type = slotGroup.board.GetType();

            
            string[] listNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                            .Where(fi => fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(List<>) || fi.FieldType.IsArray)
                            .Select(fi => fi.Name).ToArray();

            if (listNames.Length > 0)
            {
                int index = Array.IndexOf(listNames, slotGroup.listName);
                if (index == -1) index = 0;
                index = EditorGUILayout.Popup("ListName", index, listNames);

                slotGroup.listName = listNames[index];
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
  
  
    public virtual void GetSlotComponent()
    {
        components = Utility.FindAllComponentsInChildren<Components>(this.transform)
                                    .Where(co => co.hierarchyType == Components.HierarchyType.Slot).ToList();
        SlotComponents.ImageComponents.Clear();
        SlotComponents.sliderComponent.Clear();
        SlotComponents.textComopnent.Clear();
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

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("BoardName", slot.myBoard.name);
        EditorGUI.EndDisabledGroup();

      

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
    public static T FindComponentInParent<T>(Transform transform) where T : class
    {
        // 현재 게임 오브젝트의 부모를 추적합니다.
        Transform currentTransform = transform;

        while (currentTransform != null)
        {
            // 현재 부모에서 컴포넌트를 찾습니다.
            T component = currentTransform.GetComponent<T>();
            if (component != null)
            {
                // 컴포넌트를 찾은 경우 반환합니다.
                return component;
            }

            // 부모가 없으면 null을 반환합니다.
            currentTransform = currentTransform.parent;
        }

        // 모든 부모를 탐색했지만 컴포넌트를 찾지 못한 경우 null을 반환합니다.
        return null;
    }
}




