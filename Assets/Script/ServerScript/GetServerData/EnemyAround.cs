using System.Collections;
using System.Threading;
using UnityEngine;


public class EnemyAround : ISocket
{
    private Thread thread;
    

    public override void RunNetworkCode(Data data)
    {
        foreach (var enemy in enemyCount.Enemys)
        {
            int thisEnemyIndex = int.Parse(enemy.name.Split(' ')[1]);
            EnemyAi enemyAi = enemy.GetComponent<EnemyAi>();
            enemyAi.FindUser.SetActive(false);


            if (data.enemyList[thisEnemyIndex].state.Equals("FollowUser"))
            {
                enemyAi.FindUser.SetActive(true);
            }
            if(!enemyAi.IsAttack){
                enemyAi.stateMachine.Transition(new MoveState() , new Vector2(data.enemyList[thisEnemyIndex].x , data.enemyList[thisEnemyIndex].y));
            }
        }
    }
}
