
using System.Linq;
using UnityEngine;

public class CheckOutUser : ISocket
{
    public override void RunNetworkCode(Data data)
    {        
        for(int i = 0; i < socket.other.Count; i++) {
            if(data.id == socket.other[i].name) {
                Destroy(socket.other[i]);
                socket.other.RemoveAt(i);
            }
        }
        
    }
}