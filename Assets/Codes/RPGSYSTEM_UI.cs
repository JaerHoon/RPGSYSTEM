using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPGSYSTEM;
using System;
using System.Reflection;
using UnityEditor;
using System.Linq;
using UnityEngine.EventSystems;

namespace RPGSYSTEM.UI
{
    [CustomEditor(typeof(UIView), true)]
    public class UIEditor : Editor
    {
        UIView UI;

        public override void OnInspectorGUI()
        {
            UI = (UIView)target;

            UI.uIType = (Enums.UIType)EditorGUILayout.EnumPopup("UI Type", UI.uIType);
            UI.uIController = (UIController)EditorGUILayout.ObjectField("UIController", UI.uIController, typeof(UIController), true);

            if (UI.uIController == null) return;
            if (UI.uIController.uIMoels.Count > 0)
            {
                string[] UIModelName = new string[UI.uIController.uIMoels.Count];
                for (int i = 0; i < UI.uIController.uIMoels.Count; i++)
                {
                    UIModelName[i] = UI.uIController.uIMoels[i].GetType().Name;

                }

                UI.UIModel_Index = EditorGUILayout.Popup("UIModelName", UI.UIModel_Index, UIModelName);


                UIModel uIModel = UI.uIController.uIMoels[UI.UIModel_Index];

                UI.uIModel = uIModel;

                Type type = uIModel.GetType();

                switch (UI.uIType)
                {
                    case Enums.UIType.Image: Image(type); break;
                    case Enums.UIType.GameObject: GameObject(type); break;
                    case Enums.UIType.Info: InFo(type); break;
                    case Enums.UIType.Slider: Slider(type); break;
                    case Enums.UIType.Slot: Slot(type); break;
                    case Enums.UIType.Text: Text(type); break;
                    case Enums.UIType.DragNDrop: DragNDrop(type); break;
                }

            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(UI);
            }
        }

        void DragNDrop(Type type)
        {
            UI.SlotNumber = EditorGUILayout.IntField("SlotNumber", UI.SlotNumber);
        }

        void Image(Type type)
        {
            string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(field => field.FieldType == typeof(Image))
                                  .Select(field => field.Name)
                                  .ToArray();
            string[] prorettyNames = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(pro => pro.PropertyType == typeof(Image))
                                    .Select(pro => pro.Name)
                                    .ToArray();

            string[] combinedArray = fieldNames.Concat(prorettyNames).ToArray();

            if (combinedArray.Length > 0)
            {
                int index = Array.IndexOf(combinedArray, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("fieldName", index, combinedArray);

                UI.ValueName = combinedArray[index];

                if (fieldNames.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Field;
                }
                else if (prorettyNames.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Property;
                }

            }


        }

        void Text(Type type)
        {
            string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(field =>
                                     field.FieldType == typeof(TextMeshProUGUI))
                                     .Select(field => field.Name)
                                     .ToArray();
            string[] prorettyNames = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(property =>
                                     property.PropertyType == typeof(TextMeshProUGUI)).Select(pro => pro.Name)
                                     .ToArray();

            string[] combinedArray = fieldNames.Concat(prorettyNames).ToArray();

            if (combinedArray.Length > 0)
            {
                int index = Array.IndexOf(combinedArray, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("fieldName", index, combinedArray);

                UI.ValueName = combinedArray[index];

                if (fieldNames.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Field;
                }
                else if (prorettyNames.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Property;
                }
            }

        }
        void Slider(Type type)
        {
            FieldInfo[] field = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            string[] fieldName = field.Where(field =>
                field.FieldType == typeof(Slider))
                .Select(field => field.Name)
               .ToArray();

            PropertyInfo[] property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string[] propertyName = property.Where(property =>
                property.PropertyType == typeof(Slider)).Select(pro => pro.Name)
            .ToArray();

            string[] combinedArray = fieldName.Concat(propertyName).ToArray();

            if (combinedArray.Length > 0)
            {
                int index = Array.IndexOf(combinedArray, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("fieldName", index, combinedArray);

                UI.ValueName = combinedArray[index];

                if (fieldName.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Field;
                }
                else if (propertyName.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Property;
                }
            }

        }

        void GameObject(Type type)
        {
            string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                               .Where(field => field.FieldType == typeof(GameObject))
                               .Select(fi => fi.Name)
                               .ToArray();

            string[] prorettyNames = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(property => property.PropertyType == typeof(GameObject))
                                   .Select(pro => pro.Name)
                                    .ToArray();

            string[] combinedArray = fieldNames.Concat(prorettyNames).ToArray();

            if (combinedArray.Length > 0)
            {
                int index = Array.IndexOf(combinedArray, UI.ValueName);
                if (index == -1) index = 0;
                index = EditorGUILayout.Popup("Field", index, combinedArray);

                UI.ValueName = combinedArray[index];

                if (fieldNames.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Field;
                }
                else if (prorettyNames.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Property;
                }

              
            }
            
        }
        void Slot(Type type)
        {
            string[] listName = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                              .Where(fi => fi.FieldType == typeof(List<Slot>))
                              .Select(fi => fi.Name).ToArray();

            if (listName.Length > 0)
            {
                int index = Array.IndexOf(listName, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("SlotList", index, listName);

                UI.ValueName = listName[index];

                UI.SlotNumber = EditorGUILayout.IntField("SlotNumber", UI.SlotNumber);

            }
            
        }

        void InFo(Type type)
        {
            string[] listName = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                             .Where(fi => fi.FieldType == typeof(Info))
                             .Select(fi => fi.Name).ToArray();

            if (listName.Length > 0)
            {
                int index = Array.IndexOf(listName, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("InfoName", index, listName);

                UI.ValueName = listName[index];

            }
        }

    
    }
    

    public class UIView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public Enums.UIType uIType;
       
        public enum valueType { Field, Property }
        [HideInInspector]
        public valueType value1_type;
       
        public UIController uIController;
        public int UIModel_Index;
      
        [HideInInspector]
        public UIModel uIModel;
       
      
        [HideInInspector]
        public string ValueName;
       

        [HideInInspector]
        public int SlotNumber;
        [HideInInspector]
        public Slot slot;
        [HideInInspector]
        public Info info;

        protected Image image;
        protected TextMeshProUGUI textMesh;
        protected Slider slider;

        public delegate void DargNDorpEvent(PointerEventData eventData, int SlotNum);
        public DargNDorpEvent Ondrag;
        public DargNDorpEvent Dragging;
        public DargNDorpEvent OffDrag;
        public DargNDorpEvent Drop;


        protected void Init()
        {
            SetComopnent(uIType);
        }

        protected void SetComopnent(Enums.UIType uIType)
        {
            switch (uIType)
            {
                case Enums.UIType.Image: 
                    TryGetComponent<Image>(out image);
                    Set<Image>(image);
                    break;
                case Enums.UIType.Text: 
                    TryGetComponent<TextMeshProUGUI>(out textMesh);
                    Set<TextMeshProUGUI>(textMesh);
                    break;
                case Enums.UIType.Slider: 
                    TryGetComponent<Slider>(out slider);
                    Set<Slider>(slider);
                    break;
                case Enums.UIType.GameObject: SetGameObject(); break;
                case Enums.UIType.Slot: SetSlot(); break;
                case Enums.UIType.Info: SetInfo(); break;
                case Enums.UIType.DragNDrop: SetDragNDrop(); break;
               
            }

          
        }
        protected virtual void SetDragNDrop()
        {
            uIModel.SetDragNDrop(ref Ondrag, ref Dragging, ref OffDrag, ref Drop);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            Ondrag?.Invoke(eventData, SlotNumber);
        }

        public void OnDrag(PointerEventData eventData)
        {

            Dragging?.Invoke(eventData, SlotNumber);
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            OffDrag?.Invoke(eventData, SlotNumber);
        }

        public void OnDrop(PointerEventData eventData)
        {
            Drop?.Invoke(eventData, SlotNumber);
        }


        protected void Set<T>(T component) where T: Component
        {
            uIModel.SetComponent<T>(component, ValueName, value1_type);
        }

      
        protected virtual void SetGameObject()
        {
            uIModel.SetGameObject(this.gameObject, ValueName, value1_type);
        }


        protected virtual void SetSlot()
        {
            slot = new Slot();
            slot.Slotnum = SlotNumber;
            slot.mySlot = this.gameObject;

            List<UIComponent> SlotComponent = Utility.FindAllComponentsInChildren<UIComponent>(this.gameObject.transform);
            for(int i = 0; i < SlotComponent.Count; i++)
            {
                switch (SlotComponent[i].componentType)
                {
                    case UIComponent.UI_Component.Icon: SlotComponent[i].TryGetComponent<Image>(out slot.Icon); break;
                    case UIComponent.UI_Component.Frame: SlotComponent[i].TryGetComponent<Image>(out slot.Frame); break;
                    case UIComponent.UI_Component.CoolTime1: SlotComponent[i].TryGetComponent<Image>(out slot.CoolTime); break;
                    case UIComponent.UI_Component.CoolTime2: SlotComponent[i].TryGetComponent<Image>(out slot.CoolTime2); break;
                    case UIComponent.UI_Component.Background: SlotComponent[i].TryGetComponent<Image>(out slot.Background); break;

                    case UIComponent.UI_Component.Description: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Description); break;
                    case UIComponent.UI_Component.Name: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Name); break;
                    case UIComponent.UI_Component.ID: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.ID); break;
                    case UIComponent.UI_Component.Lv: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.Lv); break;
                    case UIComponent.UI_Component.Grade: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out slot.grade); break;

                    case UIComponent.UI_Component.HPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.HPbar); break;
                    case UIComponent.UI_Component.MPbar: SlotComponent[i].TryGetComponent<Slider>(out slot.MPbar); break;
                    case UIComponent.UI_Component.EXPbar:SlotComponent[i].TryGetComponent<Slider>(out slot.EXPvar); break;

                }
            }

            uIModel.SetSLot(ValueName, slot);
        }

        protected virtual void SetInfo()
        {
            info = new Info();
            info.myObject = this.gameObject;
            List<UIComponent> SlotComponent = Utility.FindAllComponentsInChildren<UIComponent>(this.gameObject.transform);
            for (int i = 0; i < SlotComponent.Count; i++)
            {
                switch (SlotComponent[i].componentType)
                {
                    case UIComponent.UI_Component.Icon: SlotComponent[i].TryGetComponent<Image>(out info.Icon); break;
                    case UIComponent.UI_Component.Frame: SlotComponent[i].TryGetComponent<Image>(out info.Frame); break;
                    case UIComponent.UI_Component.CoolTime1: SlotComponent[i].TryGetComponent<Image>(out info.CoolTime); break;
                    case UIComponent.UI_Component.CoolTime2: SlotComponent[i].TryGetComponent<Image>(out info.CoolTime2); break;
                    case UIComponent.UI_Component.Background: SlotComponent[i].TryGetComponent<Image>(out info.Background); break;

                    case UIComponent.UI_Component.Description: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.Description); break;
                    case UIComponent.UI_Component.Name: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.Name); break;
                    case UIComponent.UI_Component.ID: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.ID); break;
                    case UIComponent.UI_Component.Lv: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.Lv); break;
                    case UIComponent.UI_Component.Grade: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out info.grade); break;

                    case UIComponent.UI_Component.HPbar: SlotComponent[i].TryGetComponent<Slider>(out info.HPbar); break;
                    case UIComponent.UI_Component.MPbar: SlotComponent[i].TryGetComponent<Slider>(out info.MPbar); break;
                    case UIComponent.UI_Component.EXPbar: SlotComponent[i].TryGetComponent<Slider>(out info.EXPvar); break;

                }
            }

            uIModel.SetInfo(ValueName, info);
        }

       
    }

    public class UIComponent : MonoBehaviour
    {
        public enum UI_Component { Name, ID, Icon, Frame, Lv, CoolTime1, CoolTime2, Description, Background, Grade, HPbar, MPbar, EXPbar }
        public UI_Component componentType;
    }
    public class Slot
    {
        public GameObject mySlot;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI ID;
        public TextMeshProUGUI grade;
        public Image Icon;
        public Image Frame;
        public Image CoolTime;
        public Image CoolTime2;
        public Image Background;
        public TextMeshProUGUI Lv;
        public TextMeshProUGUI Description;
        public Slider HPbar;
        public Slider MPbar;
        public Slider EXPvar;
        public int Slotnum;
        
    }


    public class Info
    {
        public GameObject myObject;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI ID;
        public TextMeshProUGUI grade;
        public Image Icon;
        public Image Frame;
        public Image CoolTime;
        public Image CoolTime2;
        public Image Background;
        public TextMeshProUGUI Lv;
        public TextMeshProUGUI Description;
        public Slider HPbar;
        public Slider MPbar;
        public Slider EXPvar;
    }

    public class UIController : MonoBehaviour
    {
       public List<UIModel> uIMoels = new List<UIModel>();
       
    }

    public class UIModel : MonoBehaviour 
    {
        public Action UPdateUI;
        public UIController uIController;
        
       
        public virtual void SetDragNDrop(ref UIView.DargNDorpEvent Ondrag, ref UIView.DargNDorpEvent drag, ref UIView.DargNDorpEvent offdrag, ref UIView.DargNDorpEvent drop)
        {
            Ondrag += OnDrag;
            drag += Dragging;
            offdrag += OffDrag;
            drop += Drop;
        }

        protected virtual void OnDrag(PointerEventData eventData, int slotnum)
        {

        }

        protected virtual void Dragging(PointerEventData eventData,int slotnum)
        {

        }

        protected virtual void OffDrag(PointerEventData eventData, int slotnum)
        {

        }

        protected virtual void Drop(PointerEventData eventData, int slotnum)
        {

        }

        public virtual void SetSLot(string SlotListName, Slot slot)
        {
            Type type = this.GetType();
            FieldInfo field = type.GetField(SlotListName, BindingFlags.Public | BindingFlags.Instance);

            if (field != null)
            {
                List<Slot> slots = field.GetValue(this) as List<Slot>;

                slots.Add(slot);

                slots.OrderBy(slot => slot.Slotnum).ToList();
            }

        }

        public virtual void SetInfo(string infoName, Info info)
        {
            Type type = this.GetType();
            FieldInfo field = type.GetField(infoName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(this, info);
            }

        }

        public virtual void SetComponent<T>(T comopnent, string compoName, UIView.valueType valueType) where T: Component
        {
            Type type = this.GetType();
            if ((int)valueType == 0)
            {
                FieldInfo field = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                .FirstOrDefault(f => f.Name == compoName);

                field.SetValue(this, comopnent);
               
            }
            else
            {
                PropertyInfo property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .FirstOrDefault(p => p.Name == compoName);

                property.SetValue(this, comopnent);
            }
        }

     
        public virtual void SetGameObject(GameObject gameObject, string fieldName, UIView.valueType valueType)
        {
            Type type = this.GetType();
            if((int)valueType == 0)
            {
                FieldInfo field = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                .FirstOrDefault(f => f.Name == fieldName);

                field.SetValue(this, gameObject);
            }
            else
            {
                PropertyInfo property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .FirstOrDefault(p => p.Name == fieldName);

                property.SetValue(this, gameObject);
            }
          
        }

       
    }
    public class UISlotModel : UIModel
    {
        protected List<Slot> slots = new List<Slot>();
        public Image dragImage;
        protected int dragIndex;
        protected GameObject canvas;
        int BeforSlotNum;

        protected virtual void Init()
        {
            dragImage.gameObject.SetActive(false);
        }

        protected override void OnDrag(PointerEventData eventData, int slotnum)
        {
            BeforSlotNum = slotnum;
            dragImage.sprite = slots[BeforSlotNum].Icon.sprite;
            dragImage.gameObject.transform.position = eventData.position;
            dragImage.gameObject.SetActive(true);

        }

        protected override void Dragging(PointerEventData eventData, int slotnum)
        {
            dragImage.gameObject.transform.position = eventData.position;
        }
           

        protected override void OffDrag(PointerEventData eventData, int slotnum)
        {
            dragImage.gameObject.SetActive(false);
        }

        protected override void Drop(PointerEventData eventData, int slotnum)
        {
            if(slotnum != BeforSlotNum)
            {
                slots[slotnum].Icon.sprite = dragImage.sprite;
                ChageSLot();
            }
            else
            {
                return;
            }
        }

        protected virtual void ChageSLot()
        {
            //드래그 앤 드롭으로 슬롯의 이미지가 빠져나갔을때 처리하는 함수.
        }

        public virtual void Setting()
        {
            //슬롯과 데이터 간에 연동하는 함수
            // 데이터 수 만큼 세팅을 해주고, 데이터 수에 없는건? 디폴트로 해준다. 혹은 비활성화 한다.
        }

        public virtual void OnClick(int num)
        {
            //데이터[num]을 인포로 넘겨준다.(info에 함수 만들기)
        }


    }


}