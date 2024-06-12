using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyAround : ISocket
{
    [SerializeField]
    private EnemyCount enemyCount;
    private Thread thread;
    public void Start(){
        setSocket();
    }

    public override void RunNetworkCode(Data data)
    {
        foreach(var enemy in enemyCount.Enemys){
            
            StartCoroutine(enemy.GetComponent<EnemyAi>().Move
            (new Vector2(data.enemyList[int.Parse(enemy.name.Split(' ')[1])].x , 
            data.enemyList[int.Parse(enemy.name.Split(' ')[1])].y)));
        }
    }
    
    
}