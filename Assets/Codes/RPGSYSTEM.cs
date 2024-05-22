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

        public virtual void GetCharacter(int characterNum)// ĳ���͸� ������� �����Լ�
        {
            ativeCharacters.Add(characters[characterNum]);
        }

        //ĳ���͸� ���� ���� ���Կ� ������ų�� ���� �Լ�
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

        //�˻�� UI���� �Ѵ�. ĳ���Ϳ��� ��� ������ų�� ���� �Լ�
        public void EquipEquipment(int characterNum , Equipment equipment) 
        {
            characters[characterNum].OnEquip(equipment);
            equipment.equipmentStat = Equipment.EquipmentStat.Equip;
        }

        //ĳ���Ϳ��� ��� �����Ҷ� ���� �Լ�
        public void RemoveEquipment(int characterNum, Equipment equipment)
        {
            characters[characterNum].RemoveEquipment((int)equipment.equipmentType);
            equipment.equipmentStat = Equipment.EquipmentStat.Gain;
        }

        // ��� �κ��丮���� ���������� Ȥ�� ĳ���Ͱ� ����ϰ� �ִٸ� ������Ű�� �Լ�
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
        protected CharacterData characterData; // ĳ���� ������
        protected int[] stats = new int[Nums.StatCount]; // ���ȹ迭�� ����
        protected int[] stats_Lv = new int[Nums.StatCount];//���� ���� �迭 Ȥ�� ���� ����Ʈ
        protected List<int>[] temp_stat = new List<int>[Nums.StatCount]; // �ӽ÷� ���ϴ� ������ ����Ʈ�� ���� �迭�ȿ� ����Ʈ�� ����
        protected int lv=1; //�÷��̾� ����
        protected int maxLv = Nums.CharacterMaxLv; // �÷��̾� �ִ� ���� ����
        protected List<Skill> skills = new List<Skill>(); // ĳ���� ��� ��ų
        protected Skill[] equipSkills = new Skill[Nums.SkillSetCount]; // �ɸ��� ��ų���Կ� ������ ��ų �迭
        protected Equipment[] equipments = new Equipment[Nums.EquipmentCount]; // ĳ���� ���Կ� ������ ��� �迭

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

        public void InitSkill(List<Skill> Characterskills)// ��ų�� ���� �ٽ� �޾Ƽ� ����Ʈ�� ����
        {
            skills.AddRange(Characterskills);
        }

        public virtual T GetStat<T>(int stat, bool Istemp) // stat ����ؼ� �� T�� ��ȯ�ϴ� �Լ�
        {
            int st = 0;
            if(Istemp == true)// �������϶�
            {
               st = Cal.CalStat(stat, stats_Lv[stat], equipments, temp_stat);
            }
            else//�������� �ƴҶ�
            {
                st = Cal.CalStat(stat, stats_Lv[stat], equipments);
            }
           
            return (T)Convert.ChangeType(st, typeof(T));
        }

        public virtual void OnEquip(Equipment equipment)//��� ���Կ� �����Լ�
        {
            equipments[(int)equipment.equipmentType] = equipment;
        }

        public virtual void RemoveEquipment(int slotnum)//��񽽷Կ��� ���� �Լ�.
        {
            equipments[slotnum] = null;
        }

        //�������� �κ��丮���� ���������� ĳ���Ϳ��Լ��� ���� �����Ǵ� �Լ�
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

        public virtual void StatUP(int stat, int amount)// �� ���� �������� ��ȭ �Լ�
        {
            stats[stat] += Cal.CalStatUP(stats[stat], amount);
        }

        public virtual void StatUP(int stat, float amount)// �� ���� �������� ��ȭ �Լ� �÷Թ���
        {
            stats[stat] += Cal.CalStatUP(stats[stat], amount); 
        }

        public virtual void Temp_statUp(int stat, int amount)// �ӽ÷� �ö󰡴� ������ ������ִ� �Լ�
        {
            temp_stat[stat].Add(amount);
           
        }

        public virtual void Temp_statRestore(int stat, int amount)//�ӽ� ���� ���� ȸ���ϴ� �Լ�.
        {
            temp_stat[stat].Remove(amount);
        }

        public void Temp_Reset()//���� ����ÿ� �ӽ÷� ���� ������ ������ �����ϴ� �Լ�
        {
            for(int i = 0; i < temp_stat.Length; i++)
            {
                temp_stat[i].Clear();
            }
        }

        public virtual void EquipSkill(int slotnum,Skill skill)// ��ų ���� �Լ�
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
        protected List<Skill> allSkills = new List<Skill>(); // ��� ��ų ������ �ν����� â���� ����
        public List<List<Skill>> charaterskills = new List<List<Skill>>(); // �� ĳ���͸��� ��ų�� ����Ʈ�� ����

        public virtual void ListingSkill()// �� ĳ���͸���Ʈ �ε����� �ٽ� ����Ʈ�� ��ų ����
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
        public int characterNum; // �� ��ų�� � ĳ���� ��ų����
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
            //��ųŸ�� �������̽��� ���� �Լ��� ȣ�����ش�.
        }

        public virtual T GetSkillPow<T>()// ������ ���� ��ų�Ŀ��� ��ȯ���ִ� �Լ�
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

        //���������� �������� �����ϴ� �Լ�.
        //�����ؼ� equipment����Ʈ�� �ִ´�.
        protected virtual void CreateEquipment()
        {
            //������ �����Ҷ� �ߺ��Ǵ� ���� equipmentCount�� ����Ѵ�.
        }

        public virtual void LossEquipment(Equipment equipment)
        {
            equipments.Remove(equipment);
        }

        // �÷��̾ ��� ������� �Լ�, ������ ��Ŀ� ���� �޶��� �� ����
        // �̰� �̸� ����� ���´ٴ� �����Ͽ� �̸� ����� ���� equipments���� equipment�� �����Ͽ� �Ű������� ���
        // �ƴϸ� ���������� ����� �׳� �ִ� ����� ����.
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

        //�÷��̾ �κ��丮���� ��� ������ �� ���� �Լ�. �ߺ����������� ���� ��� ī��Ʈ�� �ϳ� �÷���.
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
            //equipmentdata�� equmentType�� �ݵ�� this.equipmentType�� �Ҵ��ؾ���.
        }


        public virtual int[] Cal_Pow() // �������� �ö󰡴� �� ���� ����� �迭�� ��ȯ
        {
            int[] a = Cal.CalEquipments(this) ;
            return a;
        }

        

    }

    public class Cal
    {
        //�������� ���Ⱦ� ����Լ� Int����
        public static int CalStatUP(int origin, int amount)
        {
            int a = 0;
            a = amount;
            return a;
        }

        //�������� ���Ⱦ� ����Լ� Float����
        public static int CalStatUP(int origin, float amount)
        {
            int a = 0;
            a = Mathf.RoundToInt(origin*amount);
            return a;
        }


        //���Ȱ���Լ� �ӽú�ȭ ���� ����
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

            int a = (stat*Lv) + e+t;// �⺻����+ ���� ���� + �����, �ӽ� ���� 
            return a;
        }

        // ���Ȱ�� �Լ� �ӽ� ��ȭ ���� ������
        public static int CalStat(int stat, int Lv, Equipment[] equipments)
        {
            int e = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                e += equipments[i].Cal_Pow()[stat];
            }
          
            int a = (stat * Lv) + e ;// �⺻���� + ���� ���� + �����
            return a;
        }

        // ��� ���Ⱦ� ����Լ�
        public static int[] CalEquipments(Equipment equipment)
        {
            int[] a = new int[Nums.StatCount];
           for(int i=0; i < equipment.statUp.Length; i++)
            {
                a[i] = equipment.statUp[i]* equipment.ItemLv;
            }
            return a;
        }

        // ��ų �Ŀ� ��� �Լ�
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
        public float duration;  // �����ð�
        public float checkduration; // ������ ��Ʈ �ð�
        public int buffPow; //�����Ŀ�
       
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
        //ĳ���� ������ ���� ����
        public const int StatCount = 0;
        //��ų ���� ������ ����
        public const int SkillSetCount = 0;
        //ĳ���� ������� ���� ����
        public const int EquipmentCount = 0;
        //ĳ���͸� ��Ʋ�Ҷ� ��� ������ �� �ִ��� ����
        public const int CharacterEquipCount = 0;
        //ĳ������ �ְ� ����
        public const int CharacterMaxLv = 0;
        //�� ��ų�� �ְ� ����
        public const int SkillMaxLv = 0;
        //ĳ���� ���� HP�� ��ȣ
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



