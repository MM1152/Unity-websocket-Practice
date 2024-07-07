using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemUi : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler , IDragHandler , IBeginDragHandler , IEndDragHandler
{
    [SerializeField] private GameObject itemUI;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private ItemList itemList;
    [SerializeField] private RectTransform itemRectTransForm;
    [SerializeField] private Sprite InitImage;
    [SerializeField]private GetInventoryData inventoryData;
    GameObject moveItemSlot;
    static bool isDrag;
    Sprite thisSlotImage;
    public int thisSlotItemType;
    private void Start() {
        itemList = ItemPooling.Instance.itemList;
        if(transform.parent.parent.Find("ItemUI")){
            itemUI = transform.parent.parent.Find("ItemUI").gameObject;
        }
        inventoryData = transform.parent.parent.parent.GetComponent<GetInventoryData>();
        itemNameText = itemUI.transform.Find("Name").GetComponent<Text>();
        itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
        itemRectTransForm = itemUI.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDrag || gameObject.GetComponent<ShowItemUi>().thisSlotItemType == 0){
            return;
        }
        foreach (var type in itemList.itemDatas){
            if(type.item_id == thisSlotItemType){
                
                itemNameText.text = "이름 : " + type.item_name;
                
                if(CheckWord(type.item_damage.ToString())) itemNameText.text += "\n데미지 : " + type.item_damage;
                if(CheckWord(type.HPrecovery.ToString()))  itemNameText.text += "\n회복량 : " + type.HPrecovery;
                if(CheckWord(type.MPrecovery.ToString()))  itemNameText.text += "\n회복량 : " + type.MPrecovery;
    
                
                itemImage.sprite = itemList.itemImages[type.item_id - 1];
                break;
            }
        }
        itemUI.transform.position = eventData.position + new Vector2( itemRectTransForm.rect.width / 2 , itemRectTransForm.rect.height / 2);
        itemUI.SetActive(true);
    }
    public bool CheckWord(string word){
        if(word != null && word != "0" && word != ""){
            return true;
        }
        return false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemNameText.text = " ";
        itemUI.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(moveItemSlot != null){
            moveItemSlot.transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        thisSlotImage = gameObject.GetComponent<Image>().sprite;
        
        if(thisSlotItemType == 0 || gameObject.transform.parent.parent.name == "MapUI"){
            return;
        }
        gameObject.GetComponent<Image>().sprite = InitImage;
        itemUI.SetActive(false);
        moveItemSlot = Instantiate(gameObject , gameObject.transform.parent.parent) as GameObject;
        moveItemSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(30f , 30f);
        moveItemSlot.GetComponent<Image>().sprite = thisSlotImage;
        moveItemSlot.layer = 5;
        moveItemSlot.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        Vector3 ray = (Vector3)eventData.position + (Vector3.back * 10f); 
        Vector3 direction = Vector3.forward * 100f;
        Image thisSlot = GetComponent<Image>();
        RaycastHit2D hit = Physics2D.Raycast(ray, direction, Mathf.Infinity , LayerMask.GetMask("InventoryUI"));

        if(hit.collider != null){
            if(hit.collider.GetComponent<ShowItemUi>().thisSlotItemType != 0){
                Destroy(moveItemSlot);
                thisSlot.sprite = thisSlotImage;
            }
            else {
                GameObject colliderObj = hit.collider.gameObject;
                ShowItemUi showItemUi = colliderObj.GetComponent<ShowItemUi>();
                showItemUi.thisSlotItemType = thisSlotItemType;
                thisSlotItemType = 0;
                colliderObj.GetComponent<Image>().sprite = thisSlotImage;
                ChangeItemSlot(gameObject.name , thisSlotItemType , colliderObj.name , showItemUi.thisSlotItemType);
                Destroy(moveItemSlot);
            }
        }else {
            Destroy(moveItemSlot);
            ChangeItemSlot(gameObject.name , 0 , null , 0);
            inventoryData.itemsNumber[thisSlotItemType - 1]--;
            thisSlotItemType = 0;
            thisSlot.sprite = InitImage;
        }
        moveItemSlot = null;
        isDrag = false;
    }
    private void ChangeItemSlot(string changedSlotKey , int changedSlotValue , string changeSlotKey , int changeSlotValue){
        SaveInvenData saveData = new SaveInvenData();
        saveData.id = Socket.Instance.this_player.name;
        saveData.Key = changedSlotKey;
        saveData.Value = changedSlotValue;
        StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/saveinventoryData", "item", JsonUtility.ToJson(saveData) , (value) => Debug.Log("saveInventory"))); 

        saveData.Key = changeSlotKey;
        saveData.Value = changeSlotValue;
        StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/saveinventoryData", "item", JsonUtility.ToJson(saveData) , (value) => Debug.Log("saveInventory")));
    }
}
