using UnityEngine;

public class HitState : Istate
{
    private IMoveObj moveObject;
    public void Enter(IMoveObj moveObject)
    {
        if(!moveObject.IsAttack) {
            this.moveObject = moveObject;
            moveObject.ani.SetTrigger("IsHit");
            moveObject.IsAttack = false;
        }        
    }
    public void Enter(IMoveObj moveObject, Vector2 targetPos){}

    public void Exit() { }
    
    public void Update() { 
        // fix !!
        if(moveObject.ani.GetCurrentAnimatorStateInfo(0).IsName("PlayerHit") && moveObject.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9){
            moveObject.stateMachine.Transition(new IdleState());
        }
    }

}