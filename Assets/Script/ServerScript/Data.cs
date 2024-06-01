using System;
using UnityEngine;
/// <summary>
/// Type (string title , int id , int[] users , float x , float y)
/// </summary>
[Serializable]
public class Data{
    public string title;
    public int id;
    public int[] users;
    public float x;
    public float y;
    public Vector2 moveXY;
    public Data(string title){
        this.title = title;
    }
    public Data(string title , int id){
        this.title = title;
        this.id = id;
    }
    public Data(string title , int[] users){
        this.title = title;
        this.users = users;
    }
    public Data(string title , int id, float x , float y){
        this.title = title;
        this.id = id;
        this.x = x;
        this.y = y;
    }
    public Data (string title, int id , float x , float y , Vector2 moveXY){
        this.title=title;
        this.id = id;
        this.x = x;
        this.y = y;
        this.moveXY = moveXY;
    }
    public Data getData(){
        return this;
    }

}