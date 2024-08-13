using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SkillCoolTimeManager : MonoBehaviour
{
    private List<SkillData> skillData;
    public SkillData SkillData {
        set {
            skillData.Add(value);
            skillData.coolDown = skillData.skill_cooltime;
        } 
    } 
    public static Dictionary<string , int> thisSlotEquipSlot = new Dictionary<string, int>() {
        { "0", 0 },
        { "1", 0 },
        { "2", 0 },
        { "3", 0 },
        { "4", 0 },
        { "5", 0 },
        { "6", 0 },
        { "7", 0 },
    } ;
    private void Update() {
        if(skillData != null &&  skillData.coolDown > 0) {
            Debug.Log("스킬 쿨타임 도는중 : " + skillData.coolDown);
            skillData.coolDown -= Time.deltaTime;
        }
    }
}
