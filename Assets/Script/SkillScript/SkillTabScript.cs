
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
            SkillType = value != null ? skillScript.skillData.skill_type : 0; 
        }
    }
    private int skillType;
    private int skillDamage;
    private float skillCoolTime;
    public int SkillType {
        get => skillType;
        set {
            skillType = value;
            skillImage.color = new Color(1, 1 , 1 , value);
            skillImage.sprite = GetSkillData.Instance.skillImage[skillType];
        }
    }
    public int SkillDamage {
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
    [SerializeField] private Image skillCool;

    public void UseSkill()
    {
        /*MoveObject에서 호출할 예정*/
        if(skillScript.skillData.coolDown <= 0) {
            skillScript.skillData.coolDown = skillScript.skillData.skill_cooltime;
            /*스킬 애니메이션 재생*/
        }
    }
    private void Update() { 
        if(Input.GetKeyDown(KeyCode.Q) && skillScript != null) {
            UseSkill();
        }
        if(skillScript != null && skillScript.skillData.coolDown > 0) {
            skillImage.fillAmount = 1 / skillScript.skillData.coolDown;
        }
    }
    void Awake()
    {
        skillImage = transform.Find("SkillTab").Find("Skill Image").GetComponent<Image>();
        skillImage.color = new Color(1, 1 , 1 , 0);
    }
}
