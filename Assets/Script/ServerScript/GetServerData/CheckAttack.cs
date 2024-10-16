
using System.Linq;
using UnityEngine;

public class CheckAttack : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        
        OtherPlayerMove attackingUser = GameObject.Find(data.id.ToString()).GetComponent<OtherPlayerMove>();
        if(attackingUser.UserData.mapName == socket.this_player_MoveObject.UserData.mapName) {
            Debug.Log("Attack user : " + data.id);
            attackingUser.stateMachine.Transition(new AttackState());  
        }

    }
}