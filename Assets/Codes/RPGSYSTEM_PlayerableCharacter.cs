using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM;

namespace RPGSYSTEM.PlayableCharacter
{
    public class PlayableCharacter : MonoBehaviour
    {
        protected Character myCharacter;
        protected int HP;

        public virtual void Init()
        {
            HP = myCharacter.GetStat<int>(Nums.CharacterHP, false);
        }

        public virtual void ChangeStat(int stat, int amount)
        {
            myCharacter.Temp_statUp(stat, amount);
        }

        public virtual void RestoreStat(int stat, int amount)
        {
            myCharacter.Temp_statRestore(stat, amount);
        }

        public virtual void ChangeHP(int amount)
        {
            HP -= amount;

            if (HP <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {

        }

        public virtual void OnSkill(Skill skill)
        {

        }
    }

    public class CharacterSkill : MonoBehaviour
    {
        public List<IAtionSkill> ative_AtionSkills = new List<IAtionSkill>();
        public List<IPassiveSkill> ative_PassiveSkills = new List<IPassiveSkill>();
        public List<IConditionSkill> ative_ConditionSkills = new List<IConditionSkill>();
        public List<IBuffDebuffSkill> ative_BuffDeffSkills = new List<IBuffDebuffSkill>();

        public virtual void OnSkill(Skill skill)
        {
            skill.OnSkill();
            switch (skill.skillType)
            {
                case Enums.SkillType.Ation: 
                    ative_AtionSkills.Add((IAtionSkill)skill);
                    break;
                case Enums.SkillType.Passive:
                    ative_PassiveSkills.Add((IPassiveSkill)skill);
                    break;
                case Enums.SkillType.Condtion:
                    ative_ConditionSkills.Add((IConditionSkill)skill);
                    break;
                case Enums.SkillType.BuffDebuff:
                    ative_BuffDeffSkills.Add((IBuffDebuffSkill)skill);
                    break;
            }
        }

        public virtual void CheckCondtion(Enums.SkillConditionType skillConditiontype)
        {
            for(int i = 0; i < ative_ConditionSkills.Count; i++)
            {
                if(ative_ConditionSkills[i].skillConditionType == skillConditiontype)
                {
                    ative_ConditionSkills[i].CheckCondition();

                }
            }
        }
    }

    public class CharacterBattle : MonoBehaviour
    {
        protected virtual (int pow, bool IsCrt) Cal_AttackPow(int skillpow)
        {
            int pow = 0;
            bool IsCrt = false;

            return (pow, IsCrt);
        }

        public virtual void OnAttack(CharacterBattle character, int skillpow)
        {

            //공격력 올려주는 요소들을 여기서 모두 검사한 뒤 최종 파워를 반환해야함.
            var result = Cal_AttackPow(skillpow);

            int atkpow = result.pow;
            bool IsCrt = result.IsCrt;

            character.OnDamage(atkpow, IsCrt);

        }

        public virtual void OnDamage(int atkpow, bool IsCrt)
        {
            //회피계산, 방어계산등등
        }

        public virtual void ContinuosDamage(int pow, float duration)
        {
            //아마 코루틴을 만들어야 할 듯.
        }

        public virtual void UIDamage(int pow, bool IsCrt)
        {

        }
    }
   
    public class CharacterBuffDebuff : MonoBehaviour
    {

    }
}