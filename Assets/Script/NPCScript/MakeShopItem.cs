
using UnityEngine;
using UnityEngine.UI;

public class MakeShopItem : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject itemPrefeb;
    [SerializeField] GameObject shop;
    [SerializeField] NpcAi npcAi;
    [SerializeField] ItemList items;
    
    private void Start() {
        npcAi = GetComponent<NpcAi>();
        items = ItemPooling.ItemPool.itemList;
        shop = gameObject.transform.Find("Canvas").Find("Shop").gameObject;
        itemList = gameObject.transform.transform.Find("Canvas").Find("Shop").Find("ItemList").gameObject;
        itemPrefeb = Resources.Load<GameObject>("ShopItem");
        MakeShop();
    }

    private void OnTriggerExit2D(Collider2D other) {
         if(other.tag == "Player" && Socket.Instance.this_player == other.gameObject){
            shop.SetActive(false);
         }
    }
    private void Update() {
        if(npcAi.inPlayer){
            if(Input.GetKeyDown(KeyCode.X)) shop.SetActive(!shop.activeSelf);
        }
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
