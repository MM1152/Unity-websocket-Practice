using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour 
{
    [Header ("타겟")]
    [SerializeField] private GameObject target;
    [Space(10)]
    [Header ("사용자 위치")]
    public Transform pos;
    [Space(10)]
    private ISkill skill;
    private SpriteRenderer sp;
    public int skill_index;
    public string skillAnimationClip;
    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }  
    private void OnEnable() {
        if(skillAnimationClip != null && skillAnimationClip != "") {
            skill = CreateSkillInstance(skillAnimationClip.Split(' ')[1]);
            skill.skill = this;
            skill.UseSkill();
            skill.SetAnimation(skillAnimationClip);
            transform.position = (Vector3)pos.position + (Vector3)skill.SetPosition(sp.flipX);
             pos.GetComponent<IMoveObj>().stateMachine.Transition(new SkillState());
             StartCoroutine(skill.WaitFor_SkillShow());
        }

        //StartCoroutine(WaitFor_SkillShow());    
    }
    private ISkill CreateSkillInstance(string skillName){
        Type type = Type.GetType(skillName);
        return Activator.CreateInstance(type) as ISkill;
    }
    private void Update() {
        if(skill != null && !skill.finSkillMotion) {
            SkillPooling.Instance.ReturnObject(this);
            skill = null;
        }
        if(BuffSkillController.buffType.ContainsKey(skillAnimationClip.Split(' ')[1]) && BuffSkillController.buffType[skillAnimationClip.Split(' ')[1]] < 0) {
            skill.finSkillMotion = true;
            SkillPooling.Instance.ReturnObject(this);
        }
        skill?.Update();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        skill.OnTriggerEnter2D(other);
    }
}
