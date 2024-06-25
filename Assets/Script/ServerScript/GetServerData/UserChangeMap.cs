using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserChangeMap : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        Debug.Log("맵을 이동한 유저 : " + data.this_player.id);
        for(int i = 0; i < socket.other.Length; i++){
            if(socket.other[i].name == data.this_player.id && data.this_player.mapName != socket.this_player_MoveObject.getUserData().mapName) {
                socket.other[i].SetActive(false);
                break;
            }
            else if(socket.other[i].name == data.this_player.id && data.this_player.mapName == socket.this_player_MoveObject.getUserData().mapName) {
                socket.other[i].SetActive(true);
                break;
            }
        }
    }
}
