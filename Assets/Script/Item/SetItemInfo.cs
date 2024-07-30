using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class SetItemInfo : MonoBehaviour
{
    
    [SerializeField]            private ItemInfo iteminfo;
    [SerializeField]            private SpriteRenderer sp;
    private ItemPooling itemPooling;
    private int itemIndex;
    private void Awake() {
        sp = GetComponent<SpriteRenderer>();
        itemPooling = ItemPooling.ItemPool;
    }

    public int ItemIndex {
        get => itemIndex;
        set {
            itemIndex = value;
            sp.sprite = itemPooling.itemList.itemImages[itemIndex - 1];
        }
    }
}
