using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStat : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        Debug.Log(data.id);
        if(data.id == "Str") {
            Socket.Instance.this_player_MoveObject.attack += 5;  
        }    
    }
}


