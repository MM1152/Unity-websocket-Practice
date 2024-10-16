using System;
using UnityEngine;

[Serializable]
public class SkillDatas{
    public SkillData[] skillsData;
}

[Serializable]
public class SkillLearn {
    public int[] learned_skill;
}

[Serializable]
public class SkillLearnData {
    public SkillLearn[] skillLearn;
}

[Serializable]
public class SkillData {
    public string id;
    public int skill_type;
    public float skill_damage;
    public float skill_cooltime;
    public string skill_info;
    public int mp_cost;
    public float coolDown = 0;
    public int learnableLevel;
    public int learnableGold;
}   
