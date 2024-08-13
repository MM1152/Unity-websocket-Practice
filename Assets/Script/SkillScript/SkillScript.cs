
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillScript : MonoBehaviour , IBeginDragHandler , IDragHandler , IPointerClickHandler , IEndDragHandler
{
    public SkillData skillData;
    private GameObject copyObject;
 // 해당 스킬이 장착 되어있는지 여부
    [SerializeField] GameObject SkillLock;
    public Text[] texts;
    private bool unLock;
    public bool Unlock{
        get => unLock;
        set {
            unLock = value;
            SkillLock.SetActive(false);
            texts[0].gameObject.SetActive(true);
            texts[1].gameObject.SetActive(true);
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        copyObject = Instantiate(this.gameObject , transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        copyObject.transform.position = eventData.position;
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
            var nullSkillSlot = SkillCoolTimeManager.thisSlotEquipSlot.FirstOrDefault(x => x.Value == 0).Key;
            if(nullSkillSlot == null) return;
            else {
                SkillCoolTimeManager.thisSlotEquipSlot[nullSkillSlot] = skillData.skill_type;
                ISkill iSkill = GameObject.Find("Skill").transform.GetChild(int.Parse(nullSkillSlot)).GetComponent<ISkill>();
                iSkill.SkillType = skillData.skill_type;
            }
        }
    }
}
