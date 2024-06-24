using UnityEngine;

public class ItemPooling : MonoBehaviour
{
    private static ItemPooling itemPooling;
    [SerializeField] private ItemList itemList;
    private int createItemCount;
    public static ItemPooling Instance
    {
        get
        {
            if (itemPooling == null)
            {
                return null;
            }
            return itemPooling;
        }
    }

    private void Awake()
    {
        itemPooling = this;
    }

    public void MakeItem(Transform dropPos, Transform userPos, int item)
    {
        if (item != 0)
        {
            
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                GameObject thisItem = gameObject.transform.GetChild(i).gameObject;
                int type = thisItem.gameObject.GetComponent<SetItemInfo>().type;
                if (type == item)
                {
                    thisItem.transform.position = dropPos.position;
                    thisItem.GetComponent<Item>().SetTarget(userPos);
                    thisItem.SetActive(true);
                    createItemCount++;
                }
            }

            for(int i = 0; i < itemList.Items.Length; i++){
                SetItemInfo itemInfo = itemList.Items[i].GetComponent<SetItemInfo>();
                if(itemInfo.type == item){
                    GameObject thisItem = Instantiate(itemList.Items[i].gameObject , gameObject.transform);
                    thisItem.SetActive(false);
                    thisItem.GetComponent<Item>().SetTarget(userPos);
                    thisItem.name = itemInfo.name;
                    thisItem.transform.position = dropPos.position;
                    thisItem.SetActive(true);
                }
            }
            createItemCount = 0;
        }
    }
}