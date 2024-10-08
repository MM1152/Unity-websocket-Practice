using System;
using UnityEngine;

public class EnemyDie : ISocket
{
    [SerializeField]Quest quest;
    public override void RunNetworkCode(Data data)
    {
        quest ??= GameObject.FindObjectOfType<Quest>();
        Debug.Log($"DropItem {data.dropItem}");
        
        EnemyAi enemy = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>();   
        if(quest.QuestData.enemyType != 0 && quest.QuestData.enemyType == enemy.enemyData.type) quest.QuestData.count++;
        Transform killUserPos = GameObject.Find(data.this_player.id).transform;
        if(enemy.gameObject.activeSelf){
            enemy.Hpbar.value = data.enemy.Hp;
            //DamagePooling.Instance.ShowObject(enemy.transform.position , data.this_player.attack);
            StartCoroutine(enemy.Die(data));
        }
        if(socket.this_player.name == data.this_player.id){
            ItemPooling.Instance?.ShowObject(enemy.transform , killUserPos , data.dropItem);
            CoinPooling.Instance?.ShowObject(enemy.transform , killUserPos , 0);
        }
    

    }
}