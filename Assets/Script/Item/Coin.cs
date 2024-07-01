using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Socket socket;
    [SerializeField] GetInventoryData setGold;
    private void Awake() {
        socket ??= Socket.Instance;
        setGold = socket.this_player.GetComponent<MoveObject>().UI.GetComponent<GetInventoryData>();
    }
    public IEnumerator AbsorbCoin(Transform userPos)
    {   
        
        yield return new WaitForSeconds(1.3f);
        
        for(float radio = 0f; radio <= 1f; radio += 0.005f){
            transform.position = Vector3.Lerp(transform.position , userPos.position  , radio);
            yield return null;
        }
        StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/inventoryData", "id", socket.this_player.name, (value) => setGold.ChangeMoney(value , this.gameObject)));
        
    }




}