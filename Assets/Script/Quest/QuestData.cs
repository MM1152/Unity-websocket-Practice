using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData{
    [Header("처치 해야할 적")]
    public int enemyType;
    [Header("처치한 적의 수")]
    public int count;
    [Header("처치해야할 적의 수")]
    public int maxCount;
    [Header("선행되야하는 퀘스트 타입")]
    public int prerequisiteQuestType;
    [Header("현재 퀘스트 타입(value)")]
    public int type;
    [Header("퀘스트 설명")]
    public string questinfo;
    [Header("수행해야할 퀘스트 목표")]
    public string questexplanation;
    [Header ("NPC 정보")]
    public Transform giveQuestNPC;
    public Transform clearQuestNPC;
    public string mapName;
    [Header("퀘스트 클리어 보상")]
    public int clearGold;
    public int clearExp;
    public int clearItem;
}

[Serializable]
public class QuestDatas{
    public QuestData[] quests;
}