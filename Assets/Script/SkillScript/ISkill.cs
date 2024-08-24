using UnityEngine;

public interface ISkill
{
    public int SkillType { get; set; }
    public float SkillDamage { get; set;}
    public float SkillCoolTime { get; set; }
} 
