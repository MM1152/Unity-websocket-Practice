
using Newtonsoft.Json;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

public class GetInventoryData : MonoBehaviour
{
    [SerializeField] private ItemPooling itemPooling;
    [SerializeField] private GameObject itemTab;
    [SerializeField] private Transform inventorySize;
    [SerializeField] private ItemList itemList;
    [SerializeField] private InventoryData inven;
    [SerializeField] private Text gold;
    public int[] itemsNumber;
    HttpRequest httpRequest;
    Socket socket;
    SaveInvenData saveData;
    int slotIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        itemPooling = ItemPooling.Instance;
        socket = Socket.Instance;
        saveData = new SaveInvenData();
        httpRequest = HttpRequest.HttpRequests;
        saveData.id = socket.this_player.name;
        itemList = itemPooling.itemList;
        itemsNumber = new int[itemList.itemDatas.Count];
        httpRequest.Request("http://localhost:8001/inventoryData", "id", socket.this_player.name, (value) => InitInventory(value));
    }
    
    public void ChangeMoney(string Data , GameObject offGameObject){
        inven = JsonConvert.DeserializeObject<InventoryData>(Data);
        gold.text = "Gold : " + inven.gold.ToString();
        offGameObject.SetActive(false);
    }

    public void InitInventory(string Data)
    {
        inven = JsonConvert.DeserializeObject<InventoryData>(Data);
        Debug.Log(inven.gold);
        gold.text = "Gold : " + inven.gold.ToString();

        if(inventorySize.transform.childCount != 0){
            return;
        }

        SetInventoryItem(inven);
        SetEquipItem(inven);
        SetEquipItemTab(inven);
    }
    public void SetInventory(int item , GameObject thisItem)
    {
        bool setPostion = false;
        if(itemPooling.itemList.itemDatas[item - 1].item_Type == "Postion"){
            if (itemsNumber[item - 1] != 0){
                setPostion = true;
            }
        }
        for (int i = 0; i < 30; i++)
        {
            ShowItemUi showItemUi = inventorySize.GetChild(i).GetComponent<ShowItemUi>();
            if (showItemUi.ThisSlotItemType == 0 && !setPostion)
            {
                showItemUi.ThisSlotItemType = item;
                showItemUi.thisSlotCount++;
                itemsNumber[item - 1]++;
                saveData.Key = (i + 1).ToString();
                saveData.Value[0] = item;
                saveData.Value[1] = 1;
                string jsonData = JsonUtility.ToJson(saveData);
                httpRequest.Request("http://localhost:8001/saveinventoryData", "item", jsonData , (value) => PickUpItem(thisItem));  
                break;
            }
            else if(showItemUi.ThisSlotItemType == item && setPostion){
                showItemUi.thisSlotCount++;
                itemsNumber[item - 1]++;
                saveData.Key = (i + 1).ToString();
                saveData.Value[0] = item;
                saveData.Value[1] = showItemUi.thisSlotCount;
                string jsonData = JsonUtility.ToJson(saveData);
                httpRequest.Request("http://localhost:8001/saveinventoryData", "item", jsonData , (value) => PickUpItem(thisItem));  
                break;
            }
            
        }
    }
    void PickUpItem(GameObject thisItem){
        if(thisItem != null){
            thisItem.SetActive(false);
        }
    }
    /// <summary>
    /// 장착한 아이템 정보 설정
    /// </summary>
    /// <param name="inven">서버로부터 받아온 장착 정보데이터</param>
    private void SetEquipItem(InventoryData inven){
        foreach(var equip in inven.equip.Keys){
            transform.Find("Equip").Find(equip).transform.GetChild(0).GetComponent<ShowItemUi>().ThisSlotItemType = inven.equip[equip];
        }
    }
    /// <summary>
    /// 인벤토리에서 장착된 아이템 정보 설정
    /// </summary>
    /// <param name="inven">서버로부터 받아온 장착된 아이템인벤토리 슬릇</param>
    private void SetEquipItemTab(InventoryData inven){
        
        foreach(var equipItemTab in inven.equipItemTab.Keys) {
            if(inven.equipItemTab[equipItemTab] != 0){
                ShowItemUi equipItemSlot = inventorySize.GetChild(inven.equipItemTab[equipItemTab] - 1).gameObject.GetComponent<ShowItemUi>();
                EquipItemInfo equipitemInfo = transform.Find("Equip").Find(equipItemTab).GetComponent<EquipItemInfo>();
                equipItemSlot.equipItem = true;
                equipItemSlot.equipItemInfo = equipitemInfo;
                equipitemInfo.equipItemSlot = equipItemSlot.gameObject;
            }
        }
    }
    /// <summary>
    /// 인벤토리 데이터 설정
    /// </summary>
    /// <param name="inven">서버로부터 받아온 인벤토리 데이터</param>
    private void SetInventoryItem(InventoryData inven){
        foreach (var item in inven.item.Keys)
        {
            GameObject createItem = Instantiate(itemTab, inventorySize);
            ShowItemUi createItemShowItemUI = createItem.GetComponent<ShowItemUi>();
            createItem.name = slotIndex++.ToString();  
            createItemShowItemUI.ThisSlotItemType = 0;
            if (inven.item[item][0] == 0)
            {
                continue;
            }
            createItemShowItemUI.thisSlotCount = inven.item[item][1];
            createItemShowItemUI.ThisSlotItemType = inven.item[item][0];

            itemsNumber[inven.item[item][0] - 1] += createItemShowItemUI.thisSlotCount;
        }
    }
    public ShowItemUi FindPostionSlot(int index){
        for(int i = 0; i < inventorySize.childCount; i++){
            if(inventorySize.GetChild(i).GetComponent<ShowItemUi>().ThisSlotItemType == index) {
                return inventorySize.GetChild(i).GetComponent<ShowItemUi>();
            }
        }
        return null;
    }
}
