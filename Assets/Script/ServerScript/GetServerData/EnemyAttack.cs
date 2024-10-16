
using UnityEngine;

public class EnemyAttack : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        bool isSameMap = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>().enemyData.mapName == socket.this_player_MoveObject.UserData.mapName;
        if(enemyCount.Enemys.Count != 0 && isSameMap){
            EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>(); 
            enemyAi.stateMachine.Transition(new AttackState());
            IMoveObj attackingPlayer =  GameObject.Find(data.this_player.id).GetComponent<IMoveObj>();
            attackingPlayer.stateMachine.Transition(new HitState());
            DamagePooling.Instance.ShowObject(attackingPlayer.transform , data.hitDamage);
            attackingPlayer.UserData = data.this_player;
        }
    }
}
