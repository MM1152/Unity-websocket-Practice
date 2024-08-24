using UnityEngine;

public class SkillState : Istate
{
    private IMoveObj moveObject;
    public void Enter(IMoveObj moveObject)
    {
        this.moveObject = moveObject;
        moveObject.useSkill = true;
    }

    public void Enter(IMoveObj moveObject, Vector2 targetPos){ }

    public void Exit(){ }

    public void Update()
    {
        if(!moveObject.useSkill) {
            moveObject.stateMachine.Transition(new IdleState());
        }
    }
}