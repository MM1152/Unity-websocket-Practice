
using System.Linq;
using UnityEngine;

public class CreateOtherUser : ISocket
{
    public void Start()
    {
        setSocket();
    }
    public override void RunNetworkCode(Data data)
    {
        socket.other = GameObject.FindGameObjectsWithTag("Player");
        if(socket.other.Length != data.users.Length){
            for(int i = 0; i < data.users.Length; i++){
                bool isCreated = false;
                for(int j = 0; j < socket.other.Length; j++){
                    if(socket.other[j].name == data.users[i].ToString()){
                        isCreated = true;
                        break;
                    }
                }
                if(!isCreated){
                    GameObject createUser = Instantiate(socket.user) as GameObject;
                    createUser.name = data.users[i].ToString();
                }
            }
        }
        socket.other = GameObject.FindGameObjectsWithTag("Player");
    }
}