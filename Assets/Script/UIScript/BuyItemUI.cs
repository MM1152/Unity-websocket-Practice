using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemUI : MonoBehaviour
{
    private GetInventoryData inventoryData;
    private InventoryData inven;
    public int itemIndex;
    
    private void Awake() {
        inventoryData = GameObject.Find("InventoryANDstatus").GetComponent<GetInventoryData>();
        
    }
    public void Buy() {
        Data data = new Data("BuyItem" , itemIndex.ToString());
        Socket.Instance.ws.Send(JsonUtility.ToJson(data));

        HttpRequest.HttpRequests.Request("http://localhost:8001/inventoryData", "id", Socket.Instance.this_player.name, (value) => inventoryData.ChangeMoney(value , this.gameObject));
        inventoryData.SetInventory(itemIndex , null);
    }
    public void DontBuy() {
        this.gameObject.SetActive(false);
    }
}
