using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class MoveObject : MonoBehaviour
{

    Socket socket;
    public ObjectStatus stat;
    [Header("Component")]
    public Rigidbody2D rg;
    public Animator ani;
    public Collider2D attackingPlayer;
    public GameObject attackShow;
    public SpriteRenderer playerHand; // 이동방향마다 같이 flipX 해줘야됌
    public SpriteRenderer sp;
    public Text text;
    [Space(9)]
    [Header("Charector Status")]
    public State state;
    public float speed;
    public float radio;
    public float pushPower;
    public float moveX;
    public float moveY;
    public bool isAttackSussecs;
    public bool firstSendMoveData;
    [Space(9)]

    [Header("Character Name")]
    [SerializeField] private string userName;
    
    


    void Update(){
        Move();
        Attack();
    }
    void Start(){
        radio = 0.02f;
        state = State.IDLE;
        socket = Socket.Instance;
        sp = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        text.text = this.gameObject.name;
        rg = GetComponent<Rigidbody2D>();
    }
    public void Move(){
        if(this.gameObject == socket.this_player && state != State.ATTACK){
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
            SetAnimation(State.MOVE);
            transform.position += new Vector3(moveX , moveY).normalized * speed * Time.deltaTime;

            if(!firstSendMoveData){
                firstSendMoveData = true;
                InvokeRepeating("SendMessage" , 0 , 0.5f);     
            }
        }
    }
 
    private void SendMessage(){
        
        Data data = new Data("PlayerMove" , this.gameObject.name , transform.position.x , transform.position.y , new Vector2(moveX , moveY));
        socket.ws.Send(JsonUtility.ToJson(data));
    }
    public void Attack(){
        if(Input.GetKeyDown(KeyCode.Space) && this.gameObject == socket.this_player && !socket.this_player_MoveObject.attackShow.activeSelf){
            StartCoroutine(AttackShow());
            Data attackData = new Data("AttackOtherPlayer" , this.gameObject.name);
            socket.ws.Send(JsonUtility.ToJson(attackData));
        }
        
    }
    public IEnumerator Moving(Vector2 targetPos)
    {
        Debug.Log("StartCorutain");
        Vector2 startPos = transform.position;
        for (float i = 0f; i <= 1.0f; i += radio)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, i);
            yield return null;
        }
        transform.position = targetPos;
        moveX = 0;
        moveY = 0;
        SetAnimation(State.IDLE);
        Debug.Log("EndCorutain");
    }
    public IEnumerator AttackShow(){
        SetAnimation(State.ATTACK);
        attackShow.SetActive(true);
        yield return new WaitForSeconds(.5f);
        attackShow.SetActive(false);
        SetAnimation(State.IDLE);
    } 

    public Vector2 GetPosition(){
        return transform.position;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Debug.Log("AttackEnemy");
            //other.GetComponent<Animator>().SetTrigger("IsHit");
            //other.GetComponent<EnemyAi>().state = State.HURT;
            socket.ws.Send(JsonUtility.ToJson(new Data("HitEnemy" , other.name.Split(' ')[1])));
        }
    }

    ///<summary>
    /// 애니메이션을 STATE로 SET해주는 함수
    ///</summary>
    public void SetAnimation(State state){
        this.state = state;
        
        if(state == State.ATTACK){
            ani.SetTrigger("IsAttack");
        }
        if(state == State.MOVE || state == State.IDLE){
            ani.SetInteger("IsRun" , Math.Abs((int)moveX) + Math.Abs((int)moveY));
        }

        if(moveX < 0){
            sp.flipX = true;
            playerHand.flipX = true;
        }else if(moveX > 0){
            sp.flipX = false;
            playerHand.flipX = false;
        }
    }
}
public enum State{
    IDLE,
    ATTACK,
    MOVE,
    FINDENEMY,
    HURT

}
