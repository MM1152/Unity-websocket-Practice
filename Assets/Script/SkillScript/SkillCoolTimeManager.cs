using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SkillCoolTimeManager : MonoBehaviour
{
    public static List<SkillData> skillData;
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
        foreach(var skill in skillData){
            if(skill.coolDown > 0) skill.coolDown -=  Time.deltaTime;
        }
    }
}
