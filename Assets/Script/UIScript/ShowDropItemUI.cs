
using UnityEngine;
using UnityEngine.UI;

public class ShowDropItemUI : MonoBehaviour
{
    [SerializeField] GameObject dropItemPrefeb;
    [SerializeField] EnemyCount enemyCount;
    
    // Start is called before the first frame update
    void Awake()
    {
        
        for(int i = 0; i < 15; i++){
            GameObject dropItem = Instantiate(dropItemPrefeb , transform);
            dropItem.GetComponent<ShowItemUi>().ThisSlotItemType = 0;
            dropItem.SetActive(false);
        }
        transform.parent.gameObject.SetActive(false);
    }

    public void SetDropItem(string mapName){
        for(int i = 0 ; i < gameObject.transform.childCount; i++){
            if(gameObject.transform.GetChild(i).gameObject.activeSelf) gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        for(int i = 0; i < ItemPooling.Instance.dropItemList[mapName].Count; i++){
            Debug.Log( ItemPooling.Instance.dropItemList[mapName][i]);
            gameObject.transform.GetChild(i).GetComponent<Image>().sprite = ItemPooling.Instance.itemList.itemImages[ItemPooling.Instance.dropItemList[mapName][i] - 1];
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        
    }
}
