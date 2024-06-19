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

namespace RPGSYSTEM.UI
{
    [CustomEditor(typeof(UIView), true)]
    public class UIEditor : Editor
    {
        UIView UI;

        public override void OnInspectorGUI()
        {
            UI = (UIView)target;
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
                    case Enums.UIType.Info: Info(type); break;
                    case Enums.UIType.Slider: Slider(type); break;
                    case Enums.UIType.Slot: Slot(type); break;
                    case Enums.UIType.Text: Text(type); break;
                }

            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(UI);
            }
        }

        void Image(Type type)
        {
            string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(field => field.FieldType == typeof(Sprite))
                                  .Select(field => field.Name)
                                  .ToArray();
            string[] prorettyNames = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(pro => pro.PropertyType == typeof(Sprite))
                                    .Select(pro => pro.Name)
                                    .ToArray();

            string[] combinedArray = fieldNames.Concat(prorettyNames).ToArray();

            if (combinedArray.Length > 0)
            {
                int index = Array.IndexOf(combinedArray, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("ValueName", index, combinedArray);

                UI.ValueName = combinedArray[index];
            }

            object va = default;

            if (fieldNames.Contains(UI.ValueName))
            {
                UI.value1_type = UIView.valueType.Field;
            }
            else if (prorettyNames.Contains(UI.ValueName))
            {
                UI.value1_type = UIView.valueType.Property;
            }
            va = UI.uIModel.GetValue(UI.ValueText, UI.value1_type);



            Sprite sp = va as Sprite;

            if (sp != null)
            {
                UI.Value = va;
                UI.ValueText = sp.name;
            }
            else
            {
                UI.ValueText = "null";
            }

            UI.ValueText = EditorGUILayout.TextField("SpriteName", UI.ValueText);
        }

        void Text(Type type)
        {
            string[] fieldNames = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(field =>
                                     field.FieldType == typeof(int) ||
                                     field.FieldType == typeof(float) ||
                                     field.FieldType == typeof(string) || (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>) && (
                                     field.FieldType.GetGenericArguments()[0] == typeof(int) ||
                                     field.FieldType.GetGenericArguments()[0] == typeof(float))))
                                     .Select(field => field.Name)
                                     .ToArray();
            string[] prorettyNames = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(property =>
                                     property.PropertyType == typeof(int) ||
                                     property.PropertyType == typeof(float) ||
                                     property.PropertyType == typeof(string) ||
                                     (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                     (property.PropertyType.GetGenericArguments()[0] == typeof(int) ||
                                     property.PropertyType.GetGenericArguments()[0] == typeof(float)))).Select(pro => pro.Name)
                                     .ToArray();

            string[] combinedArray = fieldNames.Concat(prorettyNames).ToArray();

            if (combinedArray.Length > 0)
            {
                int index = Array.IndexOf(combinedArray, UI.ValueName);
                if (index == -1) index = 0;

                index = EditorGUILayout.Popup("ValueName", index, combinedArray);

                UI.ValueName = combinedArray[index];
            }

            object va = default;

            if (fieldNames.Contains(UI.ValueName))
            {
                UI.value1_type = UIView.valueType.Field;
            }
            else if (prorettyNames.Contains(UI.ValueName))
            {
                UI.value1_type = UIView.valueType.Property;
            }
            va = UI.uIModel.GetValue(UI.ValueText, UI.value1_type);

            int? intva = va as int?;

            UI.Value = va;

            if (intva != null)
            {
                UI.ValueText = intva.ToString();
            }
            else
            {
                UI.ValueText = "null";
            }

            UI.ValueText = EditorGUILayout.TextField("Value", UI.ValueText);
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

                UI.slot = UI.uIModel.GetSlotList(UI.ValueName, UI.SlotNumber);

            }

        }

        void Info(Type type)
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

                Info va = UI.uIModel.GetValue(UI.ValueName, UIView.valueType.Field) as Info;

                if (va != null)
                {
                    UI.info = va;
                    UI.ValueText = va.ToString();
                }
                else
                {
                    UI.ValueText = null;
                }

                UI.ValueText = EditorGUILayout.TextField("Valuetype", UI.ValueText);


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

                UI.uIModel.SetGameObject(UI.gameObject, UI.ValueName, UI.value1_type);
            }

        }

        void Slider(Type type)
        {
            FieldInfo[] field = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            string[] fieldName = field.Where(field =>
                field.FieldType == typeof(int) ||
                field.FieldType == typeof(float))
                .Select(field => field.Name)
               .ToArray();

            PropertyInfo[] property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string[] propertyName = property.Where(property =>
                property.PropertyType == typeof(int) ||
                property.PropertyType == typeof(float)).Select(pro => pro.Name)
            .ToArray();

            string[] conbinedArray = fieldName.Concat(propertyName).ToArray();

            if (conbinedArray.Length > 0)
            {
                int index = Array.IndexOf(conbinedArray, UI.ValueName);
                if (index == -1) index = 0;
                index = EditorGUILayout.Popup("MaxName", index, conbinedArray);

                UI.ValueName = conbinedArray[index];

                object va = default;

                if (fieldName.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Field;
                }
                else if (propertyName.Contains(UI.ValueName))
                {
                    UI.value1_type = UIView.valueType.Property;
                }

                va = UI.uIModel.GetValue(UI.ValueName, UI.value1_type);
                UI.Value = va;
                UI.ValueText = va.ToString();
                UI.ValueText = EditorGUILayout.TextField("Maxvalue", UI.ValueText);

                int index2 = Array.IndexOf(conbinedArray, UI.ValueName2);
                if (index2 == -1) index2 = 0;
                index2 = EditorGUILayout.Popup("CurName", index2, conbinedArray);

                UI.ValueName2 = conbinedArray[index2];

                object va2 = default;

                if (fieldName.Contains(UI.ValueName2))
                {
                    UI.value2_type = UIView.valueType.Field;
                }
                else if (propertyName.Contains(UI.ValueName2))
                {
                    UI.value2_type = UIView.valueType.Property;
                }

                va2 = UI.uIModel.GetValue(UI.ValueName2, UI.value2_type);
                UI.Value2 = va2;
                UI.ValueText2 = va2.ToString();
                UI.ValueText2 = EditorGUILayout.TextField("Curvalue", UI.ValueText2);
            }
        }
    }

    public class UIView : MonoBehaviour
    {
        public Enums.UIType uIType;
        public enum IsSlotType { None, Slot}
        public IsSlotType isSlottype;

        public enum valueType { Field, Property }
        [HideInInspector]
        public valueType value1_type;
        [HideInInspector]
        public valueType value2_type;

        public UIController uIController;
        public int UIModel_Index;
      
        [HideInInspector]
        public UIModel uIModel;
        [HideInInspector]
        public object Value;
        [HideInInspector]
        public object Value2;
        [HideInInspector]
        public object Value3;
        [HideInInspector]
        public string ValueName;
        [HideInInspector]
        public string ValueName2;
        [HideInInspector]
        public string ValueName3;
        [HideInInspector]
        public string ValueText;
        [HideInInspector]
        public string ValueText2;
        [HideInInspector]
        public string ValueText3;

        [HideInInspector]
        public int SlotNumber;
        [HideInInspector]
        public Slot slot;
        [HideInInspector]
        public Info info;

        protected Image image;
        protected TextMeshProUGUI textMesh;
        protected Slider slider;


        protected TextMeshProUGUI SlotName_Text;
        protected TextMeshProUGUI SlotLv_Text;
        protected TextMeshProUGUI SlotDescription_Text;
        protected TextMeshProUGUI SlotID_Text;
        protected Image SlotIconImage;
        protected Image SlotCoolTimeImage1;
        protected Image SlotCoolTimeImage2;
        protected Image SlotFrameImage;
        protected Image SlotBackGroundImage;

       

        private void OnEnable()
        {
            uIModel.UPdateUI += UIUpdate;
        }

        private void Start()
        {
            Init();
        }
        protected void Init()
        {
            SetComopnent(uIType);
           
        }

        protected void SetComopnent(Enums.UIType uIType)
        {
            switch (uIType)
            {
                case Enums.UIType.Image: TryGetComponent<Image>(out image); break;
                case Enums.UIType.Text: TryGetComponent<TextMeshProUGUI>(out textMesh); break;
                case Enums.UIType.Slider: TryGetComponent<Slider>(out slider); break;
                case Enums.UIType.GameObject: SetGameObject(); break;
                case Enums.UIType.Slot: SetSlot(); break;
            }

            UIUpdate();
        }

        public virtual void UIUpdate()
        {
            switch (this.uIType)
            {
                case Enums.UIType.Image: ImageUPdate(); break;
                case Enums.UIType.Text: TextMeshProGUGIUpdate(); break;
                case Enums.UIType.Slider: SliderUPdate(); break;
                case Enums.UIType.Slot: SlotUPdate(); break;
                
            }
        }

        protected virtual void ImageUPdate()
        {
            Value = uIModel.GetValue(ValueName, value1_type);
            if(Value != null)
            {
                image.sprite = (Sprite)Value;
            }
            else
            {
                image.sprite = default;
            }
        }

        protected virtual void SliderUPdate()
        {
            Value = uIModel.GetValue(ValueName, value1_type);
            Value2 = uIModel.GetValue(ValueName2, value2_type);

            slider.maxValue = Value != null ? (float)Value : 0;
            slider.value = Value2 != null ? (float)Value2 : 0;
        }

        protected virtual void TextMeshProGUGIUpdate()
        {
            Value = uIModel.GetValue(ValueName, value1_type);
            textMesh.text = Value?.ToString() ?? null;
        }

        protected virtual void SetGameObject()
        {
            uIModel.SetGameObject(this.gameObject, ValueName, value1_type);
        }


        protected virtual void SetSlot()
        {
            List<UISlot_component> SlotComponent = Utility.FindAllComponentsInChildren<UISlot_component>(this.gameObject.transform);
            for(int i = 0; i < SlotComponent.Count; i++)
            {
                switch (SlotComponent[i].componentType)
                {
                   
                    case UISlot_component.SlotComponent.Icon: SlotComponent[i].TryGetComponent<Image>(out SlotIconImage); break;
                    case UISlot_component.SlotComponent.Frame: SlotComponent[i].TryGetComponent<Image>(out SlotFrameImage); break;
                    case UISlot_component.SlotComponent.CoolTime1: SlotComponent[i].TryGetComponent<Image>(out SlotCoolTimeImage1); break;
                    case UISlot_component.SlotComponent.CoolTime2: SlotComponent[i].TryGetComponent<Image>(out SlotCoolTimeImage2); break;
                    case UISlot_component.SlotComponent.Background: SlotComponent[i].TryGetComponent<Image>(out SlotBackGroundImage); break;

                    case UISlot_component.SlotComponent.Description: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out SlotDescription_Text); break;
                    case UISlot_component.SlotComponent.Name: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out SlotName_Text); break;
                    case UISlot_component.SlotComponent.ID: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out SlotID_Text); break;
                    case UISlot_component.SlotComponent.Lv: SlotComponent[i].TryGetComponent<TextMeshProUGUI>(out SlotLv_Text); break;

                }
            }

            slot = uIModel.GetSlotList(ValueName, SlotNumber);
        }

        protected virtual void SlotUPdate()
        {
            if (SlotName_Text != null) SlotName_Text.text = slot.Name;
            if (SlotID_Text != null) SlotID_Text.text = slot.ID.ToString();
            if (SlotLv_Text != null) SlotLv_Text.text = slot.Lv.ToString();
            if (SlotDescription_Text != null) SlotDescription_Text.text = slot.Description;
            if (SlotIconImage != null) SlotIconImage.sprite = slot.Icon;
            if (SlotFrameImage != null) SlotFrameImage.sprite = slot.Frame;
            if (SlotCoolTimeImage1 != null) SlotCoolTimeImage1.fillAmount = slot.CoolTime;
            if (SlotCoolTimeImage2 != null) SlotCoolTimeImage2.fillAmount = slot.CoolTime2;
            if (SlotBackGroundImage != null) SlotBackGroundImage.sprite = slot.Background;
        }

        private void OnDisable()
        {
            uIModel.UPdateUI -= UIUpdate;
        }

    }

    public class UISlot_component : MonoBehaviour
    {
        public enum SlotComponent { Name, ID, Icon, Frame, Lv, CoolTime1, CoolTime2, Description, Background }
        public SlotComponent componentType;
    }
    public class Slot
    {
        public string Name;
        public int ID;
        public Sprite Icon;
        public Sprite Frame;
        public float CoolTime;
        public float CoolTime2;
        public Sprite Background;
        public int Lv;
        public string Description;
       
    }

    public class UIController : MonoBehaviour
    {
       public List<UIModel> uIMoels = new List<UIModel>();
       
    }

    public class UIModel : MonoBehaviour
    {
        public Action UPdateUI;
        public virtual object GetValue(string valueName, UIView.valueType valueType)
        {
            object obj = default;

            Type type = this.GetType();
            if ((int)valueType == 0)
            {
                FieldInfo field = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                .FirstOrDefault(f => f.Name == valueName);

                obj = field.GetValue(this);
            }
            else
            {
                PropertyInfo property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .FirstOrDefault(p => p.Name == valueName);

                obj = property.GetValue(this);
;            }
            return obj;
        }

        public virtual Slot GetSlotList(string listName, int index)
        {
            Slot slot = default;

            Type type = this.GetType();

            FieldInfo field = type.GetField(listName, BindingFlags.Public | BindingFlags.Instance);

            if(field != null)
            {
                List<Slot> slots = field.GetValue(this) as List<Slot>;

                slot = slots[index];
            }

            return slot;
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

    

    public class Info
    {

    }
}