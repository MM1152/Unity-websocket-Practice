using System.Collections;
using UnityEngine;

public class Range : MonoBehaviour
{
    public float attackRadious = 0;
    public Transform target;
    private IMoveObj moveObj;
    private bool shoot;
    
    private void Start() {
        moveObj = GetComponent<IMoveObj>();
        attackRadious = moveObj.UserData.attackRadious;
    }
    public void CheckAttackPossible(){
        if(attackRadious >= Vector2.Distance(target.position ,transform.position)){
            StartCoroutine(WaitForShootinAnimation());
        }
    }
    IEnumerator WaitForShootinAnimation(){
        yield return new WaitUntil(() => moveObj.ani.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack") && moveObj.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7);
        ProjectilePooling.Instance.ShowObject(target , transform , 0);
    }
    private void OnDrawGizmos() {
        Gizmos.color = new Color(1/ 188 , 1/ 188 , 1/ 188 , 0.2f);
        Gizmos.DrawSphere(transform.position , attackRadious);
    }
}
