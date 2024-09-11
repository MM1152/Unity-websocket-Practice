
using UnityEngine;


public class CameraMoveLimit : MonoBehaviour
{
    Vector2 limit;
    float height;
    float width;
    private void Awake() {
        height = Camera.main.orthographicSize;    
        width = height * Screen.width / Screen.height;
    }
    public void SetCameraLimit(Vector2 limit){
        this.limit = limit;
    }
    private void FixedUpdate() {
        float dx = limit.x - width;
        float dy = limit.y - height;
        if(limit != null) {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x , -dx -0.5f, dx -0.5f ) , Mathf.Clamp(transform.position.y , -dy -0.5f, dy + 0.5f) , -10f);
        }
    }
}
