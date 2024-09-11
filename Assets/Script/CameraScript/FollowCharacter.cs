using UnityEngine;
public class FollowCharacter : MonoBehaviour
{
    [Header("Camera Move State")]
    public float smooth;
    public Transform followCharacter;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
         if(followCharacter == null && GameObject.Find("Server").GetComponent<Socket>().this_player.transform != null){
             followCharacter = GameObject.Find("Server").GetComponent<Socket>().this_player.transform;
        }
        if(followCharacter != null){
             gameObject.transform.position += (followCharacter.position - gameObject.transform.position + Vector3.back * 10f) * Time.deltaTime * smooth;
        }
       
    }
}
