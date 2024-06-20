public class AttackState : Istate
{
    private MoveObject moveObject;
    public void Enter(MoveObject moveObject)
    {
        this.moveObject = moveObject;
        moveObject.ani.SetTrigger("IsAttack");
        moveObject.Attack();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if(!moveObject.attackShow.activeSelf){
            moveObject.stateMachine.Transition(new IdleState());
        }
    }
}