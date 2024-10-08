using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [SerializeField] private Text questInfo;
    [SerializeField] private Text questExplanation;
    [SerializeField] private GameObject questAralm;

    private RectTransform rect;
    private Image image;
    private bool questTabHiding;
    private GameObject hidingButton;
    private Color questTablInitColor;
    private EnemyMapData enemyMapData = new EnemyMapData(); // Dictionary < Key : EnemyType , Value : EnemyMapName >
    /// <summary>
    /// 퀘스트가 클리어 됐는지 확인용
    /// </summary>
    public bool clear;
    public bool progressQuest;
    public static bool autoQuest;

    [SerializeField] private QuestData questData;
    public QuestData QuestData
    {
        get { return questData; }
        set
        {
            questData = value;
            if (questData == null)
            {
                questData = value;
                image.color = questTablInitColor;
                questInfo.text = "";
                questExplanation.text = "";
                return;
            }
            progressQuest = true;
            image.color = Color.white;
            questInfo.text = value.questinfo;
            questExplanation.text = value.enemyType != 0 ? value.questexplanation + $" {value.count} / {value.maxCount}" : value.questexplanation;
        }
    }

    public int clearQuestNumber;
    private void Awake()
    {
        image = GetComponent<Image>();
        questTablInitColor = image.color;
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        clearQuestNumber = Socket.Instance.this_player_MoveObject.getUserData().clearquestnumber;
    }
    public void ClearQuestTab()
    {
        Debug.Log(questData.questinfo);
        if(questData != null) questAralm.SetActive(true);
    }

    public void MoveToQuest()
    {
        
        Debug.Log(enemyMapData.GetEnemyMapData(questData.enemyType));
        if (questData.clearQuestTrans != null && clear)
        {
            if (Socket.Instance.this_player_MoveObject.getUserData().mapName != questData.clearQuestTrans.GetComponent<NpcAi>().NpcData.mapName)
            {
                ChangeMap.Instance.Change(questData.clearQuestTrans.GetComponent<NpcAi>().NpcData.mapName);
                StartCoroutine(MoveQuestNPC(questData.clearQuestTrans.position));
            }
            else
            {
                StartCoroutine(MoveQuestNPC(questData.clearQuestTrans.position));
            }
        }
        //퀘스트 진행 중일때 퀘스트 탭 클릭시 해당 위치로 이동하는 로직
        //이동후 퀘스트 자동진행 기능?
        else if (!clear)
        {
            if (questData.enemyType != 0)
            {
                
                if (enemyMapData.GetEnemyMapData(questData.enemyType) != Socket.Instance.this_player_MoveObject.getUserData().mapName)
                {
                    ChangeMap.Instance.Change(enemyMapData.GetEnemyMapData(questData.enemyType));
                }
            }
        }

    }
    IEnumerator MoveQuestNPC(Vector3 target)
    {
        yield return new WaitUntil(()=> TileMap2D.mapDownLoadEnd);
        Socket.Instance.this_player_MoveObject.stateMachine.Transition(new MoveState());
        autoQuest = true;
        Socket.Instance.this_player_MoveObject.FlipX(Math.Clamp(target.x - Socket.Instance.this_player.transform.position.x, -1, 1));
        while (Vector2.Distance(target, Socket.Instance.this_player.transform.position) > 0.8f || !Input.anyKeyDown)
        {
            Socket.Instance.this_player.transform.position += (target - Socket.Instance.this_player.transform.position).normalized * Time.deltaTime * Socket.Instance.this_player_MoveObject.speed;
            yield return null;
        }
        autoQuest = false;
    }
    private void ClearQuest()
    {
        questInfo.text = "";
        questExplanation.text = "Quest 클리어";
        clear = true;
        image.color = Color.yellow;
    }
    private void FixedUpdate()
    {
        if (questData != null)
        {
            if (questData.enemyType != 0)
            {
                questExplanation.text = questData.questexplanation + $" {questData.count} / {questData.maxCount}";
                if (questData.maxCount <= questData.count) ClearQuest();
            }
        }

    }
    public void QuestTabHiding()
    {
        questTabHiding = !questTabHiding;
        hidingButton ??= EventSystem.current.currentSelectedGameObject;

        string text;
        Vector3 movepos;
        float limit;

        if (hidingButton == null)
        {
            Debug.LogError("hiding button 설정 실패");
            return;
        }
        if (questTabHiding)
        {
            text = "◀";
            movepos = new Vector3(-2, 0, 0);
            limit = 85f;
        }
        else
        {
            text = "▶";
            movepos = new Vector3(2, 0, 0);
            limit = -90f;
        }

        hidingButton.GetComponentInChildren<Text>().text = text;
        StartCoroutine(MoveQuestTab(movepos, limit));
    }
    private IEnumerator MoveQuestTab(Vector3 movePos, float limit)
    {
        while (questTabHiding ? limit >= rect.anchoredPosition.x : limit <= rect.anchoredPosition.x)
        {
            transform.position -= movePos;
            yield return null;
        }
    }
}
