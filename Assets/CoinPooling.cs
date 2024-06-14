using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPooling : MonoBehaviour
{
    [SerializeField]
    private GameObject coin;
    private int createCoinCount = 0;
    public int maxCoin = 0;

    public void MakeCoin(Transform transform){
        for(int i = 0; i < this.gameObject.transform.childCount; i++){
            GameObject thisCoin = gameObject.transform.GetChild(i).gameObject;
            if(thisCoin.activeSelf){
                continue;
            } else {
                createCoinCount++;
                thisCoin.transform.position = transform.position;
                thisCoin.SetActive(true);
            }
        }

        while(createCoinCount <= maxCoin){
            GameObject coinprefeb = Instantiate(coin , gameObject.transform) as GameObject;
            coinprefeb.SetActive(false);
            coinprefeb.transform.position = transform.position;
            coinprefeb.SetActive(true);
            createCoinCount++;
        }

        createCoinCount = 0;
    }
}
