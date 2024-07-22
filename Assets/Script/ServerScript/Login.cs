using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text nick_name_text;
    public GameObject nick_name;
    public Text ID;
    public Text Password;
    LoginData loginData;
    HttpRequest httpRequest;
    public string data;
    // Start is called before the first frame update
    void Start()
    {
        httpRequest = HttpRequest.HttpRequests;
        loginData = new LoginData();
    }

    public void LoginButtonPush()
    {
        loginData.ID = ID.text;
        loginData.Password = Password.text;

        if (loginData.ID != null && loginData.Password != null)
        {
            string JsonData = JsonUtility.ToJson(loginData);
            httpRequest.Request("http://localhost:8001/login", "Login", JsonData, (value) => GetData(value));
        }
    }
    public void NickNameMakeButtonPush(){
        
        string jsonData = nick_name_text.text;
        Debug.Log(jsonData);
        if(jsonData != null){
            httpRequest.Request("http://localhost:8001/setNickName" , "NickName" , jsonData , (value) => GetData(value));
        }
    }
    public void GetData(string value)
    {
        data = value;
        Debug.Log(data);
        if(data.Equals("닉네임 입력.")){
            nick_name.SetActive(true);
        }
        if(data.Equals("로그인 성공.")){
            SceneManager.LoadScene("SampleScene");
            AudioManager.Instance.SetBgmSound(SoundClip.상점);
        }
    }

}

public class LoginData
{
    public string ID;
    public string Password;
}