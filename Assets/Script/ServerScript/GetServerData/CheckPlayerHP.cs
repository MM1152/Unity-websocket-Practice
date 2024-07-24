using UnityEngine;

public class CheckPlayerHP : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        IMoveObj this_player = GameObject.Find(data.this_player.id).GetComponent<IMoveObj>();
        Debug.Log("Recovery");
        this_player.SetUserData(data.this_player);
    }
}