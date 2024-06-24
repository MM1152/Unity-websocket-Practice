using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GetInventoryData inventoryData;
    public Transform targetPos;
    private bool isPickUp;

    private void Awake() {
        inventoryData = GameObject.Find("InventoryANDstatus").GetComponent<GetInventoryData>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && targetPos.gameObject == other.gameObject && Input.GetKey(KeyCode.X) && !isPickUp)
        {
            inventoryData.SetInventory(GetComponent<SetItemInfo>().type , this.gameObject);
            isPickUp = true;
        }
    }

    public void SetTarget(Transform userPos)
    {
        targetPos = userPos;
    }
}