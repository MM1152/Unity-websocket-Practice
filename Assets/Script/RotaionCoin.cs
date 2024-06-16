
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class RotaionCoin : MonoBehaviour
{
    float z = 0;   
    Thread thread;
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;
    Vector3 p4;
    Vector3 p5;
    Vector3 movePos;
    
    // Update is called once per frame
    void Update()
    {
        z += 1f;
        this.gameObject.transform.rotation = Quaternion.Euler(90f , z, 0f);    
        this.gameObject.transform.position = movePos;
    }
    void OnEnable(){
        p1 = this.gameObject.transform.position;
        p2 = this.gameObject.transform.position + new Vector3(0f , Random.Range(2f, 4f));
        p3 = this.gameObject.transform.position + new Vector3(Random.Range(-1f , 1f), Random.Range(0.1f, 0.5f));


        thread = new Thread(Bezier);
        thread.Start();
    }
    void Bezier()
    {
        float time = 0f;

        while(true)
        {
            if (time > 1f)
            {
                break;
            }

            p4 = Vector3.Lerp(p1, p2, time);
            p5 = Vector3.Lerp(p2, p3, time);
            
            movePos = Vector3.Lerp(p4, p5, time);
            time += 0.05f;

            Thread.Sleep(50);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            this.gameObject.SetActive(false);
        }    
    }
}
