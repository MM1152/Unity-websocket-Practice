using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public class PostionUI : MonoBehaviour
{
    public float coolTime;
    [SerializeField] private Text postionNumberText;
    [SerializeField] private GameObject autoAnimationGameObject;
    [SerializeField] private RectTransform coolDownRect;
    [SerializeField] private GetInventoryData getInventoryData;
    private GameManager gameManager;
    private bool postionCoolTime;
    private Socket socket;
    private int posionNum;
    public int PosionNum {
        get {
            return posionNum;
        }
        set {
            posionNum = value;
            postionNumberText.text = value.ToString();
        }
    }
    private void Start() {
        socket = Socket.Instance;
        gameManager = GameManager.Instance;
    }
    public void Update(){
        UsePostion();
    }
    public void UsePostion(){
        
        float value = gameObject.name == "HPpostion" ? gameManager.HPrecoveryValue : gameManager.MPrecoveryValue;
       
        if(value * 100f >= 
         socket.this_player_MoveObject.UserData.hp / (float)socket.this_player_MoveObject.UserData.maxhp * 100f && !postionCoolTime) {
            int postionType = gameObject.name == "HPpostion" ? 4 : 5;
            ShowItemUi slot = getInventoryData.FindPostionSlot(postionType);
            

            if(slot == null || slot.thisSlotCount == 0 ){
                Debug.Log("Postion Count is 0");
                return;
            }

            postionCoolTime = true;
            coolDownRect.sizeDelta = new Vector2(1f, 1f);
            StartCoroutine(PostionCoolDown());

            SaveInvenData usePostionData = new SaveInvenData();
            usePostionData.title = "UsePostion";
            usePostionData.id = socket.this_player.name;
            usePostionData.Key = slot.gameObject.name;
            usePostionData.Value[0] = slot.ThisSlotItemType;
            usePostionData.Value[1] = slot.thisSlotCount;
            Socket.Instance.ws.Send(JsonUtility.ToJson(usePostionData));
            slot.thisSlotCount--;
            getInventoryData.itemsNumber[postionType - 1]--;
        }
        
    }
    public IEnumerator PostionCoolDown(){
        for(float i = 0; i < coolTime; i += Time.deltaTime){
            coolDownRect.localScale = new Vector2(1f, (coolTime / 2) - (i / 2));
            yield return null;
        }
        postionCoolTime = false;
    }
}
