using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class MoveObject : IMoveObj
{
    private SetStatsUI stat;
    public GameObject UI;
    
    private ExpBarUI exp;
    private GameObject attackShow;
    private SpriteRenderer playerHand; // 이동방향마다 같이 flipX 해줘야됌
    public Text text;
    [SerializeField]
    private UserData this_player_info ;
    public float speed;
    public float radio;
    public float pushPower;
    public float moveX;
    public float moveY;
    public bool isAttackSussecs;
    public bool firstSendMoveData;

    public void setUserData(UserData userData){
        this.this_player_info = userData;
    }
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
    private void Update() {
        stateMachine?.Update();
    }
    public void Awake()
    {   
        UI = gameObject.transform.Find("InventoryANDstatus").gameObject;
        text = gameObject.transform.Find("Canvas").Find("Name").GetComponent<Text>();
        attackShow = gameObject.transform.Find("Attack").gameObject;
        playerHand = gameObject.transform.Find("Hand").GetComponent<SpriteRenderer>();
        exp = gameObject.transform.Find("Canvas").Find("ExpBar").GetComponent<ExpBarUI>();
        exp.gameObject.SetActive(true);
        stat = gameObject.transform.Find("InventoryANDstatus").Find("Status").GetComponent<SetStatsUI>();
        speed = 3f;
        Init(); 
    }
    void Start(){   
        text.text = this_player_info.id;
        UI.SetActive(true);
    }
    public override void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        FlipX();
        transform.position += new Vector3(moveX, moveY).normalized * speed * Time.deltaTime;
        if (!firstSendMoveData)
        {
            firstSendMoveData = true;
            InvokeRepeating("SendMessage", 0, 0.5f);
        }
    }

    public void SendMessage()
    {
        Data data = new Data("PlayerMove", this.gameObject.name, transform.position.x, transform.position.y, new Vector2(moveX, moveY));
        socket.ws.Send(JsonUtility.ToJson(data));
    }
    public override void Attack()
    {
        StartCoroutine(AttackShow());
        Socket.Instance.ws.Send(JsonUtility.ToJson(new Data("AttackOtherPlayer" , gameObject.name)));
    }

    public IEnumerator AttackShow()
    {
        IsAttack = true;
        attackShow.SetActive(true);
        yield return new WaitForSeconds(.5f);
        IsAttack = false;
        attackShow.SetActive(false);
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
    public void FlipX()
    {
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
    public override void Move(Vector2 targetPos){ }
}
