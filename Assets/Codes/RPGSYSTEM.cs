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

   
    public class CharacterManager<C,E,S> : Managers<CharacterManager<C,E,S>> 
    {
        protected SkillManager<C, E, S> skillManager;

        protected List<Data> chardata = new List<Data>();
       
        public List<C> characterDatas = new List<C>();
        protected List<Character<C, E, S>> characters = new List<Character<C, E, S>>();
        protected List<Character<C, E, S>> ativeCharacters = new List<Character<C, E, S>>();
        protected Character<C, E, S>[] inBattleCharacters = new Character<C, E, S>[Nums.CharacterEquipCount];

        protected virtual void Init()
        {
            for (int i = 0; i < characterDatas.Count; i++)
            {
                Character<C, E, S> chars = new Character<C, E, S>(characterDatas[i]);
                chars.InitSkill(skillManager.charaterskills[i]);
                characters.Add(chars);
            }
        }

        public virtual void GetCharacter(int characterNum)
        {
            ativeCharacters.Add(characters[characterNum]);
        }

        
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

        
        public void EquipEquipment(int characterNum , Equipment<C, E, S> equipment) 
        {
            characters[characterNum].OnEquip(equipment);
            equipment.equipmentStat = Equipment<C, E, S>.EquipmentStat.Equip;
        }

      
        public void RemoveEquipment(int characterNum, Equipment<C, E, S> equipment)
        {
            characters[characterNum].RemoveEquipment((int)equipment.equipmentType);
            equipment.equipmentStat = Equipment<C, E, S>.EquipmentStat.Gain;
        }

        
        public void LossEquipment(Equipment<C, E, S> equipment)
        {
            foreach(Character< C,E,S > car in characters)
            {
                car.LossEquipment(equipment);
            }
        }

        public void EquipSkill(int characterNum,int slotnum ,Skill<C,E,S> skill)
        {
            characters[characterNum].EquipSkill(slotnum, skill);
        }
    }

    public class Character<C,E,S>
    {
        public C characterData;
        protected int[] stats = new int[Nums.StatCount]; 
        protected int[] stats_Lv = new int[Nums.StatCount];
        protected List<int>[] temp_stat = new List<int>[Nums.StatCount]; //임시스탯 변화를 리스트로 관리
        protected int lv=1; 
        protected int maxLv = Nums.CharacterMaxLv; 
        protected List<Skill<C, E, S>> skills = new List<Skill<C, E, S>>(); //캐릭터가 사용할 수 있는 모든 스킬
        protected Skill<C, E, S>[] equipSkills = new Skill<C, E, S>[Nums.SkillSetCount]; // 장비하고 있는 스킬
        protected Equipment<C, E, S>[] equipments = new Equipment<C, E, S>[Nums.EquipmentCount]; //장비하고 있는 아이템, 인벤토리는 따로 있음

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

        public Character(C data)
        {
            characterData = data;
            Init();
        }

        protected virtual void Init()
        {

        }

        public void InitSkill(List<Skill<C, E, S>> Characterskills)
        {
            skills.AddRange(Characterskills);
        }

        public virtual T GetStat<T>(int stat, bool Istemp) // 스탯계산해서 반환해주는 함수
        {
            int st = 0;
            if(Istemp == true)
            {
               st = Cal<C, E, S>.CalStat(stat, stats_Lv[stat], equipments, temp_stat);
            }
            else
            {
                st = Cal<C, E, S>.CalStat(stat, stats_Lv[stat], equipments);
            }
           
            return (T)Convert.ChangeType(st, typeof(T));
        }

        public virtual void OnEquip(Equipment<C, E, S> equipment) // 장비장착 함수.
        {
            equipments[(int)equipment.equipmentType] = equipment;
        }

        public virtual void RemoveEquipment(int slotnum) // 장비제거 함수.
        {
            equipments[slotnum] = null;
        }

      
        public virtual void LossEquipment(Equipment<C, E, S> equipment) //장비 완전 제거 함수.
        {
            for(int i = 0; i < equipments.Length; i++)
            {
                if(equipments[i] == equipment)
                {
                    equipments[i] = null;
                }
            }
        }

        public virtual void StatUP(int stat, int amount)//영구적인 스탯 증가 int버전
        {
            stats[stat] += Cal<C, E, S>.CalStatUP(stats[stat], amount);
        }

        public virtual void StatUP(int stat, float amount)//영구적인 스탯 증가 float버전 아마 %계산 때문일듯?
        {
            stats[stat] += Cal<C, E, S>.CalStatUP(stats[stat], amount); 
        }

        public virtual void Temp_statUp(int stat, int amount)// 인트버전 임시 스탯 변화
        {
            temp_stat[stat].Add(amount);
           
        }

        public virtual void Temp_statRestore(int stat, int amount)// 인트버전 임시 스탯 회복..
        {
            temp_stat[stat].Remove(amount);
        }

        public void Temp_Reset()// 임시 스탯 변화 리셋.
        {
            for(int i = 0; i < temp_stat.Length; i++)
            {
                temp_stat[i].Clear();
            }
        }

        public virtual void EquipSkill(int slotnum,Skill<C, E, S> skill)//스킬 장착 함수.
        {
            for(int i = 0; i < skills.Count; i++) // 다른 칸에 
            {
                if(skills[i] = skill)
                {
                    equipSkills[i] = null;
                }
            }

            equipSkills[slotnum] = skill;
        }

        public virtual void RemoveSkill(int slotnum)
        {
            equipSkills[slotnum] = null;
        }

    }

    public class SkillManager<C, E, S> : Managers<SkillManager<C, E, S>>
    {
        [SerializeField]
        protected List<Skill<C, E, S>> allSkills = new List<Skill<C, E, S>>(); 
        public List<List<Skill<C, E, S>>> charaterskills = new List<List<Skill<C, E, S>>>();

        public virtual void ListingSkill()
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

    public class Skill<C, E, S> : MonoBehaviour 
    {
        public S skillData;
        public Enums.SkillType skillType;
        public int characterNum; 
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
           
        }

        public virtual T GetSkillPow<T>()
        {
            int a = Cal<C, E, S>.CalSkillpow(skillData, skillLv);

            return (T)Convert.ChangeType(a, typeof(T));
        }

    }

    public class EquipmentManage<C,E,S>: Managers<EquipmentManage<C, E, S>> 
    {
        protected CharacterManager<C, E, S> characterManager;
        [SerializeField]
        protected List<E> equipmentDatas = new List<E>();
        public List<Equipment<C, E, S>> equipments = new List<Equipment<C, E, S>>();
        public List<Equipment<C, E, S>> gainedEquipment = new List<Equipment<C,E,S>>();

        

        protected virtual void Init()
        {
            CreateEquipment();
        }

        protected virtual void CreateEquipment()
        {
            
        }

        public virtual void LossEquipment(Equipment<C, E, S> equipment)
        {
            equipments.Remove(equipment);
        }

        
        public virtual void GetEquipment(Equipment<C, E, S> equipment)
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

      
        public virtual void RemoveEquipment(Equipment<C, E, S> equipment)
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

    public class Equipment<C, E, S>
    {
        E equipmentdata;
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

        public Equipment (E data)
        {
            equipmentdata = data;
            Init();
        }

        protected void Init()
        {
           
        }


        public virtual int[] Cal_Pow() 
        {
            int[] a = Cal<C, E, S>.CalEquipments(this) ;
            return a;
        }

        

    }

    public class Cal<C,E,S>
    {
       //캐릭터 스탯 업 int버전
        public static int CalStatUP(int origin, int amount)
        {
            int a = 0;
            a = amount;
            return a;
        }

       //캐릭터 스탯 업 float버전 아마 %계산 때문일듯?
        public static int CalStatUP(int origin, float amount)
        {
            int a = 0;
            a = Mathf.RoundToInt(origin*amount);
            return a;
        }


       //최종적으로 스탯을 계산해서 전투에 반영해줄 수 있게 반환해주는 함수.
        public static int CalStat(int stat, int Lv, Equipment<C, E, S>[] equipments, List<int>[] tempstats)
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

            int a = (stat*Lv) + e+t;//(기본스탯*레벨)+장비스탯+ 임시스탯
            return a;
        }

        //UI에서 장비를 착용한 스탯을 확인할때 호출하는 함수
        public static int CalStat(int stat, int Lv, Equipment<C, E, S>[] equipments)
        {
            int e = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
          
            int a = (stat * Lv) + e ;
            return a;
        }

        // 장비가 각 스탯에 어떤 영향을 미치는지 계산해서 int[]로 반환해주는 함수.
        public static int[] CalEquipments(Equipment<C, E, S> equipment)
        {
            int[] a = new int[Nums.StatCount];
           for(int i=0; i < equipment.statUp.Length; i++)
            {
                a[i] = equipment.statUp[i]* equipment.ItemLv;
            }
            return a;
        }

        //스킬 파워 계산해주는 함수
        public static int CalSkillpow(S skillData, int Lv)
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
        public float duration;  // 버프 총 시간
        public float checkduration; // 버프 중에 도트데미지를 위한 시간
        public int buffPow;
       
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
    
        public enum SlotType { InventroySlot, EquipSlot}
        public enum EquipmentType { Weapon, Amor }
        public enum SkillType { Ation,Passive,Condtion, BuffDebuff}

        public enum SkillConditionType { Attack, Dffence}
       
        public enum BuffDebuffType { buff, debuff}

        public enum BuffDebuffStat { Ready, Active }

        public enum UIType { Image, Text, Slot, Info, GameObject, Slider, DragNDrop}

        public enum Grade { Normal, Rere, Unipe, Legend }
    }

    public class Data
    {
        
    }

    public abstract class CharacterData : Data { }
    

    

    public abstract class SkillData : Data { }




    public abstract class EquipmentData : Data { }
    

    public abstract class BuffDebuffData : Data { }

    

    public class DataManager<C,E,S> : Managers<DataManager<C, E, S>>
    {
        public List<C> characterDatas = new List<C>();
        public List<E> skilldatas = new List<E>();
        public List<S> equipmentDatas = new List<S>();

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

}



