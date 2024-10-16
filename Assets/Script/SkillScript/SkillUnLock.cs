using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUnLock : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] GameObject succes; // 조건에 부합할 시 뛰어주는 창
    [SerializeField] GameObject fail;  // 조건에 부합하지 않을때 뛰어주는 창
    [SerializeField] Text unlockText;
    [SerializeField] SkillScript skillScript;
    GetInventoryData inventoryData;
    public int level;
    public int gold;
    private void Start(){
         inventoryData = GameObject.FindObjectOfType<GetInventoryData>();
         level = skillScript.SkillData.learnableLevel;
         gold = skillScript.SkillData.learnableGold;
         unlockText.text = $"캐릭터 레벨 {level}이상 / {gold}G";
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(Socket.Instance.this_player_MoveObject.UserData.Level >= level && gold <= GetSkillData.Instance.CheckGold()) {
            succes.SetActive(true);
        }   
        else {
            fail.SetActive(true);
        }       
    }
    public void CloseUI(GameObject offObject) {
        offObject.SetActive(false);
    }
    public void PurchaseSkill(){
        skillScript.Unlock = true;   
        SkillData sendSkillData = new SkillData();
        succes.SetActive(false);
        
        sendSkillData.skill_type = skillScript.SkillData.skill_type;
        sendSkillData.id = Socket.Instance.this_player.name;
        sendSkillData.learnableGold = skillScript.SkillData.learnableGold;
        //HttpRequest를 통해 해당하는 스킬을 구매했다고 서버로 데이터 전송 후 데이터 저장   
        HttpRequest.HttpRequests.Request("purchaseSkill" , "skillType" , JsonUtility.ToJson(sendSkillData) , (data) => DataInsertSussecs(data));
       
    }
    void DataInsertSussecs(string value){
        Debug.Log(value);
        HttpRequest.HttpRequests.Request("inventoryData", "id", Socket.Instance.this_player.name, (value) => inventoryData.ChangeMoney(value , null));
    }
}
