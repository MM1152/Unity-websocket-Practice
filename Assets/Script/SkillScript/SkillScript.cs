
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillScript : MonoBehaviour , IBeginDragHandler , IDragHandler , IPointerClickHandler , IEndDragHandler
{
    private SkillData skillData;
    public SkillData SkillData
    {
        get { return skillData; }
        set {
            skillData = value;
            texts[0].text = skillData.skill_info;
            texts[1].text = $"공격력의 {skillData.skill_damage * 100}% 로 공격합니다.";
        }
    }
    private GameObject copyObject;
    [SerializeField] GameObject SkillLock;
    public Text[] texts;
    private bool unLock;
    public bool Unlock{
        get => unLock;
        set {
            unLock = value;
            SkillLock.SetActive(false);
            texts[0].gameObject.SetActive(true);
            if(SkillData.skill_damage == 0) return;
            texts[1].gameObject.SetActive(true);
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Unlock){
            copyObject = Instantiate(this.gameObject , transform);
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(Unlock){
            copyObject.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 ray = (Vector3)eventData.position + (Vector3.back * 10f);
        Vector3 direction = Vector3.forward * 100f;

        RaycastHit2D hit = Physics2D.Raycast(ray , direction , Mathf.Infinity); 

        if(hit.collider != null) {
            if(hit.collider.tag == "SkillTab") {
                ISkill iSkill = hit.collider.GetComponent<ISkill>();

                if(SkillCoolTimeManager.thisSlotEquipSlot[hit.collider.transform.GetSiblingIndex().ToString()] == skillData.skill_type) { // 기존에 동일한 스킬이 장착되있다면 무시
                    Destroy(copyObject);
                    return;
                }
                hit.collider.GetComponent<SkillTabScript>().SkillScript = this;
                //hit.collider.GetComponent<ISkill>().SkillType = skillData.skill_type;
            
                if(SkillCoolTimeManager.thisSlotEquipSlot.ContainsValue(skillData.skill_type)){
                    var key = SkillCoolTimeManager.thisSlotEquipSlot.FirstOrDefault(x => x.Value == skillData.skill_type).Key;
                    hit.collider.transform.parent.GetChild(int.Parse(key)).GetComponent<SkillTabScript>().SkillScript = null;
                    //hit.collider.transform.parent.GetChild(int.Parse(key)).GetComponent<ISkill>().SkillType = 0;
                    SkillCoolTimeManager.thisSlotEquipSlot[key] = 0; // 이미 스킬슬릇에 장착되있는 스킬을 다시 장착할시 기존에 장착되있던 슬릇을 없애준다.
                }
            
                //hit.collider.GetComponent<ISkill>().SkillType = skillData.skill_type;
                SkillCoolTimeManager.thisSlotEquipSlot[hit.collider.transform.GetSiblingIndex().ToString()] = skillData.skill_type;
            }
        }
        Destroy(copyObject);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) {
            var equipSkill = SkillCoolTimeManager.thisSlotEquipSlot.FirstOrDefault(x => x.Value == skillData.skill_type).Key;
            Debug.Log(equipSkill);
            if(equipSkill != null) return;
            var nullSkillSlot = SkillCoolTimeManager.thisSlotEquipSlot.FirstOrDefault(x => x.Value == 0).Key;
            if(nullSkillSlot == null) return;
            else {
                SkillCoolTimeManager.thisSlotEquipSlot[nullSkillSlot] = skillData.skill_type;
                SkillTabScript skillTabScript = GameObject.Find("Skill").transform.GetChild(int.Parse(nullSkillSlot)).GetComponent<SkillTabScript>();
                skillTabScript.SkillScript = this;
            }
        }
    }
}
