using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetInventoryData : MonoBehaviour
{
    [SerializeField] private ItemPooling itemPooling;
    [SerializeField] private GameObject itemTab;
    [SerializeField] private Transform inventorySize;
    [SerializeField] private ItemList itemList;
    [SerializeField] private InventoryData inven;
    [SerializeField] private Text gold;
    public int[] itemsNumber;
    HttpRequest httpRequest;
    Socket socket;
    SaveInvenData saveData;
    int slotIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        itemPooling = ItemPooling.Instance;
        socket = Socket.Instance;
        saveData = new SaveInvenData();
        httpRequest = HttpRequest.HttpRequests;
        saveData.id = socket.this_player.name;
        itemList = itemPooling.itemList;
        itemsNumber = new int[itemList.itemDatas.Count];
        StartCoroutine(httpRequest.Request("http://localhost:8001/inventoryData", "id", socket.this_player.name, (value) => InitInventory(value)));
    }
    
    public void ChangeMoney(string Data , GameObject offGameObject){
        inven = JsonConvert.DeserializeObject<InventoryData>(Data);
        gold.text = "Gold : " + inven.gold.ToString();
        offGameObject.SetActive(false);
    }

    public void InitInventory(string Data)
    {
        inven = JsonConvert.DeserializeObject<InventoryData>(Data);
        Debug.Log(inven.gold);
        gold.text = "Gold : " + inven.gold.ToString();
        if(inventorySize.transform.childCount != 0){
            return;
        }

        foreach(var equip in inven.equip.Keys){
            transform.Find("Equip").Find(equip).transform.GetChild(0).GetComponent<ShowItemUi>().ThisSlotItemType = inven.equip[equip];
        }
        foreach (var item in inven.item.Keys)
        {
            GameObject createItem = Instantiate(itemTab, inventorySize);
            createItem.name = slotIndex++.ToString();  
            createItem.GetComponent<ShowItemUi>().ThisSlotItemType = 0;
            if (inven.item[item] == 0)
            {
                continue;
            }
            itemsNumber[inven.item[item] - 1]++;
            createItem.GetComponent<ShowItemUi>().ThisSlotItemType = inven.item[item];
        }
    }
    public void SetInventory(int item , GameObject thisItem)
    {
        
        for (int i = 0; i < 30; i++)
        {
            ShowItemUi showItemUi = inventorySize.GetChild(i).GetComponent<ShowItemUi>();
            if (showItemUi.ThisSlotItemType == 0)
            {
                for (int j = 0; j < itemList.itemDatas.Count; j++)
                {
                    ItemInfo iteminfo = itemList.itemDatas[j];
                    Sprite itemImage = itemList.itemImages[j];
                    int itemType = itemList.itemDatas[j].item_id;
                    if (item == itemType)
                    {
                        showItemUi.ThisSlotItemType = itemType;
                        inventorySize.transform.GetChild(i).GetComponent<ShowItemUi>().ThisSlotItemType = itemType;
                        saveData.Key = (i + 1).ToString();
                        saveData.Value = itemType;
                        string jsonData = JsonUtility.ToJson(saveData);
                        StartCoroutine(httpRequest.Request("http://localhost:8001/saveinventoryData", "item", jsonData , (value) => PickUpItem(thisItem)));  
                        break;
                    }
                    
                }
                break;
            }
        }
                
       
    }
    void PickUpItem(GameObject thisItem){
        if(thisItem != null){
            thisItem.SetActive(false);
        }
    }
}
