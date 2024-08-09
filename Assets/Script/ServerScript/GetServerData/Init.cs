
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Init : ISocket
{
    [SerializeField]
    private GameObject[] enemys;
    [SerializeField]
    private GameObject[] npcs;

    
    //private EnemyCount enemyCount;
    public override void RunNetworkCode(Data data)
    {
        Debug.Log(data.this_player.type);
        foreach(var enemy in data.enemyList){
            
            GameObject SpawnEnemy = Instantiate(enemys[enemy.type - 1] , enemyCount.transform) as GameObject;
            SpawnEnemy.name = "Enemy " + enemy.id.ToString();
            SpawnEnemy.transform.position = new Vector3(enemy.x , enemy.y , -2f);
            SpawnEnemy.GetComponent<EnemyAi>().Hpbar.maxValue = enemy.MaxHp;
            SpawnEnemy.GetComponent<EnemyAi>().Hpbar.value = enemy.Hp;
            SpawnEnemy.GetComponent<EnemyAi>().enemyData = enemy;
            if(enemy.state == "Die"){
                SpawnEnemy.SetActive(false);
            }
        }
        foreach(var NPC in data.NPC[0].NPCList){
            GameObject spawnNPC = Instantiate(npcs[NPC.type - 1] , NPCcount.transform);
            NPCcount.npc_List.Add(NPC.id , spawnNPC);
            spawnNPC.name = NPC.id.ToString();
            spawnNPC.SetActive(true);
        }
        
        GameObject player = Instantiate(socket.user);
        player.name = data.id;
        player.AddComponent<MoveObject>().Type = data.this_player.type;

        socket.this_player = player;
        socket.this_player_MoveObject = socket.this_player.GetComponent<MoveObject>();
        
        socket.this_player_MoveObject.setUserData(data.this_player);
        socket.this_player_MoveObject.setUserExp(data.this_player);
        data = new Data("Connection");
        
        enemyCount.setEnemys();
        socket.ws.Send(JsonUtility.ToJson(data));

    }
}