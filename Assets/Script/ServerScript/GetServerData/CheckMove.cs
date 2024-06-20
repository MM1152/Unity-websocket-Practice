
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CheckMove : ISocket
{
    
    public GameObject[] movingPlayer;
    public MoveObject movingPlayerMoveObj;
    private StateMachine stateMachine;
    public override void RunNetworkCode(Data data)
    {
        
        for (int i = 0; i < socket.other.Length; i++)
        {
            if (socket.other[i].name == data.id.ToString())
            {
                OtherPlayerMove moveObject = socket.other[i].GetComponent<OtherPlayerMove>(); 
                stateMachine = new StateMachine(moveObject);
                stateMachine.Transition(new MoveState() , new Vector2(data.x , data.y));
                break;
            }
        }
    }


}