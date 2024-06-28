using System;

[Serializable]
public class ItemInfo{
    public int item_id;
    public int item_damage;
    public int HPrecovery;
    public int MPrecovery;
    public string item_name;
}

[Serializable]
public class ItemInfos {
    public ItemInfo[] itemInfos;
}