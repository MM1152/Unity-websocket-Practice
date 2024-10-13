using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChattingSystem : MonoBehaviour
{
    [SerializeField] Text text;
    RectTransform rect;
    StringBuilder sb = new StringBuilder();
    
    public string chattingText {
        set {
            sb.AppendLine(value);
            text.text = sb.ToString();

            rect.sizeDelta += new Vector2(0 , 10);
            Debug.Log($"Input sb : {sb.Length}");
            
            Debug.Log($"문자열의 길이 :{value.Length}");
            StartCoroutine(DeleteChatCorutine(value.Length + 2));
        }
    }
    private void Awake() {
        rect = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    private IEnumerator DeleteChatCorutine(int Count){
        yield return new WaitForSeconds(2f);
        sb.Remove(0 , Count);
        Debug.Log($"remove sb : {sb.Length}");
        rect.sizeDelta -= new Vector2(0 , 10);
        text.text = sb.ToString();
    }
    private void Update() {
        if(sb.Length <= 0) gameObject.SetActive(false);
    }
}
