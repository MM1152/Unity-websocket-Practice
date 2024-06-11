using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public class MapData{
    public int id;
    public int mapSizeX;
    public int mapSizeY;
    public int[] mapValue;
    public int[] decoValue;
    public string mapName;
    
}
[Serializable]
public class GetData{
    public List<MapData> mapData;
}
public class TileMap2D : MonoBehaviour
{
    public Sprite[] mapImage;
    public Sprite[] decoImage;
    public Transform mapSpawn;
    public Transform decoSpawn;
    public GameObject Tile;
    private void Awake() {
        StartCoroutine(GetServerData("첫번째 맵"));
    }

    public IEnumerator GetServerData(string jsonData)
    {
        string url = "http://localhost:8001/mapData";
        WWWForm form = new WWWForm();
        form.AddField("NeedMapName",jsonData);
        UnityWebRequest request = UnityWebRequest.Post(url , form);

        yield return request.SendWebRequest();

         if (request.error == null)
        {
            string result = request.downloadHandler.text;
            Debug.Log(result);
            GetData mapdata = JsonUtility.FromJson<GetData>(result);
            var mapData = mapdata.mapData[0];
            int y = 0;
            int bottomX = -mapData.mapSizeX / 2;
            int bottonY = -mapData.mapSizeY / 2;
            for(int x = 0; x < mapData.mapValue.Length; x++){
                if(mapData.mapValue[x] == 0){
                    continue;
                }
                if (x % mapData.mapSizeX == 0 && x != 0)
                {
                        y++;
                }
                Vector2 position = new Vector2(x % mapData.mapSizeX + bottomX , y + bottonY);
                GameObject spawnTile = Instantiate(Tile , mapSpawn);
                spawnTile.GetComponent<SpriteRenderer>().sprite = mapImage[mapData.mapValue[x]]; 
                spawnTile.transform.position = position;

                if(mapData.decoValue[x] == 0){
                    continue;
                }
                GameObject spawnDeco = Instantiate(Tile , decoSpawn);
                spawnDeco.GetComponent<SpriteRenderer>().sprite = decoImage[mapData.decoValue[x]];
                spawnDeco.transform.position = position;
            }
        }
        else
        {
            Debug.Log("error");
        }

    }


}
