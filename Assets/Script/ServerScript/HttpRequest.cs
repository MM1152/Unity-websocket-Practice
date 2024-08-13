using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Unity.VisualScripting.FullSerializer;

public class HttpRequest : MonoBehaviour
{
    private static HttpRequest httpRequests;
    public static HttpRequest HttpRequests {
        get {
            if (httpRequests == null) {
                return null;
            }
            return httpRequests;
        }
    }
    void Awake()
    {
        httpRequests = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Request(string url , string FiledName ,  string jsonData , Action<string> callback){
        url = "http://localhost:8001/" + url;
        StartCoroutine(Casting(url , FiledName , jsonData , callback));
    }
    public void Request(string url , string FiledName ,  string jsonData , Action<string> callback , Vector2 targetPos){
        url = "http://localhost:8001/" + url;
        StartCoroutine(Casting(url , FiledName , jsonData,callback , targetPos));
    }
    public IEnumerator Casting(string url , string FiledName ,  string jsonData , Action<string> callback){     
        WWWForm form = new WWWForm();
        form.AddField(FiledName , jsonData);
        UnityWebRequest request = UnityWebRequest.Post(url , form);

        yield return request.SendWebRequest();

         if (request.error == null)
        {
            string result = request.downloadHandler.text;
            callback(result);
            //callback(request.downloadHandler.text);
        }else {
            Debug.Log("Connection Fail");
        }
    }
    public IEnumerator Casting(string url , string FiledName ,  string jsonData , Action<string> callback , Vector2 targetPos){     
        Debug.Log(url);
        
        WWWForm form = new WWWForm();
        form.AddField(FiledName , jsonData);
        UnityWebRequest request = UnityWebRequest.Post(url , form);

        yield return request.SendWebRequest();

         if (request.error == null)
        {
            string result = request.downloadHandler.text;
            callback(result);
            Socket.Instance.this_player.transform.position = targetPos;
            //callback(request.downloadHandler.text);
        }else {
            Debug.Log("Connection Fail");
        }
    }
}
