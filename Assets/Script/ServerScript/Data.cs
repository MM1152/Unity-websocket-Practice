using System;
using UnityEngine;
/// <summary>
/// Type (string title , int id , int[] users , float x , float y)
/// </summary>
[Serializable]
public class Data{
    public string title;
    public string id;
    public UserData[] users;
    public UserData this_player;
    public Vector2 moveXY;
    public float x;
    public float y;
    public EnemyData[] enemyList;
    public EnemyData enemy;
    public NPCData[] NPC;
    public int dropItem;
    public ItemInfo item;

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
    public string id;
    public float x;
    public float y;
    public int hp;
    public int mp;
    public int strStats;
    public int intStats;
    public int exp;
    public int Level;
    public int maxExp;
    public string mapName;
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
}
[Serializable]
public class NPCData{
    public int id;
    public int type;
    public string talk;
    public string name;
    public Vector2 spawnPos;
    public NPC[] NPCList;
}

[Serializable]
public class NPC {
    public int id;
    public int type;
}
