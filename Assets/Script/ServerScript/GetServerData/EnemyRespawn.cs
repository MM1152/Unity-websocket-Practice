using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRespawn : ISocket
{
    public Text mapName;
    public override void RunNetworkCode(Data data)
    {
        
        GameObject enemy = enemyCount.Enemys[data.enemy.id];
        EnemyAi enemyAi = enemy.GetComponent<EnemyAi>();

        enemy.transform.position = new Vector3(data.enemy.x , data.enemy.y);
        enemyAi.stateMachine.Transition(new IdleState());
        enemyAi.Hpbar.value = data.enemy.Hp;
        if(data.enemy.mapName != mapName.text){
            enemy.SetActive(false);
        }else {
            enemy.SetActive(true);
        }
        
    }
}


