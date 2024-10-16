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
    public int PosionNum
    {
        get
        {
            return posionNum;
        }
        set
        {
            posionNum = value;
            postionNumberText.text = value.ToString();
        }
    }
    private void Start()
    {
        socket = Socket.Instance;
        gameManager = GameManager.Instance;
    }
    public void Update()
    {
        if(this.gameObject.name == "HPpostion")UseHpPostion();
        else if(this.gameObject.name == "MPPostion")UseMpPostion();
        
    }
    public void UseHpPostion()
    {

        float value = gameManager.HPrecoveryValue;
        bool useHpPostion = value * 100f >=
         socket.this_player_MoveObject.UserData.hp / (float)socket.this_player_MoveObject.UserData.maxhp * 100f && !postionCoolTime;
        
        if (useHpPostion)
        {
            int postionType = 4;
            ShowItemUi slot = getInventoryData.FindPostionSlot(postionType);
            if(slot.thisSlotCount <= 0) return;
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
    public void UseMpPostion()
    {

        float value = gameManager.MPrecoveryValue;
        bool useMpPostion = value * 100f >=
         socket.this_player_MoveObject.UserData.mp / 50f * 100f && !postionCoolTime;
        if (useMpPostion)
        {
            int postionType = 5;

            ShowItemUi slot = getInventoryData.FindPostionSlot(postionType);
            if(slot.thisSlotCount <= 0) return;
            
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
    public IEnumerator PostionCoolDown()
    {
        for (float i = 0; i < coolTime; i += Time.deltaTime)
        {
            coolDownRect.localScale = new Vector2(1f, (coolTime / 2) - (i / 2));
            yield return null;
        }
        postionCoolTime = false;
    }
}
