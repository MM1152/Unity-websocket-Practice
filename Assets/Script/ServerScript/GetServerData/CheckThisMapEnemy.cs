
using Unity.VisualScripting;
using UnityEngine;

public class CheckThisMapEnemy : ISocket
{
    
    public override void RunNetworkCode(Data data)
    {
        foreach(GameObject npc in NPCcount.npc_List.Values){
            npc.SetActive(false);
        }

        foreach(var NPC in data.NPC){
            GameObject this_npc = NPCcount.npc_List[NPC.id];
            this_npc.transform.localPosition = NPC.spawnPos;
            this_npc.GetComponent<NpcAi>().NpcData = NPC;
            this_npc.SetActive(true);
        }

        for(int i = 0; i < enemyCount.Enemys.Count; i++){
            enemyCount.Enemys[i].SetActive(false);
        }
        
        foreach(var enemy in data.enemyList){
            enemyCount.Enemys[enemy.id].SetActive(true);
        }

        foreach(var other in socket.other){
            if(other != socket.this_player) other.SetActive(false);
        }

        foreach(var other in socket.other) {
            foreach(var other1 in data.users){
                if(other1.id == other.name) {
                    other.SetActive(true);
                    break;
                }
            }
        }
    }
}