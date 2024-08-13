using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler , IPointerClickHandler
{
    [SerializeField] private Text itemSlotCountText;
    [SerializeField] Image thisSlotImage;
    [SerializeField] private int thisSlotItemType;
    [SerializeField] private GameObject equipText;
    private GameObject itemUI;
    private Text itemNameText;
    private Image itemImage;
    private RectTransform itemRectTransForm;
    [SerializeField] private Sprite InitImage;
    private GetInventoryData inventoryData;
    
    private GameObject equipAbleSlot;
    /// <summary>
    /// EquipItemInfo 를 집어 넣었놓는 변수 , 존재하면 해당 인벤토리의 위치가 변경되었을 때 , EquipItemInfo의 EquipItemInfo에 접근해 변경된 인벤토리 위치를 저장시켜줌
    /// </summary>
    public EquipItemInfo equipItemInfo;
    
    GameObject moveItemSlot;
    static bool isDrag;

    Sprite curImage;
    ItemPooling itemPooling;
    [SerializeField]private int slotCount;
    /// <summary>
    /// 해당 슬릇에 몇개가 들어가있는지 판단하는 변수 
    /// </summary>
    public int thisSlotCount {
        get {
            return slotCount;
        }
        set {
            if(value != 0) {
                itemSlotCountText.gameObject.SetActive(true);
            } 
            else {
                itemSlotCountText.gameObject.SetActive(false);
                ThisSlotItemType = 0;
            }
            slotCount = value;
            itemSlotCountText.text = slotCount.ToString();
        }
    }
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
                if(thisSlotItemType != 0 && ItemPooling.ItemPool.itemList.itemDatas[thisSlotItemType - 1].item_Type != "Postion"){
                     equipAbleSlot = transform.root.Find("InventoryANDstatus").Find("Equip").Find(ItemPooling.ItemPool.itemList.itemDatas[thisSlotItemType - 1].item_Type).gameObject;
                }
                thisSlotImage.sprite = ItemPooling.ItemPool.itemList.itemImages[thisSlotItemType - 1];
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
        itemPooling = ItemPooling.ItemPool;
        if (transform.parent.parent.Find("ItemUI"))
        {
            itemUI = transform.parent.parent.Find("ItemUI").gameObject;
            itemNameText = itemUI.transform.Find("Name").GetComponent<Text>();
            itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
            itemRectTransForm = itemUI.GetComponent<RectTransform>();
        }
        if(thisSlotItemType != 0 && thisSlotItemType != 4 && thisSlotItemType != 5){
            equipAbleSlot = transform.root.Find("InventoryANDstatus").Find("Equip").Find(ItemPooling.ItemPool.itemList.itemDatas[thisSlotItemType - 1].item_Type).gameObject;
        }
        inventoryData = transform.parent.parent.parent.GetComponent<GetInventoryData>();
        
        
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDrag || gameObject.GetComponent<ShowItemUi>().thisSlotItemType == 0)
        {
            return;
        }
        foreach (var type in ItemPooling.ItemPool.itemList.itemDatas)
        {
            if (type.item_id == thisSlotItemType)
            {

                itemNameText.text = "이름 : " + type.item_name;

                if (CheckWord(type.item_damage.ToString())) itemNameText.text += "\n데미지 : " + type.item_damage;
                if (CheckWord(type.HPrecovery.ToString())) itemNameText.text += "\n회복량 : " + type.HPrecovery;
                if (CheckWord(type.MPrecovery.ToString())) itemNameText.text += "\n회복량 : " + type.MPrecovery;
                if (CheckWord(type.item_defense.ToString())) itemNameText.text += "\n방어력 : " + type.item_defense;


                itemImage.sprite = ItemPooling.ItemPool.itemList.itemImages[type.item_id - 1];
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
            ShowItemUi hitShowItemUi = hit.collider.GetComponent<ShowItemUi>();
            if (hitShowItemUi.thisSlotItemType != 0) // 아이템 슬릇칸 -> 아이템 슬릇칸으로 옮길때 이미 해당 슬릇에 아이템이 존재할 때
            {
                if(itemPooling.itemList.itemDatas[hitShowItemUi.ThisSlotItemType - 1].item_Type == "Postion" &&  hitShowItemUi.ThisSlotItemType == ThisSlotItemType && hitShowItemUi.thisSlotCount + thisSlotCount < 99 ){
                    hitShowItemUi.thisSlotCount += thisSlotCount;
                    thisSlotCount = 0;
                    ThisSlotItemType = 0;
                    ChangeItemSlot(gameObject.name, new int[] {thisSlotItemType , thisSlotCount}, hitShowItemUi.gameObject.name, new int[] {hitShowItemUi.thisSlotItemType , hitShowItemUi.thisSlotCount});                
                    thisSlot.sprite = curImage;
                } 
                Destroy(moveItemSlot);
                
            }   
            else // 아이템 슬릇칸 -> 아이템 슬릇칸으로 옮길때 해당 슬릇이 비어있을 때
            {
                GameObject colliderObj = hit.collider.gameObject;
                hitShowItemUi.ThisSlotItemType = ThisSlotItemType;
                hitShowItemUi.thisSlotCount = thisSlotCount;
                hitShowItemUi.equipItem = equipItem;                
                equipItem = false;
                if(equipItemInfo != null){
                    hitShowItemUi.equipItemInfo = equipItemInfo;
                    equipItem = false;
                    equipItemInfo.EquipItemSlot = colliderObj;
                    equipItemInfo = null;
                }

                thisSlotCount = 0;
                ChangeItemSlot(gameObject.name, new int[] {thisSlotItemType , thisSlotCount}, colliderObj.name, new int[] {hitShowItemUi.thisSlotItemType , hitShowItemUi.thisSlotCount});
                Destroy(moveItemSlot);
            }
        }
        else // 아이템을 버릴 때;
        {
            Destroy(moveItemSlot);
            ChangeItemSlot(gameObject.name, new int[] {0, 0} , null, new int[] {0 ,0});
            inventoryData.itemsNumber[thisSlotItemType - 1] -= thisSlotCount;
            thisSlotCount = 0;
        }
    }
    bool EquipItemSlot(Vector3 ray, Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray, direction, Mathf.Infinity, LayerMask.GetMask("EquipUI"));

        if (hit.collider != null)
        {
            if( hit.collider.gameObject.name != ItemPooling.ItemPool.itemList.itemDatas[thisSlotItemType - 1].item_Type) {
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
   
            
            Destroy(moveItemSlot);
            return true;
        }

        return false;
    }
    private void ChangeItemSlot(string changedSlotKey, int[] changedSlotValue, string curSlotKey, int[] curSlotValue)
    {
        if(int.Parse(changedSlotKey) > 30){
            return;
        }
        
        
        SaveInvenData saveData = new SaveInvenData();
        saveData.id = Socket.Instance.this_player.name;
        saveData.Key = changedSlotKey;
        saveData.Value = changedSlotValue;
        HttpRequest.HttpRequests.Request("saveinventoryData", "item", JsonUtility.ToJson(saveData), (value) => Debug.Log("saveInventory"));
        if(curSlotKey == null){
            return;
        }
        saveData.Key = curSlotKey;
        saveData.Value = curSlotValue;
        HttpRequest.HttpRequests.Request("saveinventoryData", "item", JsonUtility.ToJson(saveData), (value) => Debug.Log("saveInventory"));
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

                        equipItemInfo.EquipItemSlot.GetComponent<ShowItemUi>().equipItem = false;
                        equipItemInfo.EquipItemSlot.GetComponent<ShowItemUi>().equipItemInfo = null;
                        
                        equipItemInfo.EquipItemSlot = null;

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
