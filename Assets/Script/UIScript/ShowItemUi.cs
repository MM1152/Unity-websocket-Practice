using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler , IPointerClickHandler
{
    [SerializeField] private GameObject itemUI;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private RectTransform itemRectTransForm;
    [SerializeField] private Sprite InitImage;
    [SerializeField] private GetInventoryData inventoryData;
    [SerializeField] private GameObject equipText;
    [SerializeField] private GameObject equipAbleSlot;
    /// <summary>
    /// EquipItemInfo 를 집어 넣었놓는 변수 , 존재하면 해당 인벤토리의 위치가 변경되었을 때 , EquipItemInfo의 EquipItemInfo에 접근해 변경된 인벤토리 위치를 저장시켜줌
    /// </summary>
    [SerializeField] EquipItemInfo equipItemInfo;

    GameObject moveItemSlot;
    static bool isDrag;
    Sprite curImage;
    [SerializeField] Image thisSlotImage;
    [SerializeField] private int thisSlotItemType;
    
    public int ThisSlotItemType {
        get {
            return thisSlotItemType;
        }
        set {
            thisSlotItemType = value;
            if(thisSlotImage == null) thisSlotImage = GetComponent<Image>();
            if(thisSlotItemType == 0){
                thisSlotImage.sprite = InitImage;
                return;
            }
            try {   
                thisSlotImage.sprite = ItemPooling.Instance.itemList.itemImages[thisSlotItemType - 1];
            } catch (Exception ex){
                Debug.LogError("Fail Set Item Image\n " + ex.Message);
            }
            
        }
    }
    public bool equipItem;
    private void Update(){
        if((equipText.activeSelf && !equipItem) || isDrag){
            equipText.SetActive(false);
        }
        else if(equipItem){
            equipText.SetActive(true);
        } 
        
    }
    private void Start()
    {
        if (transform.parent.parent.Find("ItemUI"))
        {
            itemUI = transform.parent.parent.Find("ItemUI").gameObject;
        }
        if(thisSlotItemType != 0 && thisSlotItemType != 4 && thisSlotItemType != 5){
            equipAbleSlot = transform.root.Find("InventoryANDstatus").Find("Equip").Find(ItemPooling.Instance.itemList.itemDatas[thisSlotItemType - 1].item_Type).gameObject;
        }
        inventoryData = transform.parent.parent.parent.GetComponent<GetInventoryData>();
        itemNameText = itemUI.transform.Find("Name").GetComponent<Text>();
        itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
        itemRectTransForm = itemUI.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDrag || gameObject.GetComponent<ShowItemUi>().thisSlotItemType == 0)
        {
            return;
        }
        foreach (var type in ItemPooling.Instance.itemList.itemDatas)
        {
            if (type.item_id == thisSlotItemType)
            {

                itemNameText.text = "이름 : " + type.item_name;

                if (CheckWord(type.item_damage.ToString())) itemNameText.text += "\n데미지 : " + type.item_damage;
                if (CheckWord(type.HPrecovery.ToString())) itemNameText.text += "\n회복량 : " + type.HPrecovery;
                if (CheckWord(type.MPrecovery.ToString())) itemNameText.text += "\n회복량 : " + type.MPrecovery;
                if (CheckWord(type.item_defense.ToString())) itemNameText.text += "\n방어력 : " + type.item_defense;


                itemImage.sprite = ItemPooling.Instance.itemList.itemImages[type.item_id - 1];
                break;
            }
        }
        itemUI.transform.position = eventData.position + new Vector2(itemRectTransForm.rect.width / 2, itemRectTransForm.rect.height / 2);
        itemUI.SetActive(true);
    }
    public bool CheckWord(string word)
    {
        if (word != null && word != "0" && word != "")
        {
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
        if (moveItemSlot != null)
        {
            moveItemSlot.transform.position = eventData.position;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (thisSlotItemType == 0 || gameObject.transform.parent.parent.name == "MapUI" || gameObject.tag == "EquipItem")
        {
            return;
        }

        isDrag = true;
        curImage = gameObject.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().sprite = InitImage;
        itemUI.SetActive(false);
        moveItemSlot = Instantiate(gameObject, gameObject.transform.parent.parent) as GameObject;
        moveItemSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
        moveItemSlot.GetComponent<Image>().sprite = curImage;
        moveItemSlot.layer = 5;
        moveItemSlot.transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(gameObject.tag == "EquipItem") return;

        Vector3 ray = (Vector3)eventData.position + (Vector3.back * 10f);
        Vector3 direction = Vector3.forward * 100f;

        if(!EquipItemSlot(ray , direction)){
            Debug.Log("Equip Item false");
            InventoryMoveSlot(ray, direction);
        }

        moveItemSlot = null;
        isDrag = false;
    }
    void InventoryMoveSlot(Vector3 ray, Vector3 direction)
    {
        Image thisSlot = GetComponent<Image>();
        RaycastHit2D hit = Physics2D.Raycast(ray, direction, Mathf.Infinity, LayerMask.GetMask("InventoryUI"));

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<ShowItemUi>().thisSlotItemType != 0)
            {
                Destroy(moveItemSlot);
                thisSlot.sprite = curImage;
            }   
            else
            {
                GameObject colliderObj = hit.collider.gameObject;
                ShowItemUi showItemUi = colliderObj.GetComponent<ShowItemUi>();
                
                showItemUi.thisSlotItemType = thisSlotItemType;
                showItemUi.equipItem = equipItem;                
                equipItem = false;
                if(equipItemInfo != null){
                    showItemUi.equipItemInfo = equipItemInfo;
                    
                    equipItem = false;
                    equipItemInfo.EquipItemSlot = colliderObj;
                    equipItemInfo = null;
                    
                }

                thisSlotItemType = 0;
                colliderObj.GetComponent<Image>().sprite = curImage;
                ChangeItemSlot(gameObject.name, thisSlotItemType, colliderObj.name, showItemUi.thisSlotItemType);
                Destroy(moveItemSlot);
            }
        }
        else
        {
            Destroy(moveItemSlot);
            ChangeItemSlot(gameObject.name, 0, null, 0);
            inventoryData.itemsNumber[thisSlotItemType - 1]--;
            thisSlotItemType = 0;
            thisSlot.sprite = InitImage;
        }
    }
    bool EquipItemSlot(Vector3 ray, Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray, direction, Mathf.Infinity, LayerMask.GetMask("EquipUI"));

        if (hit.collider != null)
        {
            if( hit.collider.gameObject.name != ItemPooling.Instance.itemList.itemDatas[thisSlotItemType - 1].item_Type) {
                gameObject.GetComponent<Image>().sprite = curImage;
                Destroy(moveItemSlot);
                return true;
            }
            GameObject colliderObj = hit.collider.gameObject;
            ShowItemUi showItemUi = colliderObj.transform.GetChild(0).GetComponent<ShowItemUi>();
            equipItemInfo = colliderObj.GetComponent<EquipItemInfo>();

            equipItemInfo.EquipItemSlot = this.gameObject;
            equipItemInfo.ThisSlotItemType = thisSlotItemType;
            showItemUi.thisSlotItemType = thisSlotItemType;
            equipItem = true;
            showItemUi.GetComponent<Image>().sprite = curImage;
            gameObject.GetComponent<Image>().sprite = curImage;
   
            EquipItem(gameObject.name, thisSlotItemType, colliderObj.name, showItemUi.thisSlotItemType);
            
            Destroy(moveItemSlot);
            return true;
        }

        return false;
    }
    private void ChangeItemSlot(string changedSlotKey, int changedSlotValue, string changeSlotKey, int changeSlotValue)
    {
        SaveInvenData saveData = new SaveInvenData();
        saveData.id = Socket.Instance.this_player.name;
        saveData.Key = changedSlotKey;
        saveData.Value = changedSlotValue;
        StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/saveinventoryData", "item", JsonUtility.ToJson(saveData), (value) => Debug.Log("saveInventory")));

        saveData.Key = changeSlotKey;
        saveData.Value = changeSlotValue;
        StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/saveinventoryData", "item", JsonUtility.ToJson(saveData), (value) => Debug.Log("saveInventory")));
    }
    private void EquipItem(string key, int value , string key1 , int value1)
    {
        SaveInvenData saveData = new SaveInvenData();
        saveData.id = Socket.Instance.this_player.name;
        saveData.Key = key;
        saveData.Value = value;
        //StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/saveinventoryData", "item", JsonUtility.ToJson(saveData), (value) => Debug.Log("Equip Item")));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right){
            switch (gameObject.tag) {
                case "EquipItem" :
                    if(thisSlotItemType != 0){
                        thisSlotItemType = 0;
                        EquipItemInfo equipItemInfo = gameObject.transform.parent.GetComponent<EquipItemInfo>();
                        equipItemInfo.ThisSlotItemType = thisSlotItemType;
                        if(equipItemInfo != null){
                            equipItemInfo.EquipItemSlot.GetComponent<ShowItemUi>().equipItem = false;
                            equipItemInfo.EquipItemSlot.GetComponent<ShowItemUi>().equipItemInfo = null;
                        }
                        itemUI.SetActive(false);
                        gameObject.GetComponent<Image>().sprite = null;
                    }
                    break;
                case "ItemTab" :
                    if(thisSlotItemType != 0 && !equipItem){
                        EquipItemInfo equipItemInfo = equipAbleSlot.GetComponent<EquipItemInfo>();
                        if(equipItemInfo.EquipItemSlot != null){
                            equipItemInfo.EquipItemSlot.GetComponent<ShowItemUi>().equipItem = false;
                        }
                        Debug.Log("Euqip Item");
                        equipItem = true;
                        equipAbleSlot.transform.GetChild(0).gameObject.GetComponent<ShowItemUi>().ThisSlotItemType = thisSlotItemType;

                        equipItemInfo.EquipItemSlot = this.gameObject;
                    }
                    break;

            }
        }
    }
}
