using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Socket socket;
    public GameObject followTarget;
    public float speed;
    // Update is called once per frame
    private void Start() {
        socket = Socket.Instance;
    }
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
            socket.this_player_MoveObject.HitEnemy(other.gameObject , socket.this_player_MoveObject.attack , socket.this_player_MoveObject.UserData);
            ProjectilePooling.Instance.ReturnObject(this);
        }
    }
    
}
