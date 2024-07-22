using UnityEngine;

public class HitState : Istate
{
    private IMoveObj moveObject;
    public void Enter(IMoveObj moveObject)
    {
        this.moveObject = moveObject;
        if(!moveObject.IsAttack){
            moveObject.ani.SetTrigger("IsHit");
           
        }
    }
    public void Enter(IMoveObj moveObject, Vector2 targetPos){}

    public void Exit() { }

    public void Update() { 
        // fix !!
        if(moveObject.ani.GetCurrentAnimatorStateInfo(0).IsName("PlayerHit") && moveObject.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f || Input.GetKeyDown(KeyCode.Space)){
            moveObject.stateMachine.Transition(new IdleState());
        }
    }

}