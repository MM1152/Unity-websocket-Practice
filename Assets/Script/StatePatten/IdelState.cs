
using UnityEngine;

public class IdleState : Istate{
    private MoveObject moveObject;
    public void Enter(MoveObject moveObject){
        this.moveObject = moveObject;
    }
    public void Update(){
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            moveObject.stateMachine.Transition(new MoveState());
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            moveObject.stateMachine.Transition(new AttackState());
        }
    }
    public void Exit(){
        return;
    }
}