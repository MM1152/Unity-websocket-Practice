using UnityEngine;

public class EnemyDie : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        Debug.Log($"EnemyDie {data.id}");

        EnemyAi enemy = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>();   
        Transform killUserPos = GameObject.Find(data.this_player.id).transform;
        if(enemy.gameObject.activeSelf){
             StartCoroutine(enemy.Die(data.this_player));
        }
    

    }
}