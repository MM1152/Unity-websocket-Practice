using System;
using UnityEngine;

public class SkillDatas{
    public SkillData[] skillsData;
}

[Serializable]
public class SkillData {
    public int skillType;
    public int skillDamage;
    public int skillCoolTime;
}   