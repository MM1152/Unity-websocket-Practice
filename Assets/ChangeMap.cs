using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeMap : MonoBehaviour
{
    public string currentMapName;
    public Vector2 playerSpawnPos;
    public TileMap2D tilemap2D;
    private void Awake() {
        tilemap2D = GameObject.Find("Server").GetComponent<TileMap2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            MapData mapData = new MapData();
            mapData.title = "changeMap";
            mapData.mapName = currentMapName;
            //StartCoroutine(HttpRequest.HttpRequests.Request("http://localhost:8001/mapData" , "NeedMapName" , currentMapName , (value) => tilemap2D.GetData(value) , playerSpawnPos));
            Socket.Instance.ws.Send(JsonUtility.ToJson(mapData));
            other.transform.position = playerSpawnPos;
        }
    }
    
}
