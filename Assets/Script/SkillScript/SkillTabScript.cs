
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class SkillTabScript : MonoBehaviour
{
    [SerializeField] private SkillScript skillScript;
    public SkillScript SkillScript {
        get => skillScript;
        set {
            skillScript = value;
            SkillType = value != null ? skillScript.SkillData.skill_type : 0; 
            if(skillScript != null){
                skillDamage = skillScript.SkillData.skill_damage;
                skillCoolTime = skillScript.SkillData.skill_cooltime;
            }

        }
    }
    private int skillType;
    private float skillDamage;
    private float skillCoolTime;
    public int SkillType {
        get => skillType;
        set {
            skillType = value;
            skillImage.color = new Color(1, 1 , 1 , value);
            skillImage.sprite = GetSkillData.Instance.skillImage[skillType];
        }
    }
    private Image skillImage;
    [SerializeField] private Image skillCoolImage;
    [SerializeField] private string thisSlotCode;
    [SerializeField] private Text thisSlotCodeText;
    public void UseSkill()
    {
        if(skillScript.SkillData.coolDown <= 0) {
            skillScript.SkillData.coolDown = skillScript.SkillData.skill_cooltime;
            SkillPooling.Instance.ShowObject(Socket.Instance.this_player.transform , skillScript.SkillData.skill_type - 1);
            Data data = new Data("UseSkill");
            data.id = Socket.Instance.this_player.name;
            data.skillinfo = skillScript.SkillData.skill_type - 1;
            data.useItemType = skillScript.SkillData.mp_cost; // 사용한 스킬의 MP 소모량을 Data의 useItemType을 재사용
            Socket.Instance.ws.Send(JsonUtility.ToJson(data));
            Socket.Instance.this_player_MoveObject.useSkill = true;
        }
    }
    private void Update() { 
        
        if(Input.GetKeyDown((KeyCode) System.Enum.Parse(typeof(KeyCode) , thisSlotCode)) && skillScript != null && Socket.Instance.this_player_MoveObject.getUserData().mp >= skillScript.SkillData.mp_cost) {
            UseSkill();
            
        }
        if(skillScript != null && skillCoolTime > 0) {
            skillCoolImage.fillAmount = skillScript.SkillData.coolDown / skillScript.SkillData.skill_cooltime;
        } else if(skillScript == null){
            skillCoolImage.fillAmount = 0;
        }
        
    }
    void Awake()
    {
        skillImage = transform.Find("SkillTab").Find("Skill Image").GetComponent<Image>();
        skillImage.color = new Color(1, 1 , 1 , 0);
        thisSlotCodeText.text = thisSlotCode;
        
    }
    void Start() {
        gameObject.tag = "SkillTab";
    }
}
