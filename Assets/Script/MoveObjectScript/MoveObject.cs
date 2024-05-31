using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class MoveObject : MonoBehaviour
{
    bool WaitAnimationFinish;

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
    public float pushPower;
    public float moveX;
    public float moveY;
    public bool isAttackSussecs;
    [Space(9)]

    [Header("Character Name")]
    [SerializeField] private string userName;
    


    void Start(){
        state = State.IDLE;
        sp = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        text.text = this.gameObject.name;
        rg = GetComponent<Rigidbody2D>();
    }
    public void Move(){
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        SetAnimation(State.MOVE);
        transform.position += new Vector3(moveX , moveY).normalized * speed * Time.deltaTime;
    }
    public IEnumerator Attack(){
        attackShow.SetActive(true);
        SetAnimation(State.ATTACK);
        
        yield return new WaitForSeconds(.5f);

        attackShow.SetActive(false);
        SetAnimation(State.IDLE);
    } 

    public Vector2 GetPosition(){
        return transform.position;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            isAttackSussecs = true;
            Rigidbody2D otherRigidBody = other.GetComponent<MoveObject>().rg;
            otherRigidBody.AddForce(new Vector2(moveX , moveY) * pushPower , ForceMode2D.Impulse);
            StartCoroutine(StopObject(otherRigidBody));
            attackingPlayer = other;
        }
    }
    IEnumerator StopObject(Rigidbody2D rg){
        yield return new WaitForSeconds(0.3f);
        rg.velocity = Vector2.zero;
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
    MOVE
}
