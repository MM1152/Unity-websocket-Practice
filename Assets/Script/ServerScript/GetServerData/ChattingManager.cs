using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChattingManager : ISocket
{
    GameObject thisPlayer;
    MoveObject thisPlayerData;
    PlayerReference playerReference;
    public override void RunNetworkCode(Data data)
    {       
        thisPlayer ??= Socket.Instance.this_player;
        thisPlayerData ??= Socket.Instance.this_player_MoveObject;
        playerReference ??= thisPlayer.gameObject.GetComponent<PlayerReference>();

        string mapName = data.this_player.mapName;
        bool isPlayer = data.this_player.id == thisPlayerData.UserData.id;

        if(isPlayer) {
            playerReference.chat.SetActive(true);
            playerReference.chat.GetComponent<ChattingSystem>().chattingText = data.chattingText;
        } 
        else {
            if(mapName != Socket.Instance.this_player_MoveObject.getUserData().mapName) return;
            foreach(var player in Socket.Instance.other) 
            {
                if(player.GetComponent<IMoveObj>().UserData.id == data.this_player.id) {
                    player.GetComponent<PlayerReference>().chat.SetActive(true);
                    player.GetComponent<PlayerReference>().chat.GetComponent<ChattingSystem>().chattingText = data.chattingText;
                }
            }

        }
    }
}
