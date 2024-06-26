using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
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
    string thisSlotName;

    private void Start() {
        itemUI = transform.parent.parent.Find("ItemUI").gameObject;
        itemNameText = itemUI.transform.Find("Name").GetComponent<Text>();
        itemDamageText = itemUI.transform.Find("Damage").GetComponent<Text>();
        itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
        itemRectTransForm = itemUI.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDrag){
            return;
        }
        foreach (var item in itemList.Items){
            if(item.name == this.gameObject.name){
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
        thisSlotName = gameObject.name;
        if(thisSlotImage == null){
            return;
        }
        gameObject.GetComponent<Image>().sprite = null;
        
        itemUI.SetActive(false);
        moveItemSlot = Instantiate(gameObject , gameObject.transform.parent.parent) as GameObject;
        moveItemSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(30f , 30f);
        moveItemSlot.GetComponent<Image>().sprite = thisSlotImage;
        moveItemSlot.GetComponent<Image>().raycastTarget = false;
        Destroy(moveItemSlot.GetComponent<BoxCollider2D>());
        moveItemSlot.layer = 5;
        moveItemSlot.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        Vector3 ray = (Vector3)eventData.position + (Vector3.back * 10f); 
        Vector3 direction = Vector3.forward * 100f;
        RaycastHit2D hit = Physics2D.Raycast(ray, direction, Mathf.Infinity);

        if(hit.collider != null){
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.GetComponent<Image>().sprite != null){
                Destroy(moveItemSlot);
                gameObject.GetComponent<Image>().sprite = thisSlotImage;
            }
            else {
                GameObject colliderObj = hit.collider.gameObject;
                colliderObj.GetComponent<Image>().sprite = thisSlotImage;
                colliderObj.name = thisSlotName;
                Destroy(moveItemSlot);
            }
        }else {
            Destroy(moveItemSlot);
            gameObject.GetComponent<Image>().sprite = thisSlotImage;
        }
        moveItemSlot = null;
        isDrag = false;
    }

}
