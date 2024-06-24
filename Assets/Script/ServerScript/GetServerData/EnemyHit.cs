using UnityEngine;

public class EnemyHit : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        Debug.Log(data.enemy.id);
        EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponentInChildren<EnemyAi>();
        enemyAi.Hpbar.value = data.enemy.Hp;
        enemyAi.stateMachine.Transition(new HitState());
    }
}