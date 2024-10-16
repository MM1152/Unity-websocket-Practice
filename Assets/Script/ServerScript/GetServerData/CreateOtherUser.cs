
using System.Linq;
using UnityEngine;

public class CreateOtherUser : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        socket.other = GameObject.FindGameObjectsWithTag("Player").ToList();
        
        if(socket.other.Count != data.users.Length){
            
            for(int i = 0; i < data.users.Length; i++){
                bool isCreated = false;
                for(int j = 0; j < socket.other.Count; j++){
                    if(socket.other[j].name == data.users[i].id.ToString()){
                        isCreated = true;
                        break;
                    }
                }
                if(!isCreated){
                    GameObject createUser = Instantiate(socket.user) as GameObject;
                    createUser.AddComponent<OtherPlayerMove>();
                    createUser.name = data.users[i].id.ToString();
                    createUser.GetComponent<OtherPlayerMove>().UserData = data.users[i];
                    createUser.GetComponent<OtherPlayerMove>().Type = data.users[i].type;
                    socket.other.Add(createUser);
                    if(data.users[i].mapName != socket.this_player_MoveObject.playerMap){
                        createUser.SetActive(false);
                    }
                }
            }
        }
        
    }
}