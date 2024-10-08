
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMapData {
    private static Dictionary<int , string> EnemyMap = new Dictionary<int , string>();
    public void SetEnemyMapData(int key , string value){
        if(!EnemyMap.ContainsKey(key)) {
            EnemyMap.Add(key, value);
        }
    }
    public string GetEnemyMapData(int value) {
        return EnemyMap[value];
    }
}
public class Init : ISocket
{
    private EnemyMapData enemyMapData = new EnemyMapData();
    [SerializeField]
    private GameObject[] enemys;
    [SerializeField]
    private GameObject[] npcs;

    //private EnemyCount enemyCount;
    public override void RunNetworkCode(Data data)
    {
        foreach(var enemy in data.enemyList){
            GameObject SpawnEnemy = Instantiate(enemys[enemy.type - 1] , enemyCount.transform) as GameObject;
            SpawnEnemy.name = "Enemy " + enemy.id.ToString();
            enemyMapData.SetEnemyMapData(enemy.type , enemy.mapName);
            SpawnEnemy.transform.position = new Vector3(enemy.x , enemy.y , -2f);
            SpawnEnemy.GetComponent<EnemyAi>().Hpbar.maxValue = enemy.MaxHp;
            SpawnEnemy.GetComponent<EnemyAi>().Hpbar.value = enemy.Hp;
            SpawnEnemy.GetComponent<EnemyAi>().enemyData = enemy;
            SpawnEnemy.SetActive(false);
        }
        foreach(var NPC in data.NPC[0].NPCList){

            Debug.Log(NPC.id);
            GameObject spawnNPC = Instantiate(npcs[NPC.npc_image - 1] , NPCcount.transform); 
            NPCcount.npc_List.Add(NPC.id , spawnNPC);

            if(NPC.npc_type == 1) spawnNPC.AddComponent<MakeShopItem>(); // NPC TYPE (1 : 상점 , 2 : 퀘스트)
            else if(NPC.npc_type == 2) spawnNPC.AddComponent<QuestNPC>(); 

            spawnNPC.name = NPC.id.ToString();
            spawnNPC.SetActive(true); 
            Debug.Log("Spawn NPC Set Active (true)");
        }
        
        GameObject player = Instantiate(socket.user);
        player.transform.SetSiblingIndex(0);
        player.AddComponent<PlayerAutoBattle>();
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