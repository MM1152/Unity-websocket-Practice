
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CheckMove : ISocket
{
    public float speed;
    public GameObject[] movingPlayer;
    public MoveObject movingPlayerMoveObj;

    public override void RunNetworkCode(Data data)
    {
        for (int i = 0; i < socket.other.Length; i++)
        {
            if (socket.other[i].name == data.id.ToString())
            {
                MoveObject moveObject = socket.other[i].GetComponent<MoveObject>(); 
                Debug.Log(moveObject.gameObject.name + "  data.MoveXY : " + data.moveXY.x);
                moveObject.moveX = data.moveXY.x;
                moveObject.moveY = data.moveXY.y;
                moveObject.SetAnimation(State.MOVE);
                StartCoroutine(moveObject.Moving(new Vector2(data.x , data.y)));
                break;
            }
        }
    }


}