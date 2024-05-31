using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveUserNameText : MonoBehaviour
{
    public Transform moveObject;
    void Update(){
        gameObject.transform.position = Camera.main.WorldToScreenPoint(moveObject.transform.position) + Vector3.up * 50f;
    }
}
