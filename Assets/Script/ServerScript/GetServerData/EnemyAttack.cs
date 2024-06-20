using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>(); 
        enemyAi.stateMachine.Transition(new AttackState());
    }
}
