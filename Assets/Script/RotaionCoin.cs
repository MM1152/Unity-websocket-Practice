
using System.Collections;
using System.Linq.Expressions;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class RotaionCoin : MonoBehaviour
{
    Transform targetPos;
    float z = 0;   
    bool isThreading;
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
        if(isThreading){
            z += 5f;
            this.gameObject.transform.rotation = Quaternion.Euler(0 , 0, z);    
            this.gameObject.transform.position = movePos;
        }
        
        if(!isThreading){
            this.gameObject.transform.rotation = Quaternion.Euler(0 , 0, 0);  
        }

        if(targetPos != null){
            transform.position += (targetPos.position - transform.position).normalized * Time.deltaTime;
            if(Vector2.Distance(transform.position ,targetPos.position) < 0.5f){
                gameObject.SetActive(false);
                targetPos = null;
            }
        }
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
        isThreading = true;
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

            Thread.Sleep(10);
        }
        isThreading = false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            this.gameObject.SetActive(false);
        }    
    }
    
    public IEnumerator AbsorbCoin(Transform userPos){
        yield return new WaitForSeconds(1.3f);
        targetPos = userPos;
    }
}
