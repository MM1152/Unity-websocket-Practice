using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMoveObj : MonoBehaviour {
    

    public Animator ani;
    public Range range;
    public SpriteRenderer sp;
    public StateMachine stateMachine;
    protected Socket socket;
    public bool IsAttack;
    public Rigidbody2D rg;
    private int type;
    public int Type {
        set {
            type = value;
            if(sp != null) sp.sprite = socket.playerSprite[type - 1];
            if(ani != null) ani.runtimeAnimatorController = socket.playerAnimator[type - 1];
            

            Debug.Log("Setting Type : " + value);
            if (value == 2) {
                this.gameObject.AddComponent<Range>();
                range = GetComponent<Range>();
            } 
        }
    }
    
    [SerializeField] private UserData userData;
    public UserData UserData
    {
        get
        {
            return userData;
        }
        set
        {
            userData = value;
        }
    }

    public void Init(){

        ani = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        socket = Socket.Instance;
        stateMachine = new StateMachine(this);
        stateMachine.Init(new IdleState());
        IsAttack = false;
        rg = GetComponent<Rigidbody2D>();
    }
    public void SetUserData(UserData userData){

    }
    public abstract void Move();
    public abstract void Move(Vector2 targetPos);
    public abstract void Attack();
    
}