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

    public class CharacterManager<T,S,E> : Managers<CharacterManager<T,S,E>> where T : class where S : class where E : class
    {
        [SerializeField]
        List<T> characterDatas = new List<T>();
        public List<Character<T,S,E>> characters = new List<Character<T,S,E>>();
        public List<Character<T,S,E>> ativeCharacters = new List<Character<T,S,E>>();
        public List<Character<T,S,E>> inBattleCharacters = new List<Character<T,S,E>>();

        protected virtual void Init()
        {
            for (int i = 0; i < characterDatas.Count; i++)
            {
                Character<T,S,E> chars = new Character<T,S,E>(characterDatas[i]);
                characters.Add(chars);
            }
        }
    }

    public class Character<U,S,E> where U : class where S : class where E : class
    {
        U characterData; // 캐릭터 데이터
        protected int[] stats = new int[Nums.statCount]; // 스탯배열로 관리
        protected int[] stats_Lv = new int[Nums.statCount];//스탯 레벨 배열 혹은 스탯 포인트
        protected List<int>[] temp_stat = new List<int>[Nums.statCount]; // 임시로 변하는 스텟을 리스트로 관리 배열안에 리스트가 있음
        protected int lv; //플레이어 레벨
        protected int maxLv; // 플레이어 최대 레벨 설정
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

        protected List<Skill<S>> skills = new List<Skill<S>>(); // 캐릭터 모든 스킬
        protected Skill<S>[] equipSkills = new Skill<S>[Nums.skillSetCount]; // 케릭터 스킬슬롯에 장착한 스킬 배열
        protected Equipment<E>[] equipments = new Equipment<E>[Nums.equipmentCount]; // 캐릭터 슬롯에 장착한 장비 배열
        

        public Character(U data)
        {
            characterData = data;
            Init();
        }

        protected virtual void Init()
        {

        }

        public virtual T GetStat<T>(int stat, bool Istemp) // stat 계산해서 을 T로 반환하는 함수
        {
            int st = 0;
            if(Istemp == true)
            {
               st = Cal.CalStat(stat, stats_Lv[stat], equipments, temp_stat);
            }
            else
            {
                st = Cal.CalStat(stat, stats_Lv[stat], equipments);
            }
           
            return (T)Convert.ChangeType(st, typeof(T));
        }

        public virtual void OnEquip(Equipment<E> equipment,int equipType)//장비 슬롯에 착용함수
        {
            equipments[equipType] = equipment;
        }

        public virtual void StatUP(int stat, int amount)// 각 스탯 업 함수
        {
            stats[stat] += amount;
        }

        public virtual int Temp_statUp(int stat, int amount)// 임시로 올라가는 스탯을 계산해주는 함수 -> 인덱스로 리턴
        {
            temp_stat[stat].Add(amount);
            int index = temp_stat[stat].Count-1;
            return index; 
        }

        public virtual void Temp_statRestore(int stat, int index)//임시 버프 스탯 회복하는 함수.
        {
            temp_stat[stat].RemoveAt(index);
        }

        public virtual void EquipSkill(int slotnum,Skill<S> skill)// 스킬 장착 함수
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

    public class Skill<S> : MonoBehaviour 
    {
        S skillData;
        protected int skillLv;
        protected int maxSkillLv;
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

        public Skill(S data)
        {
            skillData = data;
            Init();
        }

        protected void Init()
        {

        }


       
    }

    public class Equipment<E>
    {
        E equipmentdata;
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

        public int[] statUp = new int[Nums.statCount];

        public Equipment (E data)
        {
            equipmentdata = data;
            Init();
        }

        protected void Init()
        {

        }


        public virtual int[] Cal_Pow() // 레벨업당 올라가는 각 스탯 계산
        {
            int[] a = Cal.CalEquipments<E>(this) ;
            return a;
        }

        

    }

    public class Cal
    {
        public static int CalStat<T>(int stat, int Lv, Equipment<T>[] equipments, List<int>[] tempstats)// 스탯계산 함수
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

        public static int CalStat<T>(int stat, int Lv, Equipment<T>[] equipments)// 스탯계산 함수
        {
            int e = 0;
            int t = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
          
            int a = (stat * Lv) + e ;// 기본스탯+ 스탯 레벨 + 장비스탯
            return a;
        }

        public static int[] CalEquipments<T>(Equipment<T> equipment)// 장비 스탯업 계산함수
        {
            int[] a = new int[Nums.statCount];
           for(int i=0; i < equipment.statUp.Length; i++)
            {
                a[i] = equipment.statUp[i]* equipment.ItemLv;
            }
            return a;
        }
    }

    public class Nums
    {
        public const int statCount = 0;
        public const int skillSetCount = 0;
        public const int equipmentCount = 0;
    }

}



