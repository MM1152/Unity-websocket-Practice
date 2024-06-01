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
using UnityEngine.XR;
using WebSocketSharp;
using System.Threading;
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
    private Thread thread;
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
            
            using(ws = new WebSocket("ws://172.30.1.10:8000")) {
               
                ws.OnMessage += (sender , e) => {
                    
                    Data data = JsonUtility.FromJson<Data>(e.Data);
                    
                    if(data.title.Equals("checkUserID")){
                        userName = data.id.ToString();
                        queue.Enqueue(() => CreateUser());
                    }if(data.title.Equals("checkOutUser")){   
                        queue.Enqueue(() => DeleteUser(data.id));
                    }if(data.title.Equals("checkCreateUser")){
                        queue.Enqueue(() => CreateOtherUser(data.id , data.users));
                    }if(data.title.Equals("checkMove")){
                        Debug.Log(data.x);
                        //thread = new Thread(() => addQueue(() => MoveCharector(data.id)));
                       queue.Enqueue(() => MoveOtherCharector(data));
                    }if(data.title.Equals("checkAttack")){
                        queue.Enqueue(() => AttackShow(data.id , new Vector2(data.x , data.y)));
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
        if(this_player_MoveObject.state != State.ATTACK){
            queue.Enqueue(() => MoveCharector(int.Parse(this_player.name)));
            this_player_MoveObject.Move();
        }
            
        AttackUser();
    }
    void addQueue(Action callback){
        queue.Enqueue(callback);
    }
    ///<summary>
    ///Attack 실행시 호출되는 함수
    ///</summary>
    public void AttackUser(){
        if(Input.GetKeyDown(KeyCode.Space) && !this_player_MoveObject.attackShow.activeSelf){
            StartCoroutine(this_player_MoveObject.Attack());
            Data attackData = new Data("AttackOtherPlayer" , int.Parse(this_player.name) , this_player_MoveObject.moveX , this_player_MoveObject.moveY);
            ws.Send(JsonUtility.ToJson(attackData));
        }
    }
    ///<summary>
    ///서버에서 보내준 플레이어의 공격을 다른 클라이언트에서 보여줄때 사용
    ///</summary>
    public void AttackShow(int id , Vector2 GetAsix){
        MoveObject attackingUser = GameObject.Find(id.ToString()).GetComponent<MoveObject>();
        attackingUser.moveX = GetAsix.x;
        attackingUser.moveY = GetAsix.y;
        StartCoroutine(attackingUser.Attack());
    }
    ///<summary>
    ///서버와 연결시 처음 실행
    ///</summary>
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
    ///<summary>
    ///서버와 다른 플레이어 확인 후 생성
    ///</summary>
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
    ///<summary>
    ///한 유저가 게임을 떠날 시 실행
    ///</summary>
    public void DeleteUser(int deleteIndex){
        Destroy(GameObject.Find(deleteIndex.ToString()).gameObject);
    }
    ///<summary>
    ///캐릭터 이동방향을 서버로 보냄 / 매 프레임마다 호출
    ///</summary>
    public void MoveCharector(int movingPlayer){
        Vector2 move = this_player_MoveObject.GetPosition();
        Data data = new Data("PlayerMove" ,movingPlayer, move.x , move.y , new Vector2(this_player_MoveObject.moveX , this_player_MoveObject.moveY));
        ws.Send(JsonUtility.ToJson(data));
    }
    ///<summary>
    ///다른 캐릭터의 움직임을 받아오는 함수 / 매 프레임마다 호출
    ///</summary>
    public void MoveOtherCharector(Data data){
        for(int i = 0; i < other.Length; i++){
            
            if(other[i].name == data.id.ToString()){
                
                other[i].transform.position = new Vector3(data.x , data.y);
                MoveObject otherMoveObj = other[i].GetComponent<MoveObject>();
                otherMoveObj.SetAnimation(State.MOVE);
                otherMoveObj.moveX = data.moveXY.x;
                otherMoveObj.moveY = data.moveXY.y;
                break;
            }
        }
    }

}
