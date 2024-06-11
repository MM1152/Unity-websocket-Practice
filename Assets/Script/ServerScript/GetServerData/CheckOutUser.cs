
using System.Linq;
using UnityEngine;

public class CheckOutUser : ISocket
{
    public void Start()
    {
        setSocket();
    }
    public override void RunNetworkCode(Data data)
    {        
        Destroy(GameObject.Find(data.id.ToString()).gameObject);
        socket.other = GameObject.FindGameObjectsWithTag("Player");
    }
}