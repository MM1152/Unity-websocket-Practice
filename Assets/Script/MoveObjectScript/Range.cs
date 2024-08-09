using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Range : MonoBehaviour
{
    public float attackRadious = 0;
    public Transform target;
    private IMoveObj moveObj;
    private bool IsAttack;
    private void Start() {
        moveObj = GetComponent<IMoveObj>();
        attackRadious = moveObj.UserData.attackRadious;
    }
    public void CheckAttackPossible(){
        if(attackRadious >= Vector2.Distance(target.position ,transform.position)){
            ProjectilePooling.Instance.ShowObject(target , transform , 0);
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = new Color(1/ 188 , 1/ 188 , 1/ 188 , 0.2f);
        Gizmos.DrawSphere(transform.position , attackRadious);
    }
}
