using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class SkillScript : MonoBehaviour , IBeginDragHandler , IDragHandler , IPointerClickHandler , IEndDragHandler
{
    private GameObject copyObject;
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
            hit.collider.GetComponent<ISkill>().skillType = slotType;

        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) {
            
        }
    }

    public void UseSkill()
    {
        throw new NotImplementedException();
    }
}
