using System;

[Serializable]
public class ItemInfo{
    public string item_Type;
    public int item_id;
    public int item_damage;
    public int item_defense;
    public int HPrecovery;
    public int MPrecovery;
    public string item_name;
    public int cost;
}

[Serializable]
public class ItemInfos {
    public ItemInfo[] itemInfos;
}