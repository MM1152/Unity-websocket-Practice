

using UnityEngine;

[System.Serializable]
public class StateMachine{
    private Istate CurrentState;
    private IMoveObj moveObject;

    public StateMachine(IMoveObj moveObject){
        this.moveObject = moveObject;
    }
    public Istate getState() {
        return CurrentState;
    }
    public void Init(Istate state){
        CurrentState = state;
        CurrentState.Enter(moveObject);
    }
    public void Transition(Istate nextState){
        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter(moveObject);
    } 
    public void Transition(Istate nextState , Vector2 targetPos){
        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter(moveObject, targetPos);
    }
    public void Update(){
        CurrentState?.Update();
    }
}