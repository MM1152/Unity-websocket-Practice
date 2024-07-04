
using Unity.VisualScripting;
using UnityEngine;

public class CheckThisMapEnemy : ISocket
{
    
    public override void RunNetworkCode(Data data)
    {
        int count = 0;
        if(data.NPC.Length != 0){
            Debug.Log(data.NPC[0].id);
        }
        

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
        for(int i = data.enemyList[0].id ; i < data.enemyList[0].id + data.enemyList.Length; i++){
            enemyCount.Enemys[i].SetActive(true);
        }

        foreach(var other in socket.other){
            other.SetActive(false);
        }
        for(int i = 0; i < socket.other.Length; i++){
            for(int j = 0; j < data.users.Length; j++){
                if(socket.other[i].name == data.users[j].id){
                    socket.other[i].gameObject.SetActive(true);
                }
            }
        }
    }
}