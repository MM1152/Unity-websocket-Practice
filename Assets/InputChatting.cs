using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChatting : MonoBehaviour
{
    [SerializeField] InputField input;
    void Update()
    {
        
        if(input.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Return) && input.text != null && input.text != ""){
            Data chattingData = new Data();
            chattingData.title = "chatting";
            chattingData.id = Socket.Instance.this_player.name;
            chattingData.chattingText = input.text;
            Socket.Instance.ws.Send(JsonUtility.ToJson(chattingData));
        }
        if(Input.GetKeyDown(KeyCode.Return)) {
            input.gameObject.SetActive(!input.gameObject.activeSelf);
            if(input.gameObject.activeSelf == true) {
                input.text = "";
                input.ActivateInputField(); // Input Filed로 포커스 이동
            }
            
        }

    }
}
