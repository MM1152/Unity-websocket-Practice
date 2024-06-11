using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.Remoting.Metadata;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using System.Threading;
using System;
public class EnemyAi : MonoBehaviour
{
    [Header("적 인스펙터")]
    public GameObject User;
    public GameObject FindUser;
    public GameObject SearchingUser;
    public Animator ani;
    public SpriteRenderer sp;

    [Space(10)]
    [Header("적 스태이터스")]
    public ObjectStatus stat;
    public float speed;
    public float searchUserRadius;
    public float findUserRadious;
    public float moveAway;
    public Animator Enemyani;
    public State state;
    public Vector2 spawnPos;
    public CircleCollider2D FindUserRadious;


    float maxAroundMove;
    public bool returnSpawnPos;
    private Socket socket;
    private Data data;
    private Thread thread;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        socket = Socket.Instance;
        state = new State();
        Enemyani = GetComponent<Animator>();
        spawnPos = transform.position;
        FindUserRadious = this.gameObject.transform.Find("FindUserRadious").GetComponent<CircleCollider2D>();
        FindUserRadious.radius = searchUserRadius;
        maxAroundMove = 3f;
        thread = new Thread(() => InvokeRepeating("SendMessage" , 0.5f , 0.5f));
    }

    public void SendMessage()
    {
        data = new Data("EnemyPos", this.gameObject.name, transform.position.x, transform.position.y , state);
        socket.ws.Send(JsonUtility.ToJson(data));
    }
    private void Update()
    {
        Move();
        if (state == State.FINDENEMY || state == State.MOVE)
        {
            ani.SetBool("IsMove", true);
        }
        if (state == State.IDLE)
        {
            ani.SetBool("IsMove", false);
        }
    }
    private void Move()
    {
        if (Vector2.Distance(this.gameObject.transform.position, spawnPos) > maxAroundMove && state == State.IDLE)
        {
            returnSpawnPos = true;
            state = State.MOVE;
        }
        if (returnSpawnPos)
        {
            
            transform.position += ((Vector3)spawnPos - this.gameObject.transform.position).normalized * Time.deltaTime;
            flipX(spawnPos);
            if (Vector2.Distance(spawnPos, this.gameObject.transform.position) < 0.5f)
            {
                returnSpawnPos = false;
                state = State.IDLE;
            }
        }
        else if (state == State.FINDENEMY)
        {
            if(Vector2.Distance(transform.position , User.transform.position) > 0.5f){
                transform.position += (User.transform.position - this.gameObject.transform.position).normalized * Time.deltaTime;
            }
            
            flipX(User.transform.position);
        }
    }
    public void flipX(Vector3 targetPos){
        if(targetPos.x - this.gameObject.transform.position.x < 0f){
            sp.flipX = true;
        }
        else {
            sp.flipX = false;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!returnSpawnPos)
        {
            if (other.gameObject.tag == "Player")
            {
                if (User == null)
                {
                    User = other.gameObject;
                }
                if (Vector2.Distance(User.gameObject.transform.position, this.gameObject.transform.position) < findUserRadious / 2f)
                {
                    SearchingUser.SetActive(false);
                    FindUser.SetActive(true);
                    state = State.FINDENEMY;
                }
                else
                {
                    FindUser.SetActive(false);
                    SearchingUser.SetActive(true);
                    state = State.IDLE;
                }

            }
        }


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && state != State.FINDENEMY)
        {
            SearchingUser.SetActive(false);
            User = null;
        }
    }
}
