using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.Remoting.Metadata;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using System.Threading;
using System;
using System.Security.Cryptography;
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
    public float pos;

    float maxAroundMove;
    public bool returnSpawnPos;
    private Socket socket;
    private Data data;
    private Thread thread;

    // Start is called before the first frame update
    void Start()
    {
        pos = 0.01f;
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

    private void Update()
    {
        if (state == State.FINDENEMY || state == State.MOVE)
        {
            ani.SetBool("IsMove", true);
        }
        if (state == State.IDLE)
        {
            ani.SetBool("IsMove", false);
        }
    }
    public IEnumerator Move(Vector2 targetPos)
    {
        Vector2 startPos = transform.position;
        flipX((Vector3)targetPos);
        state = State.MOVE;
        for(float radio = 0f; radio < 1f; radio += pos){
            this.gameObject.transform.position = Vector2.Lerp(startPos , targetPos , radio);
            yield return null;
        }
        state = State.IDLE;
        transform.position = targetPos;
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
