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

    public void Update() { return; }

}