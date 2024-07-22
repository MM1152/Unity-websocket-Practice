using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class MapData
{
    public string title;
    public int id;
    public int mapSizeX;
    public int mapSizeY;
    public int[] mapValue;
    public int[] decoValue;
    public int[] colliderValue;
    public string mapName;
    
    public void ToString(){
        Debug.Log($"title : {title} , id : {id} , mapSizeX : {mapSizeX} , mapSizeY : {mapSizeY} , mapName : {mapName}");
    }
}
[Serializable]
public class GetData
{
    public string title;
    public List<MapData> mapData;
}

public class TileMap2D : MonoBehaviour
{
    [SerializeField] private Image ChangeScene;
    [SerializeField] private Text mapNameText;
    public Sprite[] mapImage;
    public Sprite[] colliderImage;
    private HttpRequest httpRequest;
    public Sprite[] decoImage;
    public Transform mapSpawn;
    public Transform decoSpawn;
    public Transform colliderSpawn;
    private bool mapDownLoad;
    public GameObject Tile;
    private bool firstIn;
    private void Awake()
    {
        httpRequest = HttpRequest.HttpRequests;
        Debug.Log("StartMapDataCorutine");
        httpRequest.Request("http://localhost:8001/mapData", "NeedMapName", "상점", (value) => GetData(value));
        Socket.Instance.ws.OnMessage += (sender , e) => {
            MapData mapdata = JsonUtility.FromJson<MapData>(e.Data);
            
            if(mapdata.title.Equals("changeMap")){
                Socket.Instance.queue.Enqueue(() => GetData(e.Data.ToString()));
            }

        };
    }
    public void GetData(string result)
    {  
        StartCoroutine(ChangeMap());
        for(int i = 0; i < mapSpawn.childCount; i++){
            Destroy(mapSpawn.GetChild(i).gameObject);
        }
        for(int i = 0; i < decoSpawn.childCount; i++){
            Destroy(decoSpawn.GetChild(i).gameObject);
        }
        for(int i = 0; i < colliderSpawn.childCount; i++){
            Destroy(colliderSpawn.GetChild(i).gameObject);
        }

        GetData mapdata = JsonUtility.FromJson<GetData>(result);
        var mapData = mapdata.mapData[0];
        mapNameText.text = mapData.mapName;
        
        int y = 0;
        int bottomX = -mapData.mapSizeX / 2;
        int bottonY = -mapData.mapSizeY / 2;
        for (int x = 0; x < mapData.mapValue.Length; x++)
        {
            if (x % mapData.mapSizeX == 0 && x != 0)
            {
                y++;
            }
            if (mapData.mapValue[x] == 0)
            {
                continue;
            }

            Vector2 position = new Vector2(x % mapData.mapSizeX + bottomX, y + bottonY);
            GameObject spawnTile = Instantiate(Tile, mapSpawn);
            spawnTile.GetComponent<SpriteRenderer>().sprite = mapImage[mapData.mapValue[x]];
            spawnTile.transform.position = (Vector3)position + new Vector3(0f, 0f, spawnTile.transform.parent.transform.position.z);
            if(mapData.colliderValue.Length != 0 && mapData.colliderValue[x] != 0){
                GameObject spawnCollider = Instantiate(Tile , colliderSpawn);
                spawnCollider.GetComponent<SpriteRenderer>().sprite = colliderImage[mapData.colliderValue[x]];
                spawnCollider.transform.position = (Vector3)position + new Vector3(0f, 0f, spawnCollider.transform.parent.transform.position.z);
                spawnCollider.AddComponent<BoxCollider2D>();
            }

            if (mapData.decoValue[x] != 0)
            {
                GameObject spawnDeco = Instantiate(Tile, decoSpawn);
                spawnDeco.GetComponent<SpriteRenderer>().sprite = decoImage[mapData.decoValue[x]];
                spawnDeco.transform.position = (Vector3)position + new Vector3(0f, 0f, spawnDeco.transform.parent.transform.position.z);
            }

        }
        mapDownLoad = true;
        if(!firstIn){
            Socket.Instance.ws.Connect();
        }
        
        Socket.Instance.ws.Send(JsonUtility.ToJson(new Data("CheckThisMapEnemy")));
        
    }
    
    IEnumerator ChangeMap(){
        ChangeScene.color = new Color(0f, 0f ,0f , 1f);
        mapNameText.color = new Color(1f, 1f, 1f , 1f);
        yield return new WaitUntil(() => mapDownLoad);
        yield return new WaitForSeconds(0.3f);
        for(float Alpha = 1.0f; Alpha >= 0f; Alpha -= 0.01f){
            ChangeScene.color = new Color(0f, 0f ,0f , Alpha);
            mapNameText.color = new Color(1f, 1f , 1f ,Alpha);
            yield return null;
        }
        
    }
}
