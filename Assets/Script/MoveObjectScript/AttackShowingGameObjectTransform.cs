using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShowingGameObjectTransform : MonoBehaviour
{
    MoveObject moveObject;
    
    // Start is called before the first frame update
    void Awake()
    {
        moveObject = gameObject.transform.parent.GetComponent<MoveObject>();
        this.gameObject.SetActive(false);
    }

    void OnEnable() {
        gameObject.transform.localPosition = new Vector3(moveObject.moveX / 1.5f  , moveObject.moveY / 1.5f).normalized;
        
        float z = 0;
        if(moveObject.moveX == 0 || moveObject.moveY == 0){
            z = moveObject.moveX == 0 ? 90 : 0;
        }else {
            z = moveObject.moveX * moveObject.moveY == -1 ? -45f : 45f; 
        }

        gameObject.transform.rotation = Quaternion.Euler(0,0,z);
    }
}
