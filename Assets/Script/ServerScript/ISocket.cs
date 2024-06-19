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
    protected ISocket(){
        socket = Socket.Instance;
    }
    private void Start() {
        enemyCount ??= GameObject.Find("EnemyCount").GetComponent<EnemyCount>();    
    } 
    public abstract void RunNetworkCode(Data data);
}