

using UnityEngine;

[System.Serializable]
public class StateMachine{
    private Istate CurrentState;
    private MoveObject moveObject;

    public StateMachine(MoveObject moveObject){
        this.moveObject = moveObject;
        Debug.Log(moveObject);
    }
    public void Init(Istate state){
      
        CurrentState = state;
        CurrentState.Enter(moveObject);
    }

    public void Transition(Istate nextState){
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter(moveObject);
    } 
    public void Update(){
        CurrentState?.Update();
    }
}