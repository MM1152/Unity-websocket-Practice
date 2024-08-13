
using UnityEngine;
using UnityEngine.UI;

public class GetSkillData : MonoBehaviour
{
    [SerializeField] private SkillCoolTimeManager skillCoolTimeManager;
    [SerializeField] private GameObject skillprefeb;
    private static GetSkillData instance;
    [SerializeField] public SkillDatas skillDatas;
    [SerializeField] private Transform skillTabTransform;
    public static GetSkillData Instance{
        get {
            if(instance == null) {
                instance = FindObjectOfType<GetSkillData>();
            }
            return instance;
        }
    }
    public Sprite[] skillImage;
    private void Awake() {
        instance = this;
        HttpRequest.HttpRequests.Request("getSkillData" , "needSkill" , "1" , (value) => SetSkill(value) );
        skillCoolTimeManager = GetComponent<SkillCoolTimeManager>();
    }

    public void SetSkill(string skillData){
        skillDatas = JsonUtility.FromJson<SkillDatas>(skillData);

        for(int i = 0; i < skillDatas.skillsData.Length; i++){
            GameObject skill = Instantiate(skillprefeb , skillTabTransform);
            skill.GetComponent<Image>().sprite = skillImage[skillDatas.skillsData[i].skill_type];
            skill.GetComponent<SkillScript>().skillData = skillDatas.skillsData[i];
            
        }
    }

    
}
