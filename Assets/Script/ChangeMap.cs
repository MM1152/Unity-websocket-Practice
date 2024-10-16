  using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeMap : MonoBehaviour , IPointerClickHandler
{
    public static ChangeMap Instance;
    public string currentMapName;
    public Vector2 playerSpawnPos;
    public TileMap2D tilemap2D;
    [SerializeField] GameObject changeMap;
    public void OnPointerClick(PointerEventData eventData)
    {
        Change(currentMapName);        
    }

    public void Change(string mapName){
        MapData mapData = new MapData();
        mapData.title = "changeMap";
        mapData.mapName = mapName;
        Socket.Instance.this_player_MoveObject.playerMap = mapName;
        HttpRequest.HttpRequests.Request("mapData" , "NeedMapName" , mapName , (value) => tilemap2D.GetData(value) , playerSpawnPos);
        Socket.Instance.ws.Send(JsonUtility.ToJson(mapData));
        Socket.Instance.this_player.transform.position = playerSpawnPos;
    }
    private void Awake() {
        tilemap2D = GameObject.Find("Server").GetComponent<TileMap2D>();
        changeMap = transform.parent.gameObject;
        Instance = this;
    }
}
