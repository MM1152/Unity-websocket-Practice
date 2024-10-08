using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour {
    [SerializeField] Text id;
    [SerializeField] Text password;
    [SerializeField] Text signUpFailText;
    LoginData loginData = new LoginData();
    public void SignUp(){
        loginData.ID = id.text;
        loginData.Password = password.text;

        HttpRequest.HttpRequests.Request("signup" , "signData" , JsonUtility.ToJson(loginData) , (data) => MakeID(data));
    }

    public void MakeID(string Data){
        if(Data == "회원가입 성공") {
            signUpFailText.gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
        else if(Data == "회원가입 실패") {
            signUpFailText.gameObject.SetActive(true);
            signUpFailText.text = Data;
        }
    }
}