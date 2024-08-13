using System;
using UnityEngine;

public class SkillDatas{
    public SkillData[] skillsData;
}

[Serializable]
public class SkillData {
    public int skill_type;
    public int skill_damage;
    public float skill_cooltime;
    public string skill_info;
    public float coolDown;
}   