using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    public List<GameObject> Enemys;
    public GameObject prefebEnemy;
    public void setEnemys(){
        for(int i = 0; i < this.gameObject.transform.childCount; i++){
            Enemys.Add(gameObject.transform.GetChild(i).gameObject);
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
