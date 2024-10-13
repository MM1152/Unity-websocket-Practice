using UnityEngine;

public class CheckPlayerHP : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        Debug.Log("Recovery");
        if(data.this_player.id == Socket.Instance.this_player_MoveObject.getUserData().id) {
            Socket.Instance.this_player_MoveObject.getUserData().hp = data.this_player.hp;
            Socket.Instance.this_player_MoveObject.getUserData().mp = data.this_player.mp;
        }
        else {
            IMoveObj this_player = GameObject.Find(data.this_player.id).GetComponent<IMoveObj>();
            this_player.UserData.hp = data.this_player.hp;
            this_player.UserData.mp = data.this_player.mp;
        }
        
        
    }
}