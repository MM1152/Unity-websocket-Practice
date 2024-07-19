using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    // Start is called before the first frame update
    Animator ani;
    private Text text;
    public string Text { 
        set{
            text.text = value;
        }
    }
    public Vector3 targetPos;

    private void Awake() {
        ani = GetComponent<Animator>();    
        text = GetComponent<Text>();
    }
    private void OnEnable() {
        StartCoroutine(EndAnimation());
    }
    private void Update() {
        
    }
    
    IEnumerator EndAnimation(){
        Debug.Log(ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        DamagePooling.ReturnObject(this.gameObject);
    }
}
