using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CoinPooling : MonoBehaviour
{
    [SerializeField]
    private GameObject coin;

    private int createCoinCount = 0;
    private Thread thread;
    public int maxCoin = 0;

    public void MakeCoin(Transform dropPos , Transform userPos){
        for(int i = 0; i < maxCoin; i++){
            if(gameObject.transform.childCount < i + 1){
                break;
            }
            GameObject thisCoin = gameObject.transform.GetChild(i).gameObject;
            if(thisCoin.activeSelf){
                continue;
            }
            createCoinCount++;
            thisCoin.transform.position = dropPos.position;
            thisCoin.SetActive(true);
            StartCoroutine(thisCoin.GetComponent<RotaionCoin>().AbsorbCoin(userPos));
        }

        while(createCoinCount < maxCoin){
            GameObject coinprefeb = Instantiate(coin , gameObject.transform) as GameObject;
            coinprefeb.SetActive(false);
            coinprefeb.transform.position = dropPos.position;
            coinprefeb.SetActive(true);
            createCoinCount++;
            StartCoroutine(coinprefeb.GetComponent<RotaionCoin>().AbsorbCoin(userPos));
        }

        createCoinCount = 0;
    }
}
