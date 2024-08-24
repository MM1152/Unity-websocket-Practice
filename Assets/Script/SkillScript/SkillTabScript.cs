
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class SkillTabScript : MonoBehaviour , ISkill
{
    [SerializeField] private SkillScript skillScript;
    public SkillScript SkillScript {
        get => skillScript;
        set {
            skillScript = value;
            SkillType = value != null ? skillScript.SkillData.skill_type : 0; 
            if(skillScript != null){
                SkillDamage = skillScript.SkillData.skill_damage;
                SkillCoolTime = skillScript.SkillData.skill_cooltime;
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
    public float SkillDamage {
        get => skillDamage;
        set {
            skillDamage = value;
        }
    }
    public float SkillCoolTime  {
        get => skillCoolTime;
        set {
            skillCoolTime = value;
        }
    }
    private Image skillImage;
    [SerializeField] private Image skillCoolImage;
    [SerializeField] private string thisSlotCode;
    [SerializeField] private Text thisSlotCodeText;
    public void UseSkill()
    {
        if(skillScript.SkillData.coolDown <= 0) {
            Debug.Log("스킬 리얼실행");
            skillScript.SkillData.coolDown = skillScript.SkillData.skill_cooltime;
            SkillPooling.Instance.ShowObject(Socket.Instance.this_player.transform.position , skillType - 1);
        }
    }
    private void Update() { 
        if(Input.GetKeyDown((KeyCode) System.Enum.Parse(typeof(KeyCode) , thisSlotCode)) && skillScript != null) {
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
}
