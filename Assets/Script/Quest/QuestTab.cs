using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class QuestTab : MonoBehaviour
{
    [SerializeField] Text questInfoText;
    [SerializeField] Text questExplanationText;
    [SerializeField] Text clearGold;
    [SerializeField] Text clearExp;
    [SerializeField] Text clearItem;
    
    [SerializeField] GameObject[] questNpcEffect; // 0 : 줄 수 있는 퀘스트가 있을때 , 1 : 퀘스트를 진행 중일때
    private GameManager gameManager;
    private NpcAi npc;
    private Quest quest;
    private QuestNPC questNPC;
    private QuestData questData;
    
    private void Awake() {
        npc = transform.parent.parent.GetComponent<NpcAi>();
        gameManager = GameManager.Instance;
        questNPC = GetComponentInParent<QuestNPC>();
        quest = questNPC.quest;
    }
    private void OnEnable() {
        questData = GameManager.Instance.quests.quests[Socket.Instance.this_player_MoveObject.UserData.clearquestnumber];   
        questInfoText.text = questData.questinfo;
        questExplanationText.text = questData.enemyType != 0 ? questData.questexplanation + " " + questData.maxCount + " 마리" : questData.questexplanation;
        clearGold.text = "Gold " + questData.clearGold;
        clearExp.text = "Exp " + questData.clearExp;
        if(questData.clearItem != 0) {
            clearItem.gameObject.SetActive(true);
            clearItem.text = "Item  ";
            clearItem.transform.GetChild(0).GetComponent<Image>().sprite = ItemPooling.ItemPool.itemList.itemImages[questData.clearItem - 1];
        }else {
            clearItem.gameObject.SetActive(false);
        }

    }
    //보상 탭 설계및 퀘스트 완료시 보상 지급 요청 구현 , 서버로 보상 데이터 넣어서 적용시킨뒤 다시 받아오면 될듯
    public void AcceptQuest(){
        if(questData != null) {
            questData.clearQuestTrans = GameObject.FindObjectOfType<NPCcount>().npc_List[questData.clearQuestNpc - 1].transform;
            quest.QuestData = questData;
            this.gameObject.SetActive(false);
        }
        
        //quest.SettingQuest();
        // 현재 플레이어의 퀘스트 단계에 맞춰 진행
        // quest.SettingQuest();
    }
    public void RefusalQuest(){
        this.gameObject.SetActive(false);
    }
}
