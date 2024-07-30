using System.Collections.Generic;
using UnityEngine;

public class ItemPooling : PoolingManager<Item>
{
    [SerializeField] private EnemyCount enemyCount;
    private static ItemPooling itemPool;
    public static ItemPooling ItemPool
    {
        get
        {
            if (itemPool == null)
            {
                return null;
            }
            return itemPool;
        }
    }
    [SerializeField] public ItemList itemList;
    [SerializeField] public Dictionary<string, List<int>> dropItemList = new Dictionary<string, List<int>>();

    public override void Awake()
    {
        base.Awake();
        HttpRequest.HttpRequests.Request("http://localhost:8001/getItemData", "ItemData", "1", (value) => SetItemValue(value));
        itemPool = this;
    }

    void SetItemValue(string Data)
    {
        ItemInfos itemDatas = JsonUtility.FromJson<ItemInfos>(Data);

        foreach (var item in itemDatas.itemInfos)
        {
            itemList.AddItemdata(item);
        }
    }

    public override void ShowObject(Transform dropPos, Transform userPos, int item)
    {
        if (item != 0)
        {
            Item createItem;
            
            if(pooling.Count > 0) createItem = pooling.Dequeue();
            else createItem = Instantiate(prefab);

            createItem.gameObject.SetActive(false);
            
            createItem.GetComponent<SetItemInfo>().ItemIndex = item;
            createItem.GetComponent<Item>().SetTarget(userPos);
            createItem.transform.position = dropPos.position;
            createItem.gameObject.SetActive(true);
        }
    }
}