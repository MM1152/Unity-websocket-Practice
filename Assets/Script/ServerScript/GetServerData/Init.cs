
using System;
using UnityEngine;

public class Init : ISocket
{
    [SerializeField]
    private GameObject[] enemys;
    [SerializeField]
    //private EnemyCount enemyCount;
    private void Start() {
        enemyCount ??= GameObject.Find("EnemyCount").GetComponent<EnemyCount>();    
    }
    public override void RunNetworkCode(Data data)
    {
        foreach(var enemy in data.enemyList){

            GameObject SpawnEnemy = Instantiate(enemys[enemy.type - 1] , enemyCount.transform) as GameObject;
            SpawnEnemy.name = "Enemy " + enemy.id.ToString();
            SpawnEnemy.transform.position = new Vector3(enemy.x , enemy.y , -2f);
            SpawnEnemy.GetComponent<EnemyAi>().Hpbar.maxValue = enemy.MaxHp;
            SpawnEnemy.GetComponent<EnemyAi>().Hpbar.value = enemy.Hp;
            if(enemy.state == "Die"){
                SpawnEnemy.SetActive(false);
            }
        }

        GameObject player = Instantiate(socket.user);
        
        socket.this_player = player;
        socket.this_player_MoveObject = socket.this_player.GetComponent<MoveObject>();
        player.name = data.id;
        socket.this_player_MoveObject.setUserExp(data.this_player);
        socket.this_player_MoveObject.stat.SetStats(data.this_player);
        socket.this_player_MoveObject.stat.SetStatsPoint(data.this_player);
        data = new Data("Connection");
        
        enemyCount.setEnemys();
        socket.ws.Send(JsonUtility.ToJson(data));

    }
}