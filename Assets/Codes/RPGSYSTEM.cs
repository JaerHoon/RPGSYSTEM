using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        public virtual void GetCharacter(int characterNum)// 캐릭터를 얻었을때 실행함수
        {
            ativeCharacters.Add(characters[characterNum]);
        }

        //캐릭터를 전투 참전 슬롯에 장착시킬때 쓰는 함수
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

        //검사는 UI에서 한다. 캐릭터에게 장비를 장착시킬때 쓰는 함수
        public void EquipEquipment(int characterNum , Equipment equipment) 
        {
            characters[characterNum].OnEquip(equipment);
            equipment.equipmentStat = Equipment.EquipmentStat.Equip;
        }

        //캐릭터에게 장비를 제거할때 쓰는 함수
        public void RemoveEquipment(int characterNum, Equipment equipment)
        {
            characters[characterNum].RemoveEquipment((int)equipment.equipmentType);
            equipment.equipmentStat = Equipment.EquipmentStat.Gain;
        }

        // 장비를 인벤토리에서 제거했을때 혹시 캐릭터가 장비하고 있다면 해제시키는 함수
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
        protected CharacterData characterData; // 캐릭터 데이터
        protected int[] stats = new int[Nums.StatCount]; // 스탯배열로 관리
        protected int[] stats_Lv = new int[Nums.StatCount];//스탯 레벨 배열 혹은 스탯 포인트
        protected List<int>[] temp_stat = new List<int>[Nums.StatCount]; // 임시로 변하는 스텟을 리스트로 관리 배열안에 리스트가 있음
        protected int lv=1; //플레이어 레벨
        protected int maxLv = Nums.CharacterMaxLv; // 플레이어 최대 레벨 설정
        protected List<Skill> skills = new List<Skill>(); // 캐릭터 모든 스킬
        protected Skill[] equipSkills = new Skill[Nums.SkillSetCount]; // 케릭터 스킬슬롯에 장착한 스킬 배열
        protected Equipment[] equipments = new Equipment[Nums.EquipmentCount]; // 캐릭터 슬롯에 장착한 장비 배열

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

        public void InitSkill(List<Skill> Characterskills)// 스킬만 따로 다시 받아서 리스트에 넣음
        {
            skills.AddRange(Characterskills);
        }

        public virtual T GetStat<T>(int stat, bool Istemp) // stat 계산해서 을 T로 반환하는 함수
        {
            int st = 0;
            if(Istemp == true)// 전투중일때
            {
               st = Cal.CalStat(stat, stats_Lv[stat], equipments, temp_stat);
            }
            else//전투중이 아닐때
            {
                st = Cal.CalStat(stat, stats_Lv[stat], equipments);
            }
           
            return (T)Convert.ChangeType(st, typeof(T));
        }

        public virtual void OnEquip(Equipment equipment)//장비 슬롯에 착용함수
        {
            equipments[(int)equipment.equipmentType] = equipment;
        }

        public virtual void RemoveEquipment(int slotnum)//장비슬롯에서 해제 함수.
        {
            equipments[slotnum] = null;
        }

        //아이템을 인벤토리에서 제거했을때 캐릭터에게서도 장착 해제되는 함수
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

        public virtual void StatUP(int stat, int amount)// 각 스탯 영구적인 변화 함수
        {
            stats[stat] += Cal.CalStatUP(stats[stat], amount);
        }

        public virtual void StatUP(int stat, float amount)// 각 스탯 영구적인 변화 함수 플롯버전
        {
            stats[stat] += Cal.CalStatUP(stats[stat], amount); 
        }

        public virtual void Temp_statUp(int stat, int amount)// 임시로 올라가는 스탯을 계산해주는 함수
        {
            temp_stat[stat].Add(amount);
           
        }

        public virtual void Temp_statRestore(int stat, int amount)//임시 버프 스탯 회복하는 함수.
        {
            temp_stat[stat].Remove(amount);
        }

        public void Temp_Reset()//전투 종료시에 임시로 변한 스탯을 완전히 리셋하는 함수
        {
            for(int i = 0; i < temp_stat.Length; i++)
            {
                temp_stat[i].Clear();
            }
        }

        public virtual void EquipSkill(int slotnum,Skill skill)// 스킬 장착 함수
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
        protected List<Skill> allSkills = new List<Skill>(); // 모든 스킬 프리팹 인스펙터 창에서 받음
        public List<List<Skill>> charaterskills = new List<List<Skill>>(); // 각 캐릭터마다 스킬을 리스트에 넣음

        public virtual void ListingSkill()// 각 캐릭터리스트 인덱스에 다시 리스트로 스킬 저장
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
        public int characterNum; // 이 스킬이 어떤 캐릭터 스킬인지
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
            //스킬타입 인터페이스에 따라서 함수를 호출해준다.
        }

        public virtual T GetSkillPow<T>()// 레벨에 따라서 스킬파워를 반환해주는 함수
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

        //순간적으로 아이템을 생성하는 함수.
        //생성해서 equipment리스트에 넣는다.
        protected virtual void CreateEquipment()
        {
            //아이템 생성할때 중복되는 것은 equipmentCount를 사용한다.
        }

        public virtual void LossEquipment(Equipment equipment)
        {
            equipments.Remove(equipment);
        }

        // 플레이어가 장비를 얻었을때 함수, 장비생성 방식에 따라 달라질 수 있음
        // 이건 미리 만들어 놓는다는 가정하에 미리 만들어 놓은 equipments에서 equipment를 추출하여 매개변수로 사용
        // 아니면 순간적으로 만들고 그냥 넣는 방법도 있음.
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

        //플레이어가 인벤토리에서 장비를 제거할 때 쓰는 함수. 중복소유가능한 장비면 장비 카운트를 하나 올려줌.
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
            //equipmentdata에 equmentType를 반드시 this.equipmentType에 할당해야함.
        }


        public virtual int[] Cal_Pow() // 레벨업당 올라가는 각 스탯 계산을 배열로 반환
        {
            int[] a = Cal.CalEquipments(this) ;
            return a;
        }

        

    }

    public class Cal
    {
        //영구적인 스탯업 계산함수 Int버전
        public static int CalStatUP(int origin, int amount)
        {
            int a = 0;
            a = amount;
            return a;
        }

        //영구적인 스탯업 계산함수 Float버전
        public static int CalStatUP(int origin, float amount)
        {
            int a = 0;
            a = Mathf.RoundToInt(origin*amount);
            return a;
        }


        //스탯계산함수 임시변화 스탯 포함
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

            int a = (stat*Lv) + e+t;// 기본스탯+ 스탯 레벨 + 장비스탯, 임시 스탯 
            return a;
        }

        // 스탯계산 함수 임시 변화 스탯 미포함
        public static int CalStat(int stat, int Lv, Equipment[] equipments)
        {
            int e = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
          
            int a = (stat * Lv) + e ;// 기본스탯 + 스탯 레벨 + 장비스탯
            return a;
        }

        // 장비 스탯업 계산함수
        public static int[] CalEquipments(Equipment equipment)
        {
            int[] a = new int[Nums.StatCount];
           for(int i=0; i < equipment.statUp.Length; i++)
            {
                a[i] = equipment.statUp[i]* equipment.ItemLv;
            }
            return a;
        }

        // 스킬 파워 계산 함수
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


    public class BuffDebuff
    {
        public Enums.BuffDebuffType buffDebuffType;
        public float duration;  // 버프시간
        public float checkduration; // 버프중 도트 시간
        public int buffPow; //버프파워
       
        public BuffDebuff(float time, float checktime, int pow)
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
        //캐릭터 스탯의 종류 개수
        public const int StatCount = 0;
        //스킬 착용 슬롯의 개수
        public const int SkillSetCount = 0;
        //캐릭터 장비착용 슬롯 개수
        public const int EquipmentCount = 0;
        //캐릭터를 배틀할때 몇명 장착할 수 있는지 개수
        public const int CharacterEquipCount = 0;
        //캐릭터의 최고 레벨
        public const int CharacterMaxLv = 0;
        //각 스킬의 최고 레벨
        public const int SkillMaxLv = 0;
        //캐릭터 스탯 HP의 번호
        public const int CharacterHP = 0;
    }

    public class Enums
    {
    
        public enum EquipmentType { Weapon, Amor }
        public enum SkillType { Ation,Passive,Condtion, BuffDebuff}

        public enum SkillConditionType { Attack, Dffence}
       
        public enum BuffDebuffType { buff, debuff}

        public enum BuffDebuffStat { Ready, Active }
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
}



