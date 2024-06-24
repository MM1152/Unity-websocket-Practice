using UnityEngine;

public class DieState : Istate
{
    private IMoveObj moveObject;
    public void Enter(IMoveObj moveObject)
    {
        this.moveObject = moveObject;
    }

    public void Enter(IMoveObj moveObject, Vector2 targetPos){ }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        
    }
}