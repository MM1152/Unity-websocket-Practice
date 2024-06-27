using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System.Threading;

public abstract class ISocket : MonoBehaviour
{
    protected Socket socket;
    protected EnemyCount enemyCount;
    public string name;
    public NPCcount NPCcount;
    protected ISocket(){
        socket = Socket.Instance;
    }
    public void Start() {
        enemyCount ??= GameObject.Find("EnemyCount").GetComponent<EnemyCount>();    
        NPCcount = GameObject.Find("NPCcount").GetComponent<NPCcount>();
    }
    public abstract void RunNetworkCode(Data data);
}