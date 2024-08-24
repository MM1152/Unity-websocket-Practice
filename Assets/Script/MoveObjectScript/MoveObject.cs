using System;
using System.Collections;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MoveObject : IMoveObj
{
    private EnemyCount enemyCount;
    private GameManager gameManager;
    private SetStatsUI stat;
    public GameObject UI;
    private GameObject PosionSlot;
    private ExpBarUI exp;
    private GameObject levelUI;
    private GameObject attackShow;
    private SpriteRenderer playerHand; // 이동방향마다 같이 flipX 해줘야됌
    public Text text;
    [SerializeField]
    public string playerMap
    {
        get
        {
            return UserData.mapName;
        }
        set
        {
            UserData.mapName = value;
        }
    }
    public float speed;
    public float curretmoveX;
    public float curretmoveY;
    public float moveX;
    public float moveY;
    public bool firstSendMoveData;
    public float attackTime;
    public float attackCoolTime;
    public void setUserData(UserData userData)
    {
        this.UserData = userData;
    }
    public UserData getUserData()
    {
        return UserData;
    }
    public void setUserExp(UserData data)
    {
        if (UserData.Level != data.Level)
        {
            stat.SetStatsPoint(data);
        }
        UserData = data;
        exp.setMaxExp(UserData.maxExp);
        exp.setcurrentExp(UserData.exp);
    }
    private void Update()
    {
        stateMachine.Update();
        if(attackTime > 0) {
            attackTime -= Time.deltaTime;
        }
        /*if(SetPosionNumberUI.autoRecovery[1] && gameManager.MPrecoveryValue >= (UserData.mp){
            UsePostion(5);
        }*/
    }
    public void Awake()
    {
        enemyCount = GameObject.Find("EnemyCount").GetComponent<EnemyCount>();
        UI = gameObject.transform.Find("InventoryANDstatus").gameObject;
        text = gameObject.transform.Find("Canvas").Find("Name").GetComponent<Text>();
        attackShow = gameObject.transform.Find("Attack").gameObject;
        playerHand = gameObject.transform.Find("Hand").GetComponent<SpriteRenderer>();
        exp = gameObject.transform.Find("Canvas").Find("ExpBar").GetComponent<ExpBarUI>();
        stat = gameObject.transform.Find("InventoryANDstatus").Find("Status").GetComponent<SetStatsUI>();
        PosionSlot = UI.transform.Find("PositionSlot").gameObject;
        levelUI = UI.transform.Find("Level").gameObject;

        PosionSlot.SetActive(true);
        exp.gameObject.SetActive(true);
        levelUI.SetActive(true);

        attackCoolTime = 2f;
        speed = 3f;
        Init();
    }
    void Start()
    {
        text.text = UserData.id;
        levelUI.GetComponent<Text>().text = "Level " + UserData.Level;
        UI.SetActive(true);
        gameManager = GameManager.Instance;

    }
    public override void Move()
    {
        curretmoveX = Input.GetAxisRaw("Horizontal");
        curretmoveY = Input.GetAxisRaw("Vertical");
        if(curretmoveX != 0) moveX = curretmoveX;
        FlipX();
        transform.position += new Vector3(curretmoveX, curretmoveY).normalized * speed * Time.deltaTime;
        if (!firstSendMoveData)
        {
            firstSendMoveData = true;
            InvokeRepeating("SendMessage", 0, 0.5f);
        }
    }
    public void SendMessage()
    {
        Data data = new Data("PlayerMove", this.gameObject.name, transform.position.x, transform.position.y, new Vector2(curretmoveX, curretmoveY));
        socket.ws.Send(JsonUtility.ToJson(data));
    }
    public override void Attack()
    {
        IsAttack = true;
        range.target = FindNearEnemy().transform;
        Debug.Log(FindNearEnemy().name);
        range.CheckAttackPossible();
        StartCoroutine(AttackShow());
        Socket.Instance.ws.Send(JsonUtility.ToJson(new Data("AttackOtherPlayer", gameObject.name)));
    }
    public IEnumerator AttackShow()
    {    
        attackShow.SetActive(true);
        yield return new WaitUntil(() => attackTime <= 0);
        attackTime = attackCoolTime;
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
            data.this_player = UserData;
            socket.ws.Send(JsonUtility.ToJson(data));
        }
    }
    ///<summary>
    /// 애니메이션을 STATE로 SET해주는 함수
    ///</summary>
    public void FlipX()
    {
        if (curretmoveX < 0)
        {
            sp.flipX = true;
            playerHand.flipX = true;
        }
        else if (curretmoveX > 0)
        {
            sp.flipX = false;
            playerHand.flipX = false;
        }
    }
    
    public GameObject FindNearEnemy(){

            int index = 0;
            float min = 0;
            float cnt = 9999999;
            for (int i = 0; i < enemyCount.Enemys.Count; i++)
            {
                if (enemyCount.Enemys[i].activeSelf)
                {
                    Vector2 enemyPos = enemyCount.Enemys[i].transform.position;
                    Vector2 userPos = transform.position;

                    min = Vector2.Distance(enemyPos, userPos);

                    if (cnt > min)
                    {
                        cnt = min;
                        index = i;
                    }
                }
            }
            GameObject nearEnemy = enemyCount.Enemys[index].gameObject;
            return nearEnemy;
    }
    public override void Move(Vector2 targetPos) { }

}
