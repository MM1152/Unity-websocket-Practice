using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveUserNameText : MonoBehaviour
{
    public Transform moveObject;
    public float yPos;
    void Update(){
        gameObject.transform.position = Camera.main.WorldToScreenPoint(moveObject.transform.position) + Vector3.up * yPos;
    }
}
