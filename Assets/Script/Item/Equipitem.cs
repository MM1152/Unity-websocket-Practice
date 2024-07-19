using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipitem : MonoBehaviour
{
    [SerializeField] private int[] thisSlotItemType;
    [SerializeField] private GameObject ItemTab;
    private ItemPooling itemPooling;

    // Start is called before the first frame update
    void Start() {
        itemPooling = ItemPooling.Instance;
    }
    void Awake()
    {
        for(int i = 0; i < 8; i++){
            GameObject itemPrefeb =  Instantiate(ItemTab , gameObject.transform.GetChild(i).transform);
            itemPrefeb.transform.position = gameObject.transform.GetChild(i).transform.position;
            itemPrefeb.GetComponent<Image>().color = new Color(1,1,1,0);
            itemPrefeb.GetComponent<RectTransform>().sizeDelta = new Vector2(20f , 20f);
            itemPrefeb.tag = "EquipItem";
        }
        this.gameObject.SetActive(false);
    }
}
