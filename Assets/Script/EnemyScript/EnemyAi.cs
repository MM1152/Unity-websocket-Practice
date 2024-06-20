using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAi : IMoveObj
{
    [Header("적 인스펙터")]
    public CoinPooling coinPooling;
    public GameObject User;
    public GameObject FindUser;
    public GameObject SearchingUser;
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
        Init();
        coinPooling = GameObject.Find("CoinPooling").GetComponent<CoinPooling>();
        pos = 0.01f;
    }

    public IEnumerator StartMove(Vector2 targetPos)
    {
        if ((Vector2)transform.position != targetPos && state != State.ATTACK)
        {
            
            Vector2 startPos = transform.position;
            state = State.MOVE;
            flipX(targetPos);
            for (float radio = 0f; radio < 1f; radio += pos)
            {
                this.gameObject.transform.position = Vector2.Lerp(startPos, targetPos, radio);
                yield return null;
            }
            state = State.IDLE;
            transform.position = targetPos;
        }
        stateMachine.Transition(new IdleState());
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
    public IEnumerator StartAttack(){
        IsAttack = true;
        ani.SetTrigger("IsAttack");
        
        yield return new WaitForSeconds(1f);
        IsAttack = false;
    }

    public override void Move(){ }

    public override void Move(Vector2 targetPos)
    {
        StartCoroutine(StartMove(targetPos));
    }

    public override void Attack()
    {
        StartCoroutine(StartAttack());
    }
}
