using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPooling : PoolingManager<Skill>
{
    [SerializeField] AnimationClip[] animationClips;
    public override void Awake() {
        animationClips = Resources.LoadAll<AnimationClip>("SkillAnimation");
        base.Awake();
    }
    /// <summary>
    /// SkillEffect is shown using SkillPooling
    /// </summary>
    /// <param name="showPos"> Setting Skill Position</param>
    /// <param name="value"> Setting SkillType Number</param>
    public override void ShowObject(Transform showPos, int value)
    {
        Skill skill;
        
        if(pooling.Count > 0) {
            skill = pooling.Dequeue();
            skill.transform.position = showPos.position;
        }else {
            skill = Instantiate(prefab , transform);
            skill.gameObject.SetActive(false);
            skill.transform.position = showPos.position;   
        }
        /*
        
        skill.skill_index = value; //
        skill.skillAnimationClip = animationClips[SkillCoolTimeManager.skillData[value].skill_type - 1].name;
        */
        skill.GetComponent<SpriteRenderer>().flipX = showPos.GetComponent<IMoveObj>().sp.flipX ? true : false;
        skill.skill_index = value;
        skill.pos = showPos;
        skill.skillAnimationClip = animationClips[value].name;
        skill.gameObject.SetActive(true);
    }
}
