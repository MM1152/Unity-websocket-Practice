using System.Collections; 
using UnityEngine;
public abstract class ISkill
{  
    public Skill skill;
    public Animator ani; 
    public bool finSkillMotion; 
    public int skill_index;


    public virtual void UseSkill() { 
       
        finSkillMotion = true;
        ani = skill.GetComponent<Animator>();
        
        /* fix !
        Data data = new Data("UseSkill");
        data.id = Socket.Instance.this_player.name;
        data.skillinfo = skill.skill_index; 
        data.useItemType = SkillCoolTimeManager.skillData[skill.skill_index].mp_cost; // 사용한 스킬의 MP 소모량을 Data의 useItemType을 재사용
        Socket.Instance.ws.Send(JsonUtility.ToJson(data));
        */
    }
    public virtual void SetAnimation(string skillAnimationClip) {
        ani.Play(skillAnimationClip);
    }
    public abstract IEnumerator WaitFor_SkillShow(); 
    public abstract void OnTriggerEnter2D(Collider2D other);
    public virtual void Update() {return; }
    public virtual Vector2 SetPosition(bool flipX){ return Vector2.zero; }
} 
