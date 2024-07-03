using System.Collections.Generic;
using UnityEngine;

public class ItemPooling : MonoBehaviour
{
    [SerializeField]private GameObject itemPrefeb;
    [SerializeField]private EnemyCount enemyCount;
    private static ItemPooling itemPooling;
    private int createItemCount;
    [SerializeField]public ItemList itemList;
    [SerializeField]public Dictionary<string , List<int>> dropItemList = new Dictionary<string , List<int>>();

    private void Awake() {
        StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/getItemData", "ItemData", "1" , (value) => SetItemValue(value)));
        itemPooling = this;

    }
    void SetItemValue(string Data){
        
        ItemInfos itemDatas = JsonUtility.FromJson<ItemInfos>(Data);
        
        foreach(var item in itemDatas.itemInfos){
            itemList.AddItemdata(item);
        }
        
    }

    public static ItemPooling Instance
    {
        get
        {
            if (itemPooling == null)
            {
                return null;
            }
            return itemPooling;
        }
    }


    public void MakeItem(Transform dropPos, Transform userPos, int item)
    {
        if (item != 0)
        {
            
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                if(gameObject.transform.GetChild(i).gameObject.activeSelf){
                    continue;
                }
                GameObject thisItem = gameObject.transform.GetChild(i).gameObject;
                int type = thisItem.gameObject.GetComponent<SetItemInfo>().type;
                if (type == item)
                {
                    thisItem.transform.position = dropPos.position;
                    thisItem.GetComponent<Item>().SetTarget(userPos);
                    thisItem.SetActive(true);
                    createItemCount++;
                }
            }

            for(int i = 0; i < itemList.itemDatas.Count; i++){
                if(itemList.itemDatas[i].item_id == item){
                    GameObject thisItem = Instantiate(itemPrefeb , gameObject.transform);

                    thisItem.SetActive(false);
                    thisItem.GetComponent<Item>().SetTarget(userPos);
                    thisItem.GetComponent<SpriteRenderer>().sprite = itemList.itemImages[i];
                    thisItem.GetComponent<SetItemInfo>().setItemData(itemList.itemDatas[i]);
                    
                    thisItem.transform.position = dropPos.position;
                    thisItem.SetActive(true);
                    break;
                }
            }
            createItemCount = 0;
        }
    }
}