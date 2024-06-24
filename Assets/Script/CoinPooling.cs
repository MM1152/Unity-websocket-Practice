
using UnityEngine;

public class CoinPooling : MonoBehaviour
{   
    private static CoinPooling coinPooling;
    public static CoinPooling Instance 
    { 
        get {
            if(coinPooling == null){
                return null;
            }
            return coinPooling;
        }
    }
    [SerializeField] private GameObject coin;
    private int createCoinCount = 0;
    public int maxCoin = 0;
    private void Awake() {
        coinPooling = this;
    }
    public void MakeCoin(Transform dropPos , Transform userPos){
        for(int i = 0; i < transform.childCount; i++){
            GameObject thisCoin = gameObject.transform.GetChild(i).gameObject;
            if(createCoinCount == 3) break;
            if(thisCoin.activeSelf){
                continue;
            }
            createCoinCount++;
            thisCoin.transform.position = dropPos.position;
            thisCoin.SetActive(true);
            StartCoroutine(thisCoin.GetComponent<Coin>().AbsorbCoin(userPos));
        }

        while(createCoinCount < maxCoin){
            GameObject coinprefeb = Instantiate(coin , gameObject.transform) as GameObject;
            coinprefeb.SetActive(false);
            coinprefeb.transform.position = dropPos.position;
            coinprefeb.SetActive(true);
            createCoinCount++;
            StartCoroutine(coinprefeb.GetComponent<Coin>().AbsorbCoin(userPos));
        }

        createCoinCount = 0;
    }
}
