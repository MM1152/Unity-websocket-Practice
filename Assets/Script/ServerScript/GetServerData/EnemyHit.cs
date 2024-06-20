using UnityEngine;

public class EnemyHit : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        EnemyAi enemy = GameObject.Find(data.id).GetComponent<EnemyAi>();
        enemy.Hit();
    }
}