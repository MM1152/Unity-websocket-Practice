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

    public IEnumerator Request(string url , string FiledName ,  string jsonData , Action<string> callback){     
        Debug.Log(url);
        
        WWWForm form = new WWWForm();
        form.AddField(FiledName , jsonData);
        UnityWebRequest request = UnityWebRequest.Post(url , form);

        yield return request.SendWebRequest();

         if (request.error == null)
        {
            string result = request.downloadHandler.text;
            Debug.Log("StartCallback : " + callback.ToString());
            callback(result);
            //callback(request.downloadHandler.text);
        }else {
            Debug.Log("Connection Fail");
        }
    }
        public IEnumerator Request(string url , string FiledName ,  string jsonData , Action<string> callback , Vector2 targetPos){     
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
