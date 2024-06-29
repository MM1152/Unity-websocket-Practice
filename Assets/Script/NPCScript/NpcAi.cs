using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcAi : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    [SerializeField] Text text;
    [SerializeField] private NPCData npcData;
    [SerializeField] private GameObject shop;
    [SerializeField] private MakeShopItem makeShopItem;
    bool inPlayer;
    public NPCData NpcData{
        get {
            return npcData;
        }
        set {
            npcData = value;
            text.text = npcData.talk;
            if(makeShopItem == null){
                makeShopItem = GetComponent<MakeShopItem>();
                makeShopItem.MakeShop();

            }
        }
    }
    private void Update() {
        if(inPlayer){
            if(Input.GetKeyDown(KeyCode.X)) shop.SetActive(!shop.activeSelf);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && Socket.Instance.this_player == other.gameObject){
            textObj.SetActive(true);
            inPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
         if(other.tag == "Player" && Socket.Instance.this_player == other.gameObject){
            textObj.SetActive(false);
            shop.SetActive(false);
            inPlayer = false;
         }
    }
}
