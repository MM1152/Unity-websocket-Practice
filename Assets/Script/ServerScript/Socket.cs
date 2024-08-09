using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System.Threading;
using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;

/// <summary>
/// ws://localhost:8000 연결 
/// </summary>
public class Socket : MonoBehaviour
{
    public List<Sprite> playerSprite;
    public List<RuntimeAnimatorController> playerAnimator;
    public GameObject ServerManager;
    public EnemyCount enemyCount;
    private static Socket socket;
    public ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();
    public string url;
    public GameObject[] other ;
    public MoveObject[] otherMoveObject;
    public GameObject user;
    public WebSocket ws;
    public Text text;
    public GameObject this_player;
    public MoveObject this_player_MoveObject;

    public Component[] components;
    public List<ISocket> Init = new List<ISocket>();
    public Socket(){
        socket = this;
    }
    public static Socket Instance
    {
        get{
            if(socket == null){
                socket = new Socket();
            }
            return socket;
        }
    }
    private void Awake(){
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 120;
        components = ServerManager.GetComponents<Component>();
        foreach(Component component in components){
            if(component is MonoBehaviour){
                Init.Add((ISocket)component);
            }
        }
        
        // OnMessage : 서버에서 메세지가 넘어오면 실행되는 함수
        try {
            
            using(ws = new WebSocket(url)) {
                
                ws.OnMessage += (sender , e) => {
                    try {
                    Data data =  JsonConvert.DeserializeObject<Data>(e.Data);

                    if(data.title == "CreateOtherUser" ){
                        Debug.Log(e.Data);
                    }
                    Action action = null;
                    for(int i = 0; i < Init.Count; i++){
                        if(Init[i].GetType().ToString().Equals(data.title)){
                            action = () => Init[i].RunNetworkCode(data);
                            break;
                        }
                    }
                    
                    addQueue(action);
                    } 
                    catch(Exception ex) {
                        Debug.LogError(ex.Message);
                    }
                };
            }
            
        
        //ws.Connect();
      
        } catch(Exception e){
            Debug.LogError("서버 연결 실패");
        }
        
    }

    void addQueue(Action action){
        queue.Enqueue(action);
    }

    void Update(){
        while(queue.Count > 0){
            if(queue.TryDequeue(out var action)){
                action?.Invoke();
            }
        }
    }


    private void OnApplicationQuit() {
        Data data = new Data("SaveData");
        data.this_player = this_player_MoveObject.getUserData();
        ws.Send(JsonUtility.ToJson(data));
        ws.Close();
    }

}
 
 
