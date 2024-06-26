using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemUi : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler , IDragHandler , IBeginDragHandler , IEndDragHandler
{
    [SerializeField] private GameObject itemUI;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDamageText;
    [SerializeField] private Image itemImage;
    [SerializeField] private ItemList itemList;
    [SerializeField] private RectTransform itemRectTransForm;
    GameObject moveItemSlot;
    static bool isDrag;
    Sprite thisSlotImage;
    public int thisSlotItemType;

    private void Start() {
        itemUI = transform.parent.parent.Find("ItemUI").gameObject;
        itemNameText = itemUI.transform.Find("Name").GetComponent<Text>();
        itemDamageText = itemUI.transform.Find("Damage").GetComponent<Text>();
        itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
        itemRectTransForm = itemUI.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDrag || gameObject.GetComponent<Image>().sprite == null){
            
            return;
        }
        foreach (var item in itemList.Items){
            if(item.GetComponent<SpriteRenderer>().sprite == GetComponent<Image>().sprite){
                SetItemInfo info = item.GetComponent<SetItemInfo>();
                itemNameText.text += info.name;
                itemDamageText.text += info.damage.ToString();
                itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                break;
            }
        }
        itemUI.transform.position = eventData.position + new Vector2( itemRectTransForm.rect.width / 2 , itemRectTransForm.rect.height / 2);
        itemUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemNameText.text = "이름 : ";
        itemDamageText.text = "데미지 : ";
        itemUI.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        moveItemSlot.transform.position = eventData.position;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        thisSlotImage = gameObject.GetComponent<Image>().sprite;
        
        if(thisSlotImage == null){
            return;
        }
        gameObject.GetComponent<Image>().sprite = null;
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
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.GetComponent<Image>().sprite != null){
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
            thisSlot.sprite = thisSlotImage;
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
