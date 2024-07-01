using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowShopItem : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IPointerClickHandler
{
    public int thisSlotItemType; 
    private Text itemText;
    private Image itemImage;
    private GameObject itemUI;
    private RectTransform itemRectTransForm;
    private GameObject buyingUI;
    [SerializeField]private string itemName;
    [SerializeField]private string item_damage;
    [SerializeField]private string item_HPrecovery;
    [SerializeField]private string item_MPrecovery;
    [SerializeField]private string item_cost;
    

    public void Start() {
        if(transform.parent.parent.Find("ItemUI") && transform.parent.parent.Find("BuyUI")){
            itemUI = transform.parent.parent.Find("ItemUI").gameObject;
            itemRectTransForm = itemUI.GetComponent<RectTransform>();
            buyingUI = transform.parent.parent.Find("BuyUI").gameObject;
        }
        itemText = itemUI.transform.Find("Name").GetComponent<Text>();
        itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
    }

    public void setItemInfo(ItemInfo itemList){
        thisSlotItemType = itemList.item_id;
        itemName = itemList.item_name.ToString();
        item_damage = itemList.item_damage.ToString();
        item_HPrecovery = itemList.HPrecovery.ToString();
        item_MPrecovery = itemList.MPrecovery.ToString();
        item_cost = itemList.cost.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameObject.GetComponent<Image>().sprite == null){
            return;
        }

        itemText.text = "이름 : " + itemName;
                
        if(CheckWord(item_damage)) itemText.text += "\n데미지 : " + item_damage;
        if(CheckWord(item_HPrecovery))  itemText.text += "\n회복량 : " + item_HPrecovery;
        if(CheckWord(item_MPrecovery))  itemText.text += "\n회복량 : " + item_MPrecovery;
        if(CheckWord(item_cost))  itemText.text += "\n가격 : " + item_cost;
    
                
        itemImage.sprite = ItemPooling.Instance.itemList.itemImages[thisSlotItemType - 1];  
        
        itemUI.transform.position = eventData.position + new Vector2( itemRectTransForm.rect.width / 2 , itemRectTransForm.rect.height / 2);
        itemUI.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        itemUI.SetActive(false);
        itemText.text = "";
    }
    public bool CheckWord(string word){
        if(word != null && word != "0" && word != ""){
            return true;
        }
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buyingUI.GetComponent<BuyItemUI>().itemIndex = thisSlotItemType;
        buyingUI.SetActive(true);
    }
}


