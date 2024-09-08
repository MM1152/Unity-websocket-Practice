using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text nick_name_text;
    [SerializeField] SelectCharacter selectCharacter; 
    public GameObject characterSelect;
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
            httpRequest.Request("login", "Login", JsonData, (value) => GetData(value));
        }
    }
    public void NickNameMakeButtonPush(){
        
        SigninData jsonData = new SigninData();
        jsonData.nick_name = nick_name_text.text;
        jsonData.characterType = selectCharacter.CenterType + 1;
        if(jsonData != null){
            httpRequest.Request("setNickName" , "NickName" , JsonUtility.ToJson(jsonData) , (value) => GetData(value));
        }
    }
    public void GetData(string value)
    {
        data = value;
        Debug.Log(data);
        if(data.Equals("닉네임 입력.")){
            characterSelect.SetActive(true);
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

public class SigninData 
{
    public string nick_name;
    public int characterType;
}