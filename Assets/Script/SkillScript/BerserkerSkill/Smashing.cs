using System.Collections;
using UnityEngine;

public class Smashing : ISkill 
{
    public override void UseSkill()
    {
        base.UseSkill();
    }
    public override IEnumerator WaitFor_SkillShow()
    { 
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        skill.pos.GetComponent<IMoveObj>().useSkill = false;
        finSkillMotion = false;
    }
    public override void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            Debug.Log($"Player Attack : {Socket.Instance.this_player_MoveObject.attack} SkillDamage : {SkillCoolTimeManager.skillData[skill_index].skill_damage}");
            Socket.Instance.this_player_MoveObject.HitEnemy(other.gameObject ,
            (int)((float)Socket.Instance.this_player_MoveObject.attack * SkillCoolTimeManager.skillData[skill_index].skill_damage) ,
            Socket.Instance.this_player_MoveObject.UserData);
        }
    }
    public override Vector2 SetPosition(bool flipX)
    {
        Vector2 pos = flipX ? new Vector2(-0.5f , 0f) : new Vector2(0.5f , 0f);
        return pos;
    }
}