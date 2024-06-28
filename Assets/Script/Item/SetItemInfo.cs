using UnityEngine;

public class SetItemInfo : MonoBehaviour
{
    
    [SerializeField] private ItemInfo iteminfo;
    
    public void setItemData(ItemInfo itemInfo){
        this.iteminfo = itemInfo;
    }
    public int type {
        get {
            return iteminfo.item_id;
        }
    }
    public int damage {
        get {
            return iteminfo.item_damage;
        }
    }
    public string name {
        get{
            return iteminfo.item_name;
        }
    }
    public int HPrecovery {
        get{
            return iteminfo.HPrecovery;
        }
    }
    public int MPrecovery {
        get{
            return iteminfo.HPrecovery;
        }
    }

}
