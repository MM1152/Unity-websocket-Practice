using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveUi : MonoBehaviour, IDragHandler , IBeginDragHandler
{
    private Vector2 movePos;
    public void OnBeginDrag(PointerEventData eventData)
    {
       movePos = eventData.position;   
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0)){
            this.gameObject.transform.position = transform.position + (Vector3)(eventData.position - movePos); 
            movePos = eventData.position;
        }
    }
}
