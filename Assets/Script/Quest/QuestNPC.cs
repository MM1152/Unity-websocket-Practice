using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

/*
 1. 현재 플레이어 퀘스트 진행단계에 맞게 다음 퀘스트를 제시하도록 해주어야 함
 2. 현재 플레이어가 퀘스트를 진행중이라면 퀘스트를 전달해주면 안됌.
*/

public class QuestNPC : MonoBehaviour
{
    private NpcAi npc;
    public Quest quest { get ; private set; }
    [SerializeField] GameObject questTab;
    private void Start() {
        npc = GetComponent<NpcAi>();
                
    }

    private void FixedUpdate() {
        if(Socket.Instance.this_player == null) return;
        if(quest == null) quest = Socket.Instance.this_player.transform.Find("InteractionCanvas").Find("Quest").GetComponent<Quest>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == Socket.Instance.this_player) {
            if(Input.GetKeyDown(KeyCode.X)) questTab.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == Socket.Instance.this_player) {
            if(Input.GetKeyDown(KeyCode.X)) questTab.SetActive(false);
        }
    }
}
