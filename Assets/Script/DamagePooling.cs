
using System.Collections.Generic;
using UnityEngine;

public class DamagePooling : MonoBehaviour
{
    private static DamagePooling damagePooling;
    public static DamagePooling Instance {
        get {
            if(damagePooling == null){
                return null;
            }
            return damagePooling;
        }
    }
    [SerializeField] private GameObject damagePrefeb;
    [SerializeField] private Camera main;
    Queue<GameObject> damagePool = new Queue<GameObject>();

    private void Awake() {
        damagePooling = this;
        main = Camera.main;
    }

    public static void ShowDamage(Vector2 showPos , int value){
        if(Instance.damagePool.Count > 0){
            GameObject damage = Instance.damagePool.Dequeue();
            damage.transform.position = Instance.main.WorldToScreenPoint(showPos)+ new Vector3(0f , 30f);
            damage.GetComponent<Damage>().Text = value.ToString();
            damage.SetActive(true);
        }
        else {
            GameObject createDamage = Instantiate(Instance.damagePrefeb , Instance.transform);
            createDamage.GetComponent<Damage>().Text = value.ToString();
            createDamage.transform.position = Instance.main.WorldToScreenPoint(showPos) + new Vector3(0f , 30f);
        }
    }
    public static void ReturnObject(GameObject damage){
        Instance.damagePool.Enqueue(damage);
        damage.SetActive(false);
    }
}
