using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerMove : IMoveObj
{
    private float radio;
    public Text text;

    SpriteRenderer playerHand;
    public GameObject attackShow;

    private GameObject offObject;
    private void Awake()
    {
        Init();
        gameObject.transform.Find("InteractionCanvas").gameObject.SetActive(false);
        attackShow = gameObject.transform.Find("Attack").gameObject;
        radio = 0.02f;
        text = gameObject.transform.Find("BackGroundCanvas").Find("Name").GetComponent<Text>();
        playerHand = gameObject.transform.Find("Hand").GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        text.text = UserData.id;
    }
    public override void Move(Vector2 targetPos)
    {
        StartCoroutine(Moving(targetPos));
    }
    public IEnumerator Moving(Vector2 targetPos)
    {
        FlipX(targetPos);
        if (targetPos != (Vector2)transform.position)
        {
            Vector2 startPos = transform.position;
            for (float i = 0f; i <= 1.0f; i += radio)
            {
                transform.position = Vector2.Lerp(startPos, targetPos, i);
                yield return null;
            }
            
            transform.position = targetPos;
        }
        if(!IsAttack){
            stateMachine.Transition(new IdleState());
        }
        
    }
    public override void Attack()
    {
        StartCoroutine(AttackShow());
    }

    public IEnumerator AttackShow()
    {
        IsAttack = true;
        attackShow.SetActive(true);
        yield return new WaitForSeconds(.5f);
        IsAttack = false;
        attackShow.SetActive(false);
    }
    public void FlipX(Vector2 targetPos)
    {
        if (targetPos.x - this.gameObject.transform.position.x < 0)
        {
            sp.flipX = true;
            playerHand.flipX = true;
        }
        else if (targetPos.x - this.gameObject.transform.position.x > 0)
        {
            sp.flipX = false;
            playerHand.flipX = false;
        }
    }
    public override void Move() { }
}