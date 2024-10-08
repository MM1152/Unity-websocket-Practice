using System.Collections;
using System.IO;
using UnityEngine;


public class ArcherBuffEffect: ISkill{
    public override void UseSkill()
    {
        if(BuffSkillController.buffType.ContainsKey(this.GetType().Name) && skill.pos.gameObject == Socket.Instance.this_player) {
            BuffSkillController.buffType[this.GetType().Name] = 60f;
            SkillPooling.Instance.ReturnObject(skill);
            Socket.Instance.this_player_MoveObject.useSkill = false;
            return;     
        } else {
            BuffSkillController.buffType.Add(this.GetType().Name , 60f);
            Socket.Instance.this_player_MoveObject.attack += 20; 
        }
        base.UseSkill();
    }
    public override void Update()
    {
        skill.transform.position = skill.transform.position = skill.pos.position;
    }
    public override IEnumerator WaitFor_SkillShow()
    { 
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        skill.pos.GetComponent<IMoveObj>().useSkill = false;
    }
    public override void OnTriggerEnter2D(Collider2D other) {
        return;
    }
}