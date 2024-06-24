using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class AttackShowingGameObjectTransform : MonoBehaviour
{
    public MoveObject moveObject;
    Vector2 attackPosition;
     float z = 0;
    // Start is called before the first frame update
    void Awake()
    {
        
        this.gameObject.SetActive(false);
    }

    void OnEnable() {
        moveObject ??= gameObject.transform.parent.GetComponent<MoveObject>();
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
