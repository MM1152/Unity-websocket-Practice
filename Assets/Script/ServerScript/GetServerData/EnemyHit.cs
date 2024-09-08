using UnityEngine;

public class EnemyHit : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponentInChildren<EnemyAi>();
        DamagePooling.Instance.ShowObject(enemyAi.transform ,  data.hitDamage);
        enemyAi.Hpbar.value = data.enemy.Hp;
        enemyAi.stateMachine.Transition(new HitState());
    }
}