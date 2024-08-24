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
    public override void ShowObject(Vector2 showPos, int value)
    {
        Skill skill;
        if(pooling.Count > 0) {
            skill = pooling.Dequeue();
            skill.transform.position = showPos;
        }else {
            skill = Instantiate(prefab , transform);
            skill.gameObject.SetActive(false);
            skill.transform.position = showPos;   
        }
        skill.GetComponent<SpriteRenderer>().flipX = Socket.Instance.this_player_MoveObject.moveX > 0 ? false : true;
        skill.skillAnimationClip = animationClips[value].name;
        skill.gameObject.SetActive(true);
    }
}
