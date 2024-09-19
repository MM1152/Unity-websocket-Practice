using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    
    [SerializeField] private Text questInfo;
    [SerializeField] private Text questExplanation;
    private Image image;
    public bool clearQuest;
    public QuestData questData;
    private void Start() {
        image = GetComponent<Image>();
        SettingQuest(questData);
    }

    public void SettingQuest(QuestData quest){
        image.color = Color.white;
        clearQuest = false;
        questData = quest;
        questInfo.text = quest.questinfo;
        questExplanation.text = quest.enemyType != 0 ? quest.questexplanation + $" {quest.count} / {quest.maxCount}" : quest.questexplanation;
    }

    public void ClearQuest(){
        questInfo.text = "";
        questExplanation.text = "Quest 클리어";
        image.color = Color.yellow;
        clearQuest = true;
    }
    private void FixedUpdate() {
        if(questData.enemyType != 0){
            questExplanation.text = questData.questexplanation + $" {questData.count} / {questData.maxCount}";
            if(questData.maxCount <= questData.count) ClearQuest();
        }
    }
}
