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
        inventoryData = GameObject.FindObjectOfType<GetInventoryData>();
    }
    public void Buy() {
        Data data = new Data("BuyItem" , itemIndex.ToString());
        Socket.Instance.ws.Send(JsonUtility.ToJson(data));

        HttpRequest.HttpRequests.Request("inventoryData", "id", Socket.Instance.this_player.name, (value) => inventoryData.ChangeMoney(value , null));
        bool insert = inventoryData.SetInventory(itemIndex , null);
        if(insert) Debug.Log("Fail To Buying Item");
    }
    public void DontBuy() {
        this.gameObject.SetActive(false);
    }
}
