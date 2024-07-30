
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class DamagePooling : PoolingManager<Damage>
{
    Camera main;
    public override void Awake() {
        base.Awake();
        main = Camera.main;
    }

    public override void ShowObject(Vector2 showPos , int value){
        Damage damage;
        if(pooling.Count > 0){
            damage = pooling.Dequeue();
        }
        else {
            damage = Instantiate(prefab , transform);
        }
        damage.transform.position = main.WorldToScreenPoint(showPos)+ new Vector3(0f , 30f);
        damage.GetComponent<Damage>().Text = value.ToString();
        damage.gameObject.SetActive(true);
    }
}
