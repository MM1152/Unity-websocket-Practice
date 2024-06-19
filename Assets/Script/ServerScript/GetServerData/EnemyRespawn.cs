using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        GameObject enemy = enemyCount.Enemys[data.enemy.id];
        EnemyAi enemyAi = enemy.GetComponent<EnemyAi>();

        enemy.transform.position = new Vector3(data.enemy.x , data.enemy.y);
        enemyAi.Hpbar.value = data.enemy.Hp;
        enemy.SetActive(true);
    }
}


