using UnityEngine;

public class PlayerAutoBattle : MonoBehaviour
{
    private MoveObject player;
    private Istate state;
    [SerializeField] private GameObject target;
    private void Start()
    {
        player = GetComponent<MoveObject>();
        state = new IdleState();
    }

    private void FixedUpdate()
    {
        target = player.FindNearEnemy();
        AutoPlay();
    }
    private void AutoPlay(){
        if (AutoBattle.autoBattle)
        {
            if (target != null)
            {
               
                if (player.getUserData().attackRadious < Vector2.Distance(target.transform.position, transform.position) && !player.IsAttack)
                {
                    transform.position += (target.transform.position - transform.position).normalized * 3f * Time.deltaTime;
                    player.FlipX(target.transform.position.x - transform.position.x);
                    player.stateMachine.Transition(new MoveState());
                }
                else if(player.getUserData().attackRadious >= Vector2.Distance(target.transform.position, transform.position) && player.attackTime <= 0 && !player.IsAttack)
                {
                    if(Socket.Instance.this_player_MoveObject.getUserData().type == 1) Socket.Instance.this_player_MoveObject.HitEnemy(target.gameObject , Socket.Instance.this_player_MoveObject.attack , Socket.Instance.this_player_MoveObject.getUserData());
                    player.stateMachine.Transition(new AttackState());
                }
            }

        }
    }
}