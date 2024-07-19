using UnityEngine;

public class EnemyHit : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponentInChildren<EnemyAi>();
        DamagePooling.ShowDamage(enemyAi.transform.position ,  data.this_player.attack);
        enemyAi.Hpbar.value = data.enemy.Hp;
        enemyAi.stateMachine.Transition(new HitState());
    }
}