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
    public StateMachine stateMachine;
    Socket socket;
    public SetStatsUI stat;
    [Header("Component")]
    public ExpBarUI exp;
    public Rigidbody2D rg;
    public Animator ani;
    public Collider2D attackingPlayer;
    public GameObject attackShow;
    public SpriteRenderer playerHand; // 이동방향마다 같이 flipX 해줘야됌
    public SpriteRenderer sp;
    public Text text;
    [Space(9)]
    [Header("Charector Status")]
    [SerializeField]
    private UserData this_player_info;
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


    public UserData getUserData()
    {
        return this_player_info;
    }
    public void setUserExp(UserData data)
    {
        if (this_player_info.Level != data.Level)
        {
            stat.SetStatsPoint(data);
        }
        this_player_info = data;
        exp.setMaxExp(this_player_info.maxExp);
        exp.setcurrentExp(this_player_info.exp);

    }
    void Update()
    {
        //Move();
        //Attack();
        stateMachine.Update();
    }
    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        text.text = this.gameObject.name;
        rg = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine(this);
        stateMachine.Init(new IdleState());
    }
    void Start()
    {
        //exp.gameObject.SetActive(false);
        //radio = 0.02f;
        //state = State.IDLE;
        //socket = Socket.Instance;

        /* if (socket.this_player == this.gameObject)
         {
             exp.gameObject.SetActive(true);
         }
         */
    }
    public void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(moveX, moveY).normalized * speed * Time.deltaTime;
        SetAnimation(State.MOVE);
        if (!firstSendMoveData)
        {
            firstSendMoveData = true;
            //InvokeRepeating("SendMessage", 0, 0.5f);
        }
    }

    public void SendMessage()
    {
        Data data = new Data("PlayerMove", this.gameObject.name, transform.position.x, transform.position.y, new Vector2(moveX, moveY));
        socket.ws.Send(JsonUtility.ToJson(data));
    }
    public void Attack()
    {
        StartCoroutine(AttackShow());
        Data attackData = new Data("AttackOtherPlayer", this.gameObject.name);
        socket.ws.Send(JsonUtility.ToJson(attackData));
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
    public IEnumerator AttackShow()
    {
        SetAnimation(State.ATTACK);
        attackShow.SetActive(true);
        yield return new WaitForSeconds(.5f);
        attackShow.SetActive(false);
        SetAnimation(State.IDLE);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //other.GetComponent<Animator>().SetTrigger("IsHit");
            //other.GetComponent<EnemyAi>().state = State.HURT;
            Data data = new Data("HitEnemy");
            data.id = other.name.Split(' ')[1];
            data.this_player = this_player_info;
            socket.ws.Send(JsonUtility.ToJson(data));


        }
    }



    ///<summary>
    /// 애니메이션을 STATE로 SET해주는 함수
    ///</summary>
    public void SetAnimation(State state)
    {
        this.state = state;


        if (state == State.MOVE || state == State.IDLE)
        {
            ani.SetInteger("IsRun", Math.Abs((int)moveX) + Math.Abs((int)moveY));
        }

        if (moveX < 0)
        {
            sp.flipX = true;
            playerHand.flipX = true;
        }
        else if (moveX > 0)
        {
            sp.flipX = false;
            playerHand.flipX = false;
        }
    }
}
public enum State
{
    IDLE,
    ATTACK,
    MOVE,
    FINDENEMY,
    HURT

}
