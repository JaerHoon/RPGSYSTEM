using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGSYSTEM;
using System;
using System.Reflection;

public class SKillManager : MonoBehaviour
{
    List<SkillData> skillDatas = new List<SkillData>();
    List<Skill<SkillData>> skills = new List<Skill<SkillData>>();

    private void Start()
    {
        SkillData skill = new SkillData();

        skill.skillName = "FireBall";
        skill.skillPow = 10;

        skillDatas.Add(skill);

        SkillData skill1 = new SkillData();

        skill1.skillName = "IceWall";
        skill.skillPow = 10;

        skillDatas.Add(skill1);

        for(int i=0; i < skillDatas.Count; i++)
        {
           
            Type type = Type.GetType(skillDatas[i].skillName);

            Skill<SkillData> _skill = gameObject.AddComponent(type) as Skill<SkillData>;

            _skill.Init(skillDatas[i]);

            skills.Add(_skill);
        }

        foreach (Skill<SkillData> Skill in skills)
        {
            Skill.OnSkill();
        }
    }
}

public struct SkillData
{
    public string skillName;
    public int skillPow;
}
