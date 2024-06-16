using System.Collections;
using System.Threading;
using UnityEngine;


public class EnemyAround : ISocket
{
    [SerializeField]
    private EnemyCount enemyCount;
    private Thread thread;
    public void Start()
    {
        setSocket();
    }

    public override void RunNetworkCode(Data data)
    {
        foreach (var enemy in enemyCount.Enemys)
        {
            int thisEnemyIndex = int.Parse(enemy.name.Split(' ')[1]);
            EnemyAi enemyAi = enemy.GetComponent<EnemyAi>();
            enemyAi.Hpbar.value = data.enemyList[thisEnemyIndex].Hp;
            enemyAi.FindUser.SetActive(false);
            if(data.enemyList[thisEnemyIndex].state.Equals("Die")){
                if(enemy.activeSelf){
                   StartCoroutine(enemyAi.Die(data.this_player)); 
                }
            }

            else if (data.enemyList[thisEnemyIndex].state.Equals("Hit"))
            {
                enemyAi.ani.SetTrigger("IsHit");
                
            }
            else
            {
                
                if (data.enemyList[thisEnemyIndex].state.Equals("FollowUser"))
                {   
                    enemyAi.FindUser.SetActive(true);
                }
                else if(data.enemyList[thisEnemyIndex].state.Equals("AttackAroundInUser"))
                {
                   enemyAi.Attack();
                }

                StartCoroutine(enemyAi.Move(new Vector2(data.enemyList[thisEnemyIndex].x, data.enemyList[int.Parse(enemy.name.Split(' ')[1])].y)));
            }

        }
    }
}
