using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClearQuestTab : MonoBehaviour
{
    [SerializeField] Text clearGold;
    [SerializeField] Text clearExp;
    [SerializeField] Text clearItem;
    [SerializeField] private Quest quest;
    private QuestNPC questNPC;
    
    // Start is called before the first frame update
    void Awake()
    {
        questNPC = transform.parent.GetComponentInParent<QuestNPC>();   
        quest = questNPC.quest;  
    }

    private void OnEnable() {
        clearGold.text = $"Gold  {quest.QuestData.clearGold}\n";
        clearExp.text = $"Exp   {quest.QuestData.clearExp}";
        if(quest.QuestData.clearItem != 0) {
            clearItem.gameObject.SetActive(true);
            clearItem.transform.GetChild(0).GetComponent<Image>().sprite = ItemPooling.ItemPool.itemList.itemImages[quest.QuestData.clearItem - 1];
        }
        else clearItem.gameObject.SetActive(false);
    }
//player questclearnumber++ 로직 추가 필요
    public void ReceiveReward(){
        quest.progressQuest = false;
        quest.clear = false;
        ClearQuest clearQuest = new ClearQuest();
        clearQuest.title = "clearQuest";
        clearQuest.clearGold = quest.QuestData.clearGold;
        clearQuest.clearExp = quest.QuestData.clearExp;
        clearQuest.clearItem = quest.QuestData.clearItem;
        Socket.Instance.ws.Send(JsonUtility.ToJson(clearQuest));
        quest.QuestData = null;
        gameObject.SetActive(false);
    }
}
