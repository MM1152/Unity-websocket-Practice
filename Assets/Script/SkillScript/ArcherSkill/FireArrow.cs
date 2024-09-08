using System.Collections;
using UnityEngine;


public class FireArrow : ISkill {
    GameObject target;
    public override void UseSkill()
    {
        target = Socket.Instance.this_player_MoveObject.FindNearEnemy();
        base.UseSkill();
    }
    public override IEnumerator WaitFor_SkillShow()
    { 
        yield return new WaitForSeconds(0.5f);
        skill.pos.GetComponent<IMoveObj>().useSkill = false;
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 3f);
        finSkillMotion = false;
    }
    public override void Update()
    {
        skill.transform.position += (target.transform.position - skill.transform.position) * Time.deltaTime * 5f;
    }
    public override void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            Socket.Instance.this_player_MoveObject.HitEnemy(other.gameObject ,
            (int)((float)Socket.Instance.this_player_MoveObject.attack * SkillCoolTimeManager.skillData[skill_index].skill_damage) ,
            Socket.Instance.this_player_MoveObject.UserData);
            Socket.Instance.this_player_MoveObject.useSkill = false;
            finSkillMotion = false;
        }
    }
}