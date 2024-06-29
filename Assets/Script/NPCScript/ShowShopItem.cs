using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowShopItem : MonoBehaviour , IPointerEnterHandler
{
    public int thisSlotItemType; 
    private Text itemText;
    private Image itemImage;
    private GameObject itemUI;
    private RectTransform itemRectTransForm;
    [SerializeField]private string itemName;
    [SerializeField]private string item_damage;
    [SerializeField]private string item_HPrecovery;
    [SerializeField]private string item_MPrecovery;

    public void Start() {
        if(transform.parent.parent.Find("ItemUI")){
            itemUI = transform.parent.parent.Find("ItemUI").gameObject;
            itemRectTransForm = itemUI.GetComponent<RectTransform>();
        }
        itemText = itemUI.transform.Find("Name").GetComponent<Text>();
        itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
    }
    public void setItemInfo(ItemInfo itemList){
        thisSlotItemType = itemList.item_id;
        itemName = itemList.item_id.ToString();
        item_damage = itemList.item_damage.ToString();
        item_HPrecovery = itemList.HPrecovery.ToString();
        item_MPrecovery = itemList.MPrecovery.ToString();
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
    
                
        itemImage.sprite = ItemPooling.Instance.itemList.itemImages[thisSlotItemType - 1];  
        
        itemUI.transform.position = eventData.position + new Vector2( itemRectTransForm.rect.width / 2 , itemRectTransForm.rect.height / 2);
        itemUI.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        itemText.text = "";
        itemUI.SetActive(false);
    }
    public bool CheckWord(string word){
        if(word != null && word != "0" && word != ""){
            return true;
        }
        return false;
    }

}


