using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject followTarget;
    public float speed;
    // Update is called once per frame
    private void OnEnable() {
        Vector2 dir = followTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y , dir.x) * Mathf.Rad2Deg;  
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        if( followTarget != null ) {
            transform.position += (followTarget.transform.position - transform.position) * speed * Time.deltaTime;
        } 
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == followTarget) {
            ProjectilePooling.Instance.ReturnObject(this);
            Data data = new Data("HitEnemy");
            data.id = other.name.Split(' ')[1];
            data.this_player = Socket.Instance.this_player_MoveObject.UserData;
            Socket.Instance.ws.Send(JsonUtility.ToJson(data));
        }
    }
    
}
