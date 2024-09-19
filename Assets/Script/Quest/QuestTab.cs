using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTab : MonoBehaviour
{
    [SerializeField] Text questInfoText;
    [SerializeField] Text questExplanationText;
    [SerializeField] Text questClearText;
    [SerializeField] GameObject[] questNpcEffect; // 0 : 줄 수 있는 퀘스트가 있을때 , 1 : 퀘스트를 진행 중일때
    private GameManager gameManager;
    private Quest quest;
    private void Awake() {
        gameManager = GameManager.Instance;
        quest = GetComponentInParent<QuestNPC>().quest;
    }
    private void FixedUpdate() {
        
    }
    private void SetNpcQuestEffect(bool qusetFin){
        questNpcEffect[0].SetActive(qusetFin);
        questNpcEffect[1].SetActive(!qusetFin);
    }

}
