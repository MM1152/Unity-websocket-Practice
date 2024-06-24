using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GetInventoryData : MonoBehaviour
{
    [SerializeField] private ItemList items;
    [SerializeField] private GameObject itemTab;
    [SerializeField] private Transform inventorySize;
    HttpRequest httpRequest;
    InventoryData inven;
    Socket socket;
    SaveInvenData saveData;

    // Start is called before the first frame update
    void Start()
    {
        socket = Socket.Instance;
        saveData = new SaveInvenData();
        httpRequest = HttpRequest.HttpRequests;
        saveData.id = socket.this_player.name;
        StartCoroutine(httpRequest.Request("http://localhost:8001/inventoryData", "id", socket.this_player.name, (value) => GetData(value)));
    }

    void GetData(string Data)
    {
        inven = JsonConvert.DeserializeObject<InventoryData>(Data);
        Debug.Log(inven);
        
        foreach (var item in inven.item.Keys)
        {
            GameObject createItem = Instantiate(itemTab, inventorySize);
            if (inven.item[item] == 0)
            {
                continue;
            }
            for(int i = 0; i < items.Items.Length; i++){
                if(items.Items[i].GetComponent<SetItemInfo>().type == inven.item[item]){
                    createItem.GetComponent<Image>().sprite = items.Items[i].GetComponent<SpriteRenderer>().sprite;
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
            if (image.sprite == null)
            {
                for (int j = 0; j < items.Items.Length; j++)
                {
                    Sprite itemImage = items.Items[j].GetComponent<SpriteRenderer>().sprite;
                    int itemType = items.Items[j].GetComponent<SetItemInfo>().type;
                    if (item == itemType)
                    {
                        image.sprite = itemImage;
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
