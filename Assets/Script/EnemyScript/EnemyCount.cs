using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    public List<GameObject> Enemys;
    public void setEnemys(){
        for(int i = 0; i < this.gameObject.transform.childCount; i++){
            Enemys.Add(gameObject.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < Enemys.Count; i++){
            if(!ItemPooling.Instance.dropItemList.ContainsKey(Enemys[i].GetComponent<EnemyAi>().enemyData.mapName)){
                ItemPooling.Instance.dropItemList.Add(Enemys[i].GetComponent<EnemyAi>().enemyData.mapName , new List<int>());
                ItemPooling.Instance.dropItemList[Enemys[i].GetComponent<EnemyAi>().enemyData.mapName] = Enemys[i].GetComponent<EnemyAi>().enemyData.dropItemList;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemys.Count != gameObject.transform.childCount){
            for(int i = 0; i < this.gameObject.transform.childCount; i++){
                if(!Enemys.Contains(gameObject.transform.GetChild(i).gameObject)){
                    Enemys.Add(gameObject.transform.GetChild(i).gameObject);
                }   
                if(gameObject.transform.childCount == Enemys.Count){
                    break;
                }
            }
        }
    }

}
