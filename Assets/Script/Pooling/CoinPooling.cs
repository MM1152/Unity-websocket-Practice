
using System;
using UnityEditor;
using UnityEngine;

public class CoinPooling : PoolingManager<Coin>
{   
    [SerializeField] GetInventoryData setGold;
    public static CoinPooling coinPooling;
    public override void Awake() {
        base.Awake();
        coinPooling = this;
    }

    public override void ShowObject(Transform dropPos, Transform userPos, int value)
    {
        setGold ??= Socket.Instance.this_player.GetComponent<MoveObject>().UI.GetComponent<GetInventoryData>();
        GetCoin(dropPos.gameObject.GetComponent<EnemyAi>().enemyData.DropGold); // 획득한 골드 양

        Coin coin = null;
        if(pooling.Count >= 3){
            for(int i = 0; i < 3; i++){
                coin = pooling.Dequeue();
                coin.transform.SetParent(null);
                coin.transform.position = dropPos.position;
                coin.gameObject.SetActive(true);
                
                StartCoroutine(coin.GetComponent<Coin>().AbsorbCoin(userPos));
            }
        }else {
            for(int i = 0; i < 3; i++){
                coin = Instantiate(prefab);
                coin.gameObject.SetActive(true);
                coin.transform.position = dropPos.position;

                coin.gameObject.SetActive(false);
                coin.gameObject.SetActive(true);

                StartCoroutine(coin.GetComponent<Coin>().AbsorbCoin(userPos));
            }
        }
    }
    /// <summary>
    /// 처음 코인을 얻었을때 
    /// </summary>
    public void GetCoin(int AcquiredGoldValue){
        Data data = new Data();
        data.title = "GetCoin";
        data.useItemType = AcquiredGoldValue;
        Socket.Instance.ws.Send(JsonUtility.ToJson(data));
    }
    /// <summary>
    /// 코인을 인벤토리에 적용시킬 때
    /// </summary>
    public void SettingCoin(Coin coin){
        setGold ??= Socket.Instance.this_player.GetComponent<MoveObject>().UI.GetComponent<GetInventoryData>();
        HttpRequest.HttpRequests.Request("inventoryData", "id", Socket.Instance.this_player.name, (value) => setGold.ChangeMoney(value , coin));
    }
}
