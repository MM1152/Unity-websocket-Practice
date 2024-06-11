using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System.Threading;

public abstract class ISocket : MonoBehaviour
{
    protected Socket socket;
    public string name;
    public virtual void setSocket() {
        socket = Socket.Instance;
    }
    public abstract void RunNetworkCode(Data data);
}