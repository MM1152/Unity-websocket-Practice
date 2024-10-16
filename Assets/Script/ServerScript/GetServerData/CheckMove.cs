
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

    public override void RunNetworkCode(Data data)
    {
        
        for (int i = 0; i < socket.other.Count; i++)
        {
            if (socket.other[i].name == data.id.ToString())
            {
                OtherPlayerMove moveObject = socket.other[i].GetComponent<OtherPlayerMove>(); 
                moveObject.stateMachine.Transition(new MoveState() , new Vector2(data.x , data.y));
                break;
            }
        }
    }


}