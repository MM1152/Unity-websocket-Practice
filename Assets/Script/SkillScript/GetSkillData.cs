
using UnityEngine;
using UnityEngine.UI;

public class GetSkillData : MonoBehaviour
{
    [SerializeField] private GameObject skillprefeb;
    private static GetSkillData instance;
    [SerializeField] public SkillDatas skillDatas;
    [SerializeField] private Transform skillTabTransform;
    [SerializeField] private GetInventoryData getInventoryData;
    public static GetSkillData Instance{
        get {
            if(instance == null) {
                instance = FindObjectOfType<GetSkillData>();
            }
            return instance;
        }
    }
    public Sprite[] skillImage;
    private void Start() {
        instance = this;
        getInventoryData = transform.parent.GetComponent<GetInventoryData>();
        HttpRequest.HttpRequests.Request("getSkillData" , "needSkill" , Socket.Instance.this_player_MoveObject.getUserData().type.ToString() , (value) => SetSkill(value) );
    }

    public void SetSkill(string skillData){
        skillDatas = JsonUtility.FromJson<SkillDatas>(skillData);

        for(int i = 0; i < skillDatas.skillsData.Length; i++){
            GameObject skill = Instantiate(skillprefeb , skillTabTransform);
            skill.GetComponent<Image>().sprite = skillImage[skillDatas.skillsData[i].skill_type];
            
            SkillCoolTimeManager.skillData.Add(skillDatas.skillsData[i]);
            skill.GetComponent<SkillScript>().SkillData = SkillCoolTimeManager.skillData[i];
        }
    }
    public int CheckGold(){
        return getInventoryData.gold;
    }
    
}
