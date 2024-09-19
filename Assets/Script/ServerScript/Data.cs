using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type (string title , int id , int[] users , float x , float y)
/// </summary>
[Serializable]
public class Data{
    public Data() {}
    public string title;
    public string id;
    public int useItemType;
    public UserData[] users;
    public UserData this_player;
    public Vector2 moveXY;
    public float x;
    public float y;
    public EnemyData[] enemyList;
    public EnemyData enemy = new EnemyData();
    public NPCData[] NPC;
    public int dropItem;
    public int hitDamage;
    public int skillinfo;
    public Data(string title){
        this.title = title;
    }
    public Data(string title , string id){
        this.title = title;
        this.id = id;
    }
    public Data(string title , string id, float x , float y){
        this.title = title;
        this.id = id;
        this.x = x;
        this.y = y;
    }
    public Data(string title , string id, float x , float y , Vector2 moveXY){
        this.title = title;
        this.id = id;
        this.x = x;
        this.y = y;
        this.moveXY = moveXY;
    }

    public Data getData(){
        return this;
    }
    
}
[Serializable]
public class UserData{
    public int type;
    public string id;
    public float x;
    public float y;
    public int maxhp;
    public int hp;
    public int mp;
    public int strStats;
    public int intStats;
    public int exp;
    public int Level;
    public int maxExp;
    public string mapName;
    public int defense;
    //public int attack;
    public float attackRadious;
    public Dictionary<string , int> equipItem;
}
[Serializable]
public class Enemys{
    public EnemyData[] enemyList;
}
[Serializable]
public class EnemyData{
    public int type;
    public int id;
    public float x;
    public float y;
    public string state;
    public int Hp;
    public int MaxHp;
    public string mapName;
    public List<int> dropItemList;
    public UserData FollowTarger;
}
[Serializable]
public class NPCData{
    public int id;
    public int type;
    public string talk;
    public string name;
    public Vector2 spawnPos;
    public int[] sellingList;
    public int[] quest_type;
    public NPC[] NPCList;
}

[Serializable]
public class NPC {
    public int id;
    public int type;
    public int npc_type;
}
