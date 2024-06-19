using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : ISocket
{
    [SerializeField] EnemyCount enemyCount;
    public override void RunNetworkCode(Data data)
    {
        Debug.Log("Enemy " +data.id + " Attack Start");
        EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>(); 
        StartCoroutine(enemyAi.Attack()); 
    }
}
