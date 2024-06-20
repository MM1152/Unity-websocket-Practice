using UnityEngine;

public class MoveState : Istate{

    private IMoveObj moveObject;
    private Socket socket = null; 
    public void Enter(IMoveObj moveObject)
    {
        this.moveObject = moveObject;
        moveObject.ani.SetBool("IsRun" , true);
    }

    public void Enter(IMoveObj moveObject , Vector2 targetPos){
        this.moveObject = moveObject;
        moveObject.ani.SetBool("IsRun" , true);
        moveObject.Move(targetPos);
    }

    public void Update()
    {
        moveObject.Move();

        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0){
            moveObject.stateMachine.Transition(new IdleState());
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            moveObject.stateMachine.Transition(new AttackState());
        }
    }

    public void Exit()
    {
        moveObject.ani.SetBool("IsRun" , false);
    }

}