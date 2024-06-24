using UnityEngine;

public abstract class IMoveObj : MonoBehaviour {
    
    public Animator ani;
    public SpriteRenderer sp;
    public StateMachine stateMachine;
    protected Socket socket;
    public bool IsAttack;
    public Rigidbody2D rg;

    public void Init(){
        ani = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        socket = Socket.Instance;
        stateMachine = new StateMachine(this);
        stateMachine.Init(new IdleState());
        IsAttack = false;
        rg = GetComponent<Rigidbody2D>();
    }
    
    public abstract void Move();
    public abstract void Move(Vector2 targetPos);
    public abstract void Attack();
}