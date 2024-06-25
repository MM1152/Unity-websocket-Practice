
using Unity.VisualScripting;
using UnityEngine;

public class CheckThisMapEnemy : ISocket
{
    
    public override void RunNetworkCode(Data data)
    {
        int count = 0;
        for(int i = 0; i < enemyCount.Enemys.Count; i++){
            
            if(data.enemyList.Length != 0 && data.enemyList[count].id.ToString() == enemyCount.Enemys[i].name.Split(' ')[1]){
                enemyCount.Enemys[i].SetActive(true);
                count++;
            }
            else {
                enemyCount.Enemys[i].SetActive(false);
            }
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