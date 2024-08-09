using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class SkillTabScript : MonoBehaviour , ISkill
{
    private float coolTime;
    private int skillType;
    private int skillDamage;
    private int skillCoolTime;
    public int SkillType {
        get => skillType;
        set {
            skillType = value;
            /*스킬 타입에 맞춰 이미지 변경*/
            /*아니면 스킬 이미지 자체를 복사해버림?*/
        }
    }
    public int SkillDamage {
        get => skillDamage;
        set {
            skillDamage = value;
        }
    }
    public int SkillCoolTime  {
        get => skillCoolTime;
        set {
            skillCoolTime = value;
        }
    }
    private Image skillImage;

    public void UseSkill()
    {
        /*MoveObject에서 호출할 예정*/
        if(coolTime <= 0) {
            coolTime = skillCoolTime;
            /*스킬 애니메이션 재생*/
        }
    }

    void Awake()
    {
        skillImage = GetComponent<Image>();
    }
    void Update(){
        if(skillType != 0 && coolTime > 0){
            coolTime -= Time.deltaTime;
        }
    }

}
