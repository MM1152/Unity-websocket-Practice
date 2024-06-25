
using UnityEngine;

public class CheckThisMapEnemy : ISocket
{
    
    public override void RunNetworkCode(Data data)
    {
        int count = 0;
        Debug.Log("checkEnemy : " + data.enemyList.Length);
        for(int i = 0; i < enemyCount.Enemys.Count; i++){
            
            if(data.enemyList.Length != 0 && data.enemyList[count].id.ToString() == enemyCount.Enemys[i].name.Split(' ')[1]){
                enemyCount.Enemys[i].SetActive(true);
                count++;
            }
            else {
                enemyCount.Enemys[i].SetActive(false);
            }
        }
    }
}