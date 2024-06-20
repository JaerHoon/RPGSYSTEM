using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPGSYSTEM.UI;

namespace RPGSYSTEM
{

    public class Managers<T> : MonoBehaviour where T : class
    {
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    return default(T);
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (null == instance)
            {
                instance = default(T);
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
       
    }

   
    public class CharacterManager : Managers<CharacterManager> 
    {
        protected SkillManager skillManager;

        [SerializeField]
        protected List<CharacterData> characterDatas = new List<CharacterData>();
        protected List<Character> characters = new List<Character>();
        protected List<Character> ativeCharacters = new List<Character>();
        protected Character[] inBattleCharacters = new Character[Nums.CharacterEquipCount];

        protected virtual void Init()
        {
            for (int i = 0; i < characterDatas.Count; i++)
            {
                Character chars = new Character(characterDatas[i]);
                chars.InitSkill(skillManager.charaterskills[i]);
                characters.Add(chars);
            }
        }

        public virtual void GetCharacter(int characterNum)// ???????? ???????? ????????
        {
            ativeCharacters.Add(characters[characterNum]);
        }

        //???????? ???? ???? ?????? ?????????? ???? ????
        public virtual void EquipCharacter(int slotnum, int characterNum)
        {   
            for(int i=0; i < inBattleCharacters.Length; i++)
            {
                if (i != slotnum)
                {
                    if(inBattleCharacters[i] == characters[characterNum])
                    {
                        inBattleCharacters[i] = null;
                    }
                }
                else
                {
                    inBattleCharacters[i] = characters[characterNum];
                }
            }
        }

        //?????? UI???? ????. ?????????? ?????? ?????????? ???? ????
        public void EquipEquipment(int characterNum , Equipment equipment) 
        {
            characters[characterNum].OnEquip(equipment);
            equipment.equipmentStat = Equipment.EquipmentStat.Equip;
        }

        //?????????? ?????? ???????? ???? ????
        public void RemoveEquipment(int characterNum, Equipment equipment)
        {
            characters[characterNum].RemoveEquipment((int)equipment.equipmentType);
            equipment.equipmentStat = Equipment.EquipmentStat.Gain;
        }

        // ?????? ???????????? ?????????? ???? ???????? ???????? ?????? ?????????? ????
        public void LossEquipment(Equipment equipment)
        {
            foreach(Character car in characters)
            {
                car.LossEquipment(equipment);
            }
        }
    }

    public class Character
    {
        protected CharacterData characterData; // ?????? ??????
        protected int[] stats = new int[Nums.StatCount]; // ?????????? ????
        protected int[] stats_Lv = new int[Nums.StatCount];//???? ???? ???? ???? ???? ??????
        protected List<int>[] temp_stat = new List<int>[Nums.StatCount]; // ?????? ?????? ?????? ???????? ???? ???????? ???????? ????
        protected int lv=1; //???????? ????
        protected int maxLv = Nums.CharacterMaxLv; // ???????? ???? ???? ????
        protected List<Skill> skills = new List<Skill>(); // ?????? ???? ????
        protected Skill[] equipSkills = new Skill[Nums.SkillSetCount]; // ?????? ?????????? ?????? ???? ????
        protected Equipment[] equipments = new Equipment[Nums.EquipmentCount]; // ?????? ?????? ?????? ???? ????

        public int Lv
        {
            get
            {
                return lv;
            }

            set
            {
                if (value > maxLv)
                {
                    lv = maxLv;
                }
                else
                {
                    lv = value;
                }
            }
        }

        public Character(CharacterData data)
        {
            characterData = data;
            Init();
        }

        protected virtual void Init()
        {

        }

        public void InitSkill(List<Skill> Characterskills)// ?????? ???? ???? ?????? ???????? ????
        {
            skills.AddRange(Characterskills);
        }

        public virtual T GetStat<T>(int stat, bool Istemp) // stat ???????? ?? T?? ???????? ????
        {
            int st = 0;
            if(Istemp == true)// ??????????
            {
               st = Cal.CalStat(stat, stats_Lv[stat], equipments, temp_stat);
            }
            else//???????? ??????
            {
                st = Cal.CalStat(stat, stats_Lv[stat], equipments);
            }
           
            return (T)Convert.ChangeType(st, typeof(T));
        }

        public virtual void OnEquip(Equipment equipment)//???? ?????? ????????
        {
            equipments[(int)equipment.equipmentType] = equipment;
        }

        public virtual void RemoveEquipment(int slotnum)//???????????? ???? ????.
        {
            equipments[slotnum] = null;
        }

        //???????? ???????????? ?????????? ?????????????? ???? ???????? ????
        public virtual void LossEquipment(Equipment equipment)
        {
            for(int i = 0; i < equipments.Length; i++)
            {
                if(equipments[i] == equipment)
                {
                    equipments[i] = null;
                }
            }
        }

        public virtual void StatUP(int stat, int amount)// ?? ???? ???????? ???? ????
        {
            stats[stat] += Cal.CalStatUP(stats[stat], amount);
        }

        public virtual void StatUP(int stat, float amount)// ?? ???? ???????? ???? ???? ????????
        {
            stats[stat] += Cal.CalStatUP(stats[stat], amount); 
        }

        public virtual void Temp_statUp(int stat, int amount)// ?????? ???????? ?????? ?????????? ????
        {
            temp_stat[stat].Add(amount);
           
        }

        public virtual void Temp_statRestore(int stat, int amount)//???? ???? ???? ???????? ????.
        {
            temp_stat[stat].Remove(amount);
        }

        public void Temp_Reset()//???? ???????? ?????? ???? ?????? ?????? ???????? ????
        {
            for(int i = 0; i < temp_stat.Length; i++)
            {
                temp_stat[i].Clear();
            }
        }

        public virtual void EquipSkill(int slotnum,Skill skill)// ???? ???? ????
        {
            for(int i = 0; i < skills.Count; i++)
            {
                if(skills[i] = skill)
                {
                    equipSkills[slotnum] = skill;
                }
            }
        }



    }

    public class SkillManager : Managers<SkillManager>
    {
        [SerializeField]
        protected List<Skill> allSkills = new List<Skill>(); // ???? ???? ?????? ???????? ?????? ????
        public List<List<Skill>> charaterskills = new List<List<Skill>>(); // ?? ?????????? ?????? ???????? ????

        public virtual void ListingSkill()// ?? ???????????? ???????? ???? ???????? ???? ????
        {
            for (int i = 0; i < allSkills.Count; i++)
            {
                if (allSkills[i].characterNum == i)
                {
                    charaterskills[i].Add(allSkills[i]);
                }
            }
        }
    }

    public class Skill : MonoBehaviour 
    {
        protected SkillData skillData;
        public Enums.SkillType skillType;
        public int characterNum; // ?? ?????? ???? ?????? ????????
        protected int skillLv=1;
        protected int maxSkillLv = Nums.SkillMaxLv;
        public int Skill_Lv
        {
            get
            {
                return skillLv;
            }
            set
            {
                if (value > maxSkillLv)
                {
                    skillLv = maxSkillLv;
                }
                else
                {
                    skillLv = value;
                }
            }
        }

        public virtual void OnSkill()
        {
            //???????? ???????????? ?????? ?????? ??????????.
        }

        public virtual T GetSkillPow<T>()// ?????? ?????? ?????????? ?????????? ????
        {
            int a = Cal.CalSkillpow(skillData, skillLv);

            return (T)Convert.ChangeType(a, typeof(T));
        }

    }

    public class EquipmentManager : Managers<EquipmentManager> 
    {
        protected CharacterManager characterManager;
        [SerializeField]
        protected List<EquipmentData> equipmentDatas = new List<EquipmentData>();
        public List<Equipment> equipments = new List<Equipment>();
        public List<Equipment> gainedEquipment = new List<Equipment>();

        

        protected virtual void Init()
        {
            CreateEquipment();
        }

        //?????????? ???????? ???????? ????.
        //???????? equipment???????? ??????.
        protected virtual void CreateEquipment()
        {
            //?????? ???????? ???????? ???? equipmentCount?? ????????.
        }

        public virtual void LossEquipment(Equipment equipment)
        {
            equipments.Remove(equipment);
        }

        // ?????????? ?????? ???????? ????, ???????? ?????? ???? ?????? ?? ????
        // ???? ???? ?????? ???????? ???????? ???? ?????? ???? equipments???? equipment?? ???????? ?????????? ????
        // ?????? ?????????? ?????? ???? ???? ?????? ????.
        public virtual void GetEquipment(Equipment equipment)
        {
            if (gainedEquipment.Contains(equipment))
            {
                gainedEquipment.Find(e => e == equipment).equipmentCount++;
            }
            else
            {
                gainedEquipment.Add(equipment);
            }
            
        }

        //?????????? ???????????? ?????? ?????? ?? ???? ????. ?????????????? ?????? ???? ???????? ???? ??????.
        public virtual void RemoveEquipment(Equipment equipment)
        {
            if (gainedEquipment.Contains(equipment))
            {
                gainedEquipment.Find(e => e == equipment).equipmentCount--;
            }
            else
            {
                return;
            }
            if(equipment.equipmentCount <= 0)
            {
                characterManager.LossEquipment(equipment);
            }

        }

       
    }

    public class Equipment
    {
        EquipmentData equipmentdata;
        public Enums.EquipmentType equipmentType;
        public enum EquipmentStat { Gain, Equip}
        public EquipmentStat equipmentStat;
        protected int itemLv;
        protected int maxLv;
        
        public int ItemLv
        {
            get
            {
                return itemLv;
            }
            set
            {
                if (value > maxLv)
                {
                    itemLv = maxLv;
                }
                else
                {
                    itemLv = value;
                }
            }
        }

        public int[] statUp = new int[Nums.StatCount];
        public int equipmentCount;

        public Equipment (EquipmentData data)
        {
            equipmentdata = data;
            Init();
        }

        protected void Init()
        {
            //equipmentdata?? equmentType?? ?????? this.equipmentType?? ??????????.
        }


        public virtual int[] Cal_Pow() // ???????? ???????? ?? ???? ?????? ?????? ????
        {
            int[] a = Cal.CalEquipments(this) ;
            return a;
        }

        

    }

    public class Cal
    {
        //???????? ?????? ???????? Int????
        public static int CalStatUP(int origin, int amount)
        {
            int a = 0;
            a = amount;
            return a;
        }

        //???????? ?????? ???????? Float????
        public static int CalStatUP(int origin, float amount)
        {
            int a = 0;
            a = Mathf.RoundToInt(origin*amount);
            return a;
        }


        //???????????? ???????? ???? ????
        public static int CalStat(int stat, int Lv, Equipment[] equipments, List<int>[] tempstats)
        {
            int e = 0;
            int t = 0;
            for(int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
            for(int i=0; i< tempstats[stat].Count; i++)
            {
                t += tempstats[stat][i];
            }

            int a = (stat*Lv) + e+t;// ????????+ ???? ???? + ????????, ???? ???? 
            return a;
        }

        // ???????? ???? ???? ???? ???? ??????
        public static int CalStat(int stat, int Lv, Equipment[] equipments)
        {
            int e = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
          
            int a = (stat * Lv) + e ;// ???????? + ???? ???? + ????????
            return a;
        }

        // ???? ?????? ????????
        public static int[] CalEquipments(Equipment equipment)
        {
            int[] a = new int[Nums.StatCount];
           for(int i=0; i < equipment.statUp.Length; i++)
            {
                a[i] = equipment.statUp[i]* equipment.ItemLv;
            }
            return a;
        }

        // ???? ???? ???? ????
        public static int CalSkillpow(SkillData skillData, int Lv)
        {
            int a = Lv;

            return a;
        }
    }

    public interface IAtionSkill
    {
        [SerializeField]
        GameObject effectPrefab { get; set; }

        public void OnAtionSkill();
        public void OnSkillAttack();

    }

    public interface IPassiveSkill
    {
        public void OnPassiveSkill();
    }

    public interface IConditionSkill
    {
        public GameObject effectPrefab { get; set; }
        public Enums.SkillConditionType skillConditionType { get; set; }
        public void OnConditionSkill();
        public void CheckCondition();
        public void CompleteCondition();

    }

    public interface IBuffDebuffSkill
    {
        public Enums.BuffDebuffType buffDebuffType { get; set; }

        public float duration { get; set; }
        public void OnBuffDebuffSkill();
    }


    public class Buff
    {
        public Enums.BuffDebuffType buffDebuffType;
        public float duration;  // ????????
        public float checkduration; // ?????? ???? ????
        public int buffPow; //????????
       
        public Buff(float time, float checktime, int pow)
        {
            duration = time;
            checkduration = checktime;
            buffPow = pow;
        }

        public virtual void OnBuff()
        {
           
        }

        public virtual void CheckBuff()
        {

        }

        public virtual void OffBuff()
        {
           
        }
        
    }


    public class Nums
    {
        public const int StatCount = 0;
       
        public const int SkillSetCount = 0;
       
        public const int EquipmentCount = 0;
       
        public const int CharacterEquipCount = 0;
       
        public const int CharacterMaxLv = 0;
       
        public const int SkillMaxLv = 0;
       
        public const int CharacterHP = 0;
    }

    public class Enums
    {
    
        public enum EquipmentType { Weapon, Amor }
        public enum SkillType { Ation,Passive,Condtion, BuffDebuff}

        public enum SkillConditionType { Attack, Dffence}
       
        public enum BuffDebuffType { buff, debuff}

        public enum BuffDebuffStat { Ready, Active }

        public enum UIType { Image, Text, Slot, Info, GameObject, Slider, DragNDrop}

        public enum Grade { Normal, Rere, Unipe, Legend }
    }


    public abstract class CharacterData { }
    

    

    public abstract class SkillData { }
    

    

    public abstract class EquipmentData { }
    
    public abstract class BuffDebuffData { }

    

    public class DataManager : Managers<DataManager>
    {
        public List<CharacterData> characterDatas = new List<CharacterData>();
        public List<SkillData> skilldatas = new List<SkillData>();
        public List<EquipmentData> equipmentDatas = new List<EquipmentData>();

        public virtual void InIt()
        {
            
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

        public static List<T> FindAllComponentsInChildren<T>(Transform parent) where T : Component
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

}



