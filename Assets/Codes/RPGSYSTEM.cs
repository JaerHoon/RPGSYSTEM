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
        U characterData; // ĳ���� ������
        protected int[] stats = new int[Nums.statCount]; // ���ȹ迭�� ����
        protected int[] stats_Lv = new int[Nums.statCount];//���� ���� �迭 Ȥ�� ���� ����Ʈ
        protected List<int>[] temp_stat = new List<int>[Nums.statCount]; // �ӽ÷� ���ϴ� ������ ����Ʈ�� ���� �迭�ȿ� ����Ʈ�� ����
        protected int lv; //�÷��̾� ����
        protected int maxLv; // �÷��̾� �ִ� ���� ����
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

        protected List<Skill<S>> skills = new List<Skill<S>>(); // ĳ���� ��� ��ų
        protected Skill<S>[] equipSkills = new Skill<S>[Nums.skillSetCount]; // �ɸ��� ��ų���Կ� ������ ��ų �迭
        protected Equipment<E>[] equipments = new Equipment<E>[Nums.equipmentCount]; // ĳ���� ���Կ� ������ ��� �迭
        

        public Character(U data)
        {
            characterData = data;
            Init();
        }

        protected virtual void Init()
        {

        }

        public virtual T GetStat<T>(int stat, bool Istemp) // stat ����ؼ� �� T�� ��ȯ�ϴ� �Լ�
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

        public virtual void OnEquip(Equipment<E> equipment,int equipType)//��� ���Կ� �����Լ�
        {
            equipments[equipType] = equipment;
        }

        public virtual void StatUP(int stat, int amount)// �� ���� �� �Լ�
        {
            stats[stat] += amount;
        }

        public virtual int Temp_statUp(int stat, int amount)// �ӽ÷� �ö󰡴� ������ ������ִ� �Լ� -> �ε����� ����
        {
            temp_stat[stat].Add(amount);
            int index = temp_stat[stat].Count-1;
            return index; 
        }

        public virtual void Temp_statRestore(int stat, int index)//�ӽ� ���� ���� ȸ���ϴ� �Լ�.
        {
            temp_stat[stat].RemoveAt(index);
        }

        public virtual void EquipSkill(int slotnum,Skill<S> skill)// ��ų ���� �Լ�
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


        public virtual int[] Cal_Pow() // �������� �ö󰡴� �� ���� ���
        {
            int[] a = Cal.CalEquipments<E>(this) ;
            return a;
        }

        

    }

    public class Cal
    {
        public static int CalStat<T>(int stat, int Lv, Equipment<T>[] equipments, List<int>[] tempstats)// ���Ȱ�� �Լ�
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

            int a = (stat*Lv) + e+t;// �⺻����+ ���� ���� + �����, �ӽ� ���� 
            return a;
        }

        public static int CalStat<T>(int stat, int Lv, Equipment<T>[] equipments)// ���Ȱ�� �Լ�
        {
            int e = 0;
            int t = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
          
            int a = (stat * Lv) + e ;// �⺻����+ ���� ���� + �����
            return a;
        }

        public static int[] CalEquipments<T>(Equipment<T> equipment)// ��� ���Ⱦ� ����Լ�
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



