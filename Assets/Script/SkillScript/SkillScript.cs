using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class SkillScript : MonoBehaviour , IBeginDragHandler , IDragHandler , IPointerClickHandler , IEndDragHandler
{
    public static bool skillEquip;
    private GameObject copyObject;
    private static Dictionary<string , int> thisSlotEquipSlot = new Dictionary<string, int>() {
        { "0", -1 },
        { "1", -1 },
        { "2", -1 },
        { "3", -1 },
        { "4", -1 },
        { "5", -1 },
        { "6", -1 },
        { "7", -1 },
    } ; // 해당 스킬이 장착 되어있는지 여부
    [SerializeField] int slotType;
    [SerializeField] GameObject SkillLock;
    [SerializeField] Text[] texts;
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

        if(hit.collider.tag == "SkillTab") {
            ISkill iSkill = hit.collider.GetComponent<ISkill>();
            if(thisSlotEquipSlot[hit.collider.transform.GetSiblingIndex().ToString()] == iSkill.SkillType) return; // 기존에 동일한 스킬이 장착되있다면 무시
            if(thisSlotEquipSlot.ContainsValue(slotType)) thisSlotEquipSlot[thisSlotEquipSlot.FirstOrDefault(x => x.Value == slotType).Key] = -1; // 이미 스킬슬릇에 장착되있는 스킬을 다시 장착할시 기존에 장착되있던 슬릇을 없애준다.
            
            hit.collider.GetComponent<ISkill>().SkillType = slotType;
            thisSlotEquipSlot[hit.collider.transform.GetSiblingIndex().ToString()] = slotType;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) {
            var nullSkillSlot = thisSlotEquipSlot.FirstOrDefault(x => x.Value == -1).Key;
            if(nullSkillSlot == null) return;
            else {
                ISkill iSkill = GameObject.Find("Skill").transform.GetChild(int.Parse(nullSkillSlot)).GetComponent<ISkill>();
                iSkill.SkillType = slotType;
            }
        }
    }
}
