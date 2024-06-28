using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GetInventoryData : MonoBehaviour
{
    [SerializeField] private ItemPooling itemPooling;
    [SerializeField] private GameObject itemTab;
    [SerializeField] private Transform inventorySize;
    [SerializeField] private ItemList itemList;

    HttpRequest httpRequest;
    InventoryData inven;
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
        StartCoroutine(httpRequest.Request("http://localhost:8001/inventoryData", "id", socket.this_player.name, (value) => GetData(value)));
    }

    void GetData(string Data)
    {
        inven = JsonConvert.DeserializeObject<InventoryData>(Data);
        Debug.Log(Data);

        foreach (var item in inven.item.Keys)
        {
            GameObject createItem = Instantiate(itemTab, inventorySize);
            createItem.name = slotIndex++.ToString();
            createItem.GetComponent<ShowItemUi>().thisSlotItemType = 0;
            if (inven.item[item] == 0)
            {
                continue;
            }
            for(int i = 0; i < itemList.itemDatas.Count; i++){
                ItemInfo iteminfo = itemList.itemDatas[i];
                Sprite itemImage = itemList.itemImages[i];

                if(iteminfo.item_id == inven.item[item]){
                    createItem.GetComponent<Image>().sprite = itemImage;
                    createItem.GetComponent<ShowItemUi>().thisSlotItemType = inven.item[item];
                    break;
                }
            }
            
        }
    }
    public void SetInventory(int item , GameObject thisItem)
    {
        for (int i = 0; i < 30; i++)
        {
            Image image = inventorySize.GetChild(i).GetComponent<Image>();
            ShowItemUi showItemUi = inventorySize.GetChild(i).GetComponent<ShowItemUi>();
            if (image.sprite == null)
            {
                
                for (int j = 0; j < itemList.itemDatas.Count; j++)
                {
                    ItemInfo iteminfo = itemList.itemDatas[j];
                    Sprite itemImage = itemList.itemImages[j];
                    int itemType = itemList.itemDatas[j].item_id;
                    if (item == itemType)
                    {
                        image.sprite = itemImage;
                        showItemUi.thisSlotItemType = itemType;
                        inventorySize.transform.GetChild(i).GetComponent<ShowItemUi>().thisSlotItemType = itemType;
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
        Debug.Log(thisItem);
        thisItem.SetActive(false);
    }
}
