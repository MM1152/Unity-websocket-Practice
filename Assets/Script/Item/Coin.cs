using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Transform targetPos;
    Socket socket;
    private void Awake() {
        socket ??= Socket.Instance;
    }
    private void Update()
    {
        if (targetPos != null)
        {
            transform.position += (targetPos.position - transform.position).normalized * Time.deltaTime;
            if (Vector2.Distance(transform.position, targetPos.position) < 0.5f && gameObject.name.Equals("Coin(Clone)"))
            {
                socket.ws.Send(JsonUtility.ToJson(new Data("GetCoin", targetPos.gameObject.name)));
                gameObject.SetActive(false);
                targetPos = null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }

    public IEnumerator AbsorbCoin(Transform userPos)
    {
        yield return new WaitForSeconds(1.3f);
        targetPos = userPos;
    }




}