using System;
using UnityEngine;

[Serializable]
public class SkillDatas{
    public SkillData[] skillsData;
}

[Serializable]
public class SkillData {
    public int skill_type;
    public float skill_damage;
    public float skill_cooltime;
    public string skill_info;
    public int mp_cost;
    public float coolDown = 0;
}   
