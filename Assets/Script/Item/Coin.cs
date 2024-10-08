using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Socket socket;

    private void Awake() {
        socket = Socket.Instance;
    }
    public IEnumerator AbsorbCoin(Transform userPos)
    {   
        
        yield return new WaitForSeconds(1.3f);
        
        for(float radio = 0f; radio <= 1f; radio += 0.005f){
            transform.position = Vector3.Lerp(transform.position , userPos.position  , radio);
            yield return null;
        }
        
        CoinPooling.coinPooling.SettingCoin(this);
    }




}