
using UnityEngine;

public class CoinPooling : PoolingManager<Coin>
{   
    public override void Awake() {
        base.Awake();
    }
    public override void ShowObject(Transform dropPos, Transform userPos, int value)
    {
        Coin coin = null;
        if(pooling.Count >= 3){
            for(int i = 0; i < 3; i++){
                coin = pooling.Dequeue();
                coin.transform.SetParent(null);
                coin.transform.position = dropPos.position;
                coin.gameObject.SetActive(true);
                
                Debug.Log($"Drop Pos is {dropPos.position} , Coin Position is {coin.gameObject.transform.position}");
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
}
