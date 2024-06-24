using UnityEngine;

public class SetItemInfo : MonoBehaviour
{
    
    [SerializeField] private ItemInfo iteminfo;

    public int type {
        get {
            return iteminfo.type;
        }
    }
    public int damage {
        get {
            return iteminfo.damage;
        }
    }
    public string name {
        get{
            return iteminfo.name;
        }
    }

}
