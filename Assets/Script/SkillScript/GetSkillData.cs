
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GetSkillData : MonoBehaviour
{
    [SerializeField] private GameObject skillprefeb;
    private static GetSkillData instance;
    [SerializeField] public SkillDatas skillDatas;
    [SerializeField] private Transform skillTabTransform;
    [SerializeField] private GetInventoryData getInventoryData;
    [SerializeField] SkillLearnData skillLearn;
    public static GetSkillData Instance{
        get {
            if(instance == null) {
                instance = FindObjectOfType<GetSkillData>();
            }
            return instance;
        }
    }
    public List<Sprite> skillImage;
    private void Awake() {
        
        skillImage = Resources.LoadAll<Sprite>("SkillImage").ToList();
        skillImage.Add(null);
        for(int i = skillImage.Count - 2; i >= 0; i--) {
            Debug.Log(skillImage[i].name);
            skillImage[i + 1] = skillImage[i];
        }
        skillImage[0] = null;
    }
    private void Start() {
        instance = this;
        getInventoryData = transform.parent.GetComponent<GetInventoryData>();
        HttpRequest.HttpRequests.Request("getSkillData" , "needSkill" , JsonUtility.ToJson(Socket.Instance.this_player_MoveObject.getUserData()) , (value) => SetSkill(value) );
        
    }

    public void SetSkill(string skillData){
        skillDatas = JsonUtility.FromJson<SkillDatas>(skillData);
        skillLearn = JsonUtility.FromJson<SkillLearnData>(skillData);
        
      
        for(int i = 0; i < skillDatas.skillsData.Length; i++){
            GameObject skill = Instantiate(skillprefeb , skillTabTransform);
            skill.GetComponent<Image>().sprite = skillImage[skillDatas.skillsData[i].skill_type];

            SkillCoolTimeManager.skillData.Add(skillDatas.skillsData[i]);
            skill.GetComponent<SkillScript>().SkillData = SkillCoolTimeManager.skillData[i];

            if(skillLearn.skillLearn.Length > 0) {
                foreach(int j in skillLearn.skillLearn[0].learned_skill){
                    if(j == skillDatas.skillsData[i].skill_type) {
                        skill.GetComponent<SkillScript>().Unlock = true;
                        break;
                    }
                } 
            }
           
            
            
        }
        //서버에서 받은 데이터 중 , UnLock부분 데이터에 존재하는 스킬들은 UnLock = True; 시켜줌
    }
    public int CheckGold(){
        return getInventoryData.gold;
    }
    
}
