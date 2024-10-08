using System.Diagnostics;
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
    [SerializeField] GameObject clearQuestTab;
    [SerializeField] GameObject[] questNpcEffect = new GameObject[2];
    private void Start() {
        npc = GetComponent<NpcAi>();
        questTab = transform.Find("Canvas").Find("QuestTab").gameObject;
        clearQuestTab = transform.Find("Canvas").Find("ClearQuestTab").gameObject;
        questNpcEffect[0] = transform.Find("QuestMark").gameObject;
        questNpcEffect[1] = transform.Find("NoneQuestMark").gameObject;
    }

    private void FixedUpdate() {
        if(Socket.Instance.this_player == null) return;
        if(quest == null) quest = Socket.Instance.this_player.transform.Find("InteractionCanvas").Find("Quest").GetComponent<Quest>();
        SetNpcQuestEffect(quest.clear);
    }
    private void SetNpcQuestEffect(bool qusetFin){
        if(GiveQuest()) {
            questNpcEffect[0].SetActive(!qusetFin);
            questNpcEffect[1].SetActive(qusetFin);
        }else {
            questNpcEffect[0].SetActive(false);
            questNpcEffect[1].SetActive(false);
        }
  
    }
    private bool GiveQuest(){
        foreach(var quests in npc.NpcData.quest_type) {
            if(quests == Socket.Instance.this_player_MoveObject.getUserData().clearquestnumber + 1) {
                return true;
            }
        }
        return false;

    }
    private void Update() {
        if(npc.inPlayer && GiveQuest() && !quest.progressQuest) {
            if(Input.GetKeyDown(KeyCode.X)) questTab.SetActive(!questTab.activeSelf);
        }else if(npc.inPlayer && quest.clear) {
            if(Input.GetKeyDown(KeyCode.X)) clearQuestTab.SetActive(!clearQuestTab.activeSelf);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == Socket.Instance.this_player) {
            questTab.SetActive(false);
        }
    }
}
