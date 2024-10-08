
using UnityEngine;

public class IdleState : Istate
{
    private IMoveObj moveObject;
    
    public void Enter(IMoveObj moveObject)
    {
        
        this.moveObject = moveObject;
    }
    public void Enter(IMoveObj moveObject, Vector2 targetPos) { }

    public void Update()
    {

        if (moveObject.gameObject.activeSelf)
        {
            if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !moveObject.IsAttack)
            {
                moveObject.stateMachine.Transition(new MoveState());
            }
            if (Input.GetKeyDown(KeyCode.Space) && moveObject.attackTime <= 0 && !moveObject.IsAttack)
            {
                moveObject.stateMachine.Transition(new AttackState());
                Socket.Instance.ws.Send(JsonUtility.ToJson(new Data("AttackOtherPlayer" , moveObject.gameObject.name)));
            }
        }

    }
    public void Exit() { return; }
}