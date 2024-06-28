using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ItemList
{
    public GameObject[] Items;
    public List<ItemInfo> itemDatas;

    public void AddItemdata(ItemInfo item){
        itemDatas.Add(item);
    }
    public Sprite[] itemImages;
}
