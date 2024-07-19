
using UnityEngine;
using UnityEngine.UI;

public class MakeShopItem : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject itemPrefeb;
    [SerializeField] NpcAi npcAi;
    [SerializeField] ItemList items;

    private void Awake() {
        npcAi = GetComponent<NpcAi>();
        items = ItemPooling.Instance.itemList;
    }

    public void MakeShop() {
        if(itemList.transform.childCount == 0){
            foreach(var type in npcAi.NpcData.sellingList) {
                foreach(var checktype in items.itemDatas){
                    if(type == checktype.item_id){
                        GameObject shopItem = Instantiate(itemPrefeb , itemList.transform);   
                        shopItem.GetComponent<ShowShopItem>().setItemInfo(checktype);
                        shopItem.GetComponent<Image>().sprite = items.itemImages[type - 1];
                    }
                }
            }
        }
    }
}
