using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GetInventoryData inventoryData;
    public Transform targetPos;

    private bool isPickUp;
    private void OnEnable() {
        isPickUp = false;
    }
    private void Awake() {
        inventoryData = GameObject.FindObjectOfType<GetInventoryData>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && targetPos.gameObject == other.gameObject && Input.GetKeyDown(KeyCode.X) && !isPickUp)
        {
            bool insert = inventoryData.SetInventory(GetComponent<SetItemInfo>().ItemIndex , this.gameObject);
            if (insert) Debug.Log("Fail To Get DropItem");
            ItemPooling.Instance.ReturnObject(this);
            isPickUp = true;
        }
    }

    public void SetTarget(Transform userPos)
    {
        targetPos = userPos;
    }
}