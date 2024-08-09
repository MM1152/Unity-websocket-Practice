using UnityEngine;

public interface ISkill
{
    public int SkillType { get; set; }
    public int SkillDamage { get; set;}
    public int SkillCoolTime { get; set; }
    void UseSkill();
} 
