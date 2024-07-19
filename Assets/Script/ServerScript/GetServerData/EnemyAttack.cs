
using UnityEngine;

public class EnemyAttack : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        if(enemyCount.Enemys.Count != 0){
            EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>(); 
            enemyAi.stateMachine.Transition(new AttackState());
            IMoveObj attackingPlayer =  GameObject.Find(data.this_player.id).GetComponent<IMoveObj>();
            attackingPlayer.stateMachine.Transition(new HitState());
            DamagePooling.ShowDamage(attackingPlayer.transform.position , attackingPlayer.UserData.hp - data.this_player.hp);
            attackingPlayer.UserData = data.this_player;
        }
    }
}
