using UnityEngine;

public interface ISkill
{
    public int skillType { get; set; }
    public int skillDamage { get; set;}
    public int skillCoolTime { get; set; }
    void UseSkill();
} 
