public class EnemyAttack : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        if(enemyCount.Enemys.Count != 0){
            EnemyAi enemyAi = enemyCount.Enemys[data.enemy.id].GetComponent<EnemyAi>(); 
            enemyAi.stateMachine.Transition(new AttackState());
        }
    }
}
