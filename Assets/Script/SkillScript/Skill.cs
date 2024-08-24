using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AttackCharacter {
    ShortRange , LongDistance
}

public class Skill : MonoBehaviour
{
    [Header ("타겟")]
    [SerializeField] private GameObject target;
    [Space(10)]
    private Animator ani;
    
    public string skillAnimationClip;
    public AttackCharacter attackCharacter;
    void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnEnable() {
        Socket.Instance.this_player_MoveObject.stateMachine.Transition(new SkillState());
        target = null;
        StartCoroutine(WaitFor_SkillShow());    
    }
    private void Update() {
        if(target != null) {
            transform.position += (target.transform.position - transform.position)  * Time.deltaTime * 5f;
        }
    }
    IEnumerator WaitFor_SkillShow(){
        ani.Play(skillAnimationClip);
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Socket.Instance.this_player_MoveObject.useSkill = false;
        if(skillAnimationClip != "3 FireArrow") SkillPooling.Instance.ReturnObject(this);
        else{
            target = Socket.Instance.this_player_MoveObject.FindNearEnemy();
            yield return new WaitForSeconds(3f);
            SkillPooling.Instance.ReturnObject(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            
            if(skillAnimationClip == "3 FireArrow") {
                SkillPooling.Instance.ReturnObject(this);
            }
        }
    }
}
