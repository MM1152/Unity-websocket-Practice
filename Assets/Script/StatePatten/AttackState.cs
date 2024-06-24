using UnityEngine;

public class AttackState : Istate
{
    private IMoveObj moveObject;
    public void Enter(IMoveObj moveObject)
    {
        this.moveObject = moveObject;
        moveObject.ani.SetTrigger("IsAttack");
        if(moveObject.gameObject.activeSelf) moveObject.Attack();
    }

    public void Enter(IMoveObj moveObject, Vector2 targetPos){ }

    public void Exit(){ }

    public void Update()
    {
        if(!moveObject.IsAttack){
            moveObject.stateMachine.Transition(new IdleState());
        }
    }
}