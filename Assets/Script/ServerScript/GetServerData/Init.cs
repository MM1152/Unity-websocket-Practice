
using Unity.VisualScripting;
using UnityEngine;

public class Init : ISocket
{
    [SerializeField]
    private GameObject[] enemys;
    [SerializeField]
    private EnemyCount enemyCount;
    [SerializeField]
    public void Start()
    {
        setSocket();
    }
    public override void RunNetworkCode(Data data)
    {
        foreach(var enemy in data.enemyList){
            GameObject SpawnEnemy = Instantiate(enemys[enemy.type - 1]) as GameObject;
            SpawnEnemy.name = "Enemy " + enemy.id.ToString();
            SpawnEnemy.transform.position = new Vector3(enemy.x , enemy.y , -2f);
            SpawnEnemy.transform.SetParent(enemyCount.transform);
        }
        GameObject player = Instantiate(socket.user);
        
        socket.this_player = player;
        socket.this_player_MoveObject = socket.this_player.GetComponent<MoveObject>();
        player.name = data.id;
        data = new Data("Connection");

        enemyCount.setEnemys();
        socket.ws.Send(JsonUtility.ToJson(data));

    }
}