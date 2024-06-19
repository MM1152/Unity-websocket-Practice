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
using UnityEngine.UI;
using System.Security.Permissions;
public class EnemyAi : MonoBehaviour
{
    [Header("적 인스펙터")]
    public CoinPooling coinPooling;
    public GameObject User;
    public GameObject FindUser;
    public GameObject SearchingUser;
    public Animator ani;
    public SpriteRenderer sp;
    public Slider Hpbar;

    [Space(10)]
    [Header("적 스태이터스")]
    public ObjectStatus stat;
    public float speed;
    public float searchUserRadius;
    public float findUserRadious;
    public float moveAway;
    public State state;
    public Vector2 spawnPos;
    public CircleCollider2D FindUserRadious;
    public float pos;
    public bool isDie;

    public bool returnSpawnPos;
    public Vector2 target;
    // Start is called before the first frame update
    void Start()
    {
        coinPooling = GameObject.Find("CoinPooling").GetComponent<CoinPooling>();
        pos = 0.01f;
        ani = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        state = new State();
        spawnPos = transform.position;
        FindUserRadious = this.gameObject.transform.Find("FindUserRadious").GetComponent<CircleCollider2D>();
        FindUserRadious.radius = searchUserRadius;
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

        if ((Vector2)transform.position != targetPos && state != State.ATTACK)
        {
            flipX(targetPos);
            Vector2 startPos = transform.position;
            state = State.MOVE;
            for (float radio = 0f; radio < 1f; radio += pos)
            {
                this.gameObject.transform.position = Vector2.Lerp(startPos, targetPos, radio);
                yield return null;
            }
            state = State.IDLE;
            transform.position = targetPos;
        }

    }
    public void Hit(){
        if(state != State.ATTACK){
            ani.SetTrigger("IsHit");
        }
    }

    public IEnumerator Die(UserData user){
        ani.SetBool("IsDie" , true);

        MoveObject userMoveObject;    
        userMoveObject = GameObject.Find(user.id).GetComponent<MoveObject>();
        userMoveObject.setUserExp(user);
        
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("EnemyDie") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        coinPooling.MakeCoin(this.gameObject.transform , userMoveObject.transform);
        this.gameObject.SetActive(false);
    }
    public void flipX(Vector3 targetPos)
    {
        if (targetPos.x - this.gameObject.transform.position.x < 0f)
        {
            sp.flipX = true;
        }
        else
        {
            sp.flipX = false;
        }
    }
    public IEnumerator Attack(){
        Debug.Log("Start Attack");
        state = State.ATTACK;
        ani.SetTrigger("IsAttack");
        
        yield return new WaitForSeconds(1f);
        Debug.Log("Finish");
        state = State.IDLE;
    }
}
