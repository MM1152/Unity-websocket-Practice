
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CheckMove : ISocket
{
    public float speed;
    public GameObject movingPlayer;
    public MoveObject movingPlayerMoveObj;
    Vector2 startPos;
    Vector2 targetPos;
    float currentTime;
    bool isMoving;

    public void Start()
    {
        setSocket();

    }
    public override void RunNetworkCode(Data data)
    {
        for (int i = 0; i < socket.other.Length; i++)
        {
            if (socket.other[i].name == data.id.ToString())
            {
                movingPlayer = socket.other[i];

                targetPos = new Vector3(data.x, data.y);
                startPos = movingPlayer.transform.position;
                movingPlayerMoveObj = movingPlayer.GetComponent<MoveObject>();
                movingPlayerMoveObj.moveX = data.moveXY.x;
                movingPlayerMoveObj.moveY = data.moveXY.y;
                movingPlayerMoveObj.SetAnimation(State.MOVE);
                

                if (!isMoving)
                {
                    StartCoroutine(Moving());
                }
                break;
            }
        }




    }

    IEnumerator Moving()
    {
        isMoving = true;
    
        for (float i = 0f; i <= 1.0f; i += speed)
        {
            movingPlayer.transform.position = Vector2.Lerp(startPos, targetPos, i);
            yield return null;
            Debug.Log($"WHO : {movingPlayer}  start Pos : {startPos}  Target Pos : {targetPos}");
        }
        movingPlayer.transform.position = targetPos;
        movingPlayerMoveObj.moveX = 0;
        movingPlayerMoveObj.moveY = 0;
        movingPlayerMoveObj.SetAnimation(State.IDLE);
        
        isMoving = false;
    }
}