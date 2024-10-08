using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcAi : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    [SerializeField] Text text;
    [SerializeField] private NPCData npcData;
    public bool inPlayer;
    public NPCData NpcData{
        get {
            return npcData;
        }
        set {
            npcData = value;
            text.text = npcData.talk;
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
            inPlayer = false;
         }
    }
}
