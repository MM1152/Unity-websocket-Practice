using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemInfo : MonoBehaviour
{
    GameObject equipItemSlot;
    public GameObject EquipItemSlot{
        get {
            return equipItemSlot;
        }
        set {
            equipItemSlot = value;
        }
    }
    ShowItemUi showItemUi;
    Image image;
    int thisSlotItemType;
    
    public int ThisSlotItemType{
        get {
            return thisSlotItemType;
        }
        set {
            thisSlotItemType = value;

            if(thisSlotItemType != 0) image.color = new Color(1f , 1f , 1f , 1f);
            else image.color = new Color(1f , 1f , 1f , 0f);

            SaveInvenData saveData = new SaveInvenData();
            saveData.id = Socket.Instance.this_player.name ;
            saveData.Key = this.gameObject.name;
            saveData.Value = value;

            StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/saveEquipItem" , "Equip" , JsonUtility.ToJson(saveData) , (value) => Debug.Log($"Save Equip Data\n Data Key : {saveData.Key}\n Data Value : {saveData.Value}")));
        }
    }
    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        showItemUi = transform.GetChild(0).GetComponent<ShowItemUi>();
    } 
    private void Update() {
        if(showItemUi.ThisSlotItemType != ThisSlotItemType){
            ThisSlotItemType = showItemUi.ThisSlotItemType;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ItemTab"){
            Debug.Log("Trigging");
        }
    }
}
