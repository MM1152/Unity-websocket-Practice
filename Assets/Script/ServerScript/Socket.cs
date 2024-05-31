using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;
using WebSocketSharp;
/*
[Serializable]
public class Data{
    public string title;
    public int id;
    public int[] users;
    public float x;
    public float y;
    public Data(string title){
        this.title = title;
    }
    public Data(string title , int id){
        this.title = title;
        this.id = id;
    }
    public Data(string title , int[] users){
        this.title = title;
        this.users = users;
    }
    public Data(string title , int id, float x , float y){
        this.title = title;
        this.id = id;
        this.x = x;
        this.y = y;
    }
    public Data getData(){
        return this;
    }
}
*/

/// <summary>
/// ws://localhost:8000 연결 
/// </summary>
public class Socket : MonoBehaviour
{

    public GameObject[] other ;
    public MoveObject[] otherMoveObject;
    public GameObject user;
    private WebSocket ws;
    private string userName;
    public Text text;
    public GameObject this_player;
    public MoveObject this_player_MoveObject;
    public ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

    private void Awake(){
        // OnMessage : 서버에서 메세지가 넘어오면 실행되는 함수
        try {
            
            using(ws = new WebSocket("ws://localhost:8000")) {
                
                ws.OnMessage += (sender , e) => {
                    
                    Data data = JsonUtility.FromJson<Data>(e.Data);
                    Debug.Log(data.title);
                    if(data.title.Equals("checkUserID")){
                        userName = data.id.ToString();
                        queue.Enqueue(() => CreateUser());
                    }if(data.title.Equals("checkOutUser")){   
                        queue.Enqueue(() => DeleteUser(data.id));
                    }if(data.title.Equals("checkCreateUser")){
                        queue.Enqueue(() => CreateOtherUser(data.id , data.users));
                    }if(data.title.Equals("checkMove")){
                        Debug.Log($"ID : {data.id} , [{data.x},{data.y}]");
                        Debug.Log($"otherLength : {other.Length}");
                        queue.Enqueue(() => MoveOtherCharector(data));
                    }
                };
            }
            
            ws.Connect();
            
        } catch(Exception e){
            Debug.LogError("서버 연결 실패");
        }
        
    }
    void Update(){
        while(queue.Count > 0){
            if(queue.TryDequeue(out var action)){
                action?.Invoke();
            }
        }
    }
    void LateUpdate(){
        queue.Enqueue(() => MoveCharector(int.Parse(this_player.name)));
        this_player_MoveObject.Move();
        AttackUser();
    }
    public void AttackUser(){
        if(Input.GetKeyDown(KeyCode.Space) && !this_player_MoveObject.attackShow.activeSelf){
            StartCoroutine(this_player_MoveObject.Attack());
        }
        if(this_player_MoveObject.isAttackSussecs){
            Data attackData = new Data("AttackOtherPlayer" , int.Parse(this_player_MoveObject.attackingPlayer.name));
            ws.Send(JsonUtility.ToJson(attackData));
            this_player_MoveObject.isAttackSussecs = false;
        }
    }
    public void CreateUser(){
        GameObject player = Instantiate(user);
        this_player = player;
        this_player_MoveObject = this_player.GetComponent<MoveObject>();
        player.name = userName;
        Data data = new Data("Connection");
        ws.Send(JsonUtility.ToJson(data));
    }
    public void OnApplicationQuit(){
        Data data = new Data("EndConnection" , int.Parse(userName));
        ws.Send(JsonUtility.ToJson(data));
    }
    public void CreateOtherUser(int createUserId , int[] users){
        other = GameObject.FindGameObjectsWithTag("Player");
        if(other.Length != users.Length){
            for(int i = 0; i < users.Length; i++){
                bool isCreated = false;
                for(int j = 0; j < other.Length; j++){
                    if(other[j].name == users[i].ToString()){
                        isCreated = true;
                        break;
                    }
                }
                if(!isCreated){
                    GameObject createUser = Instantiate(user) as GameObject;
                    createUser.name = users[i].ToString();
                }
            }
        }
        other = GameObject.FindGameObjectsWithTag("Player");
    }
    public void DeleteUser(int deleteIndex){
        Destroy(GameObject.Find(deleteIndex.ToString()).gameObject);
    }
    public void MoveCharector(int movingPlayer){
        Vector2 move = this_player_MoveObject.GetPosition();
        Data data = new Data("PlayerMove" ,movingPlayer, move.x , move.y);
        ws.Send(JsonUtility.ToJson(data));
    }
    public void MoveOtherCharector(Data data){
        for(int i = 0; i < other.Length; i++){
            Debug.Log($"OtherName : {other[i].name}");
            if(other[i].name == data.id.ToString()){
                other[i].transform.position = new Vector3(data.x , data.y);
                break;
            }
        }
    }

}
