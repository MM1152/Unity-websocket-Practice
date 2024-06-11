
using Unity.VisualScripting;
using UnityEngine;

public class Init : ISocket
{
    
    public void Start()
    {
        setSocket();
    }
    public override void RunNetworkCode(Data data)
    {

        GameObject player = Instantiate(socket.user);

        socket.this_player = player;
        socket.this_player_MoveObject = socket.this_player.GetComponent<MoveObject>();
        player.name = data.id;
        data = new Data("Connection");
        socket.ws.Send(JsonUtility.ToJson(data));

    }
}