using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class MoveObject : MonoBehaviour
{
    [Header("Charector Status")]
    public float speed;
    public float pushPower;
    public float moveX;
    public float moveY;
    public bool isAttackSussecs;
    public Collider2D attackingPlayer;
    [Space(9)]

    [Header("Character Name")]
    [SerializeField] private string userName;
    public Text text;

    [Space(9)]

    [Header("Attack Showing GameObject")] public GameObject attackShow;
       void Start(){
        text.text = this.gameObject.name;
    }
    public void Move(){
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        
        transform.position += new Vector3(moveX , moveY).normalized * speed * Time.deltaTime;
    }
    public IEnumerator Attack(){
        attackShow.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackShow.SetActive(false);
    } 

    public Vector2 GetPosition(){
        return transform.position;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            isAttackSussecs = true;
            attackingPlayer = other;
        }
    }
}
