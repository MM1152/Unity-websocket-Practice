using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class AttackShowingGameObjectTransform : MonoBehaviour
{
    MoveObject moveObject;
    Vector2 attackPosition;
    
    // Start is called before the first frame update
    void Awake()
    {
        moveObject = gameObject.transform.parent.GetComponent<MoveObject>();
        this.gameObject.SetActive(false);
    }

    void OnEnable() {
        float z = 0;

        if(moveObject.sp.flipX){
            attackPosition = new Vector2(-1f , 0f);
            z = 180f;
        } else {
            attackPosition = new Vector2(1f , 0f);
            z = 0f;
        }
        gameObject.transform.localPosition = attackPosition;
        gameObject.transform.rotation = Quaternion.Euler(0,z,0);
    }
}
