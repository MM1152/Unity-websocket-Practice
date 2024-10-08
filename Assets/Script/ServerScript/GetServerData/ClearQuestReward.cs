using UnityEngine;
public class ClearQuestReward : ISocket {
    GetInventoryData inventoryData;
    public override void RunNetworkCode(Data data)
    {
        // 아이템 추가 기능 로직 추가.
        inventoryData ??= GameObject.FindObjectOfType<GetInventoryData>();
        Debug.Log("QuestClear");
        Debug.Log(data.dropItem);
        Socket.Instance.this_player_MoveObject.UserData.exp = data.this_player.exp;
        Socket.Instance.this_player_MoveObject.UserData.clearquestnumber = data.this_player.clearquestnumber;
        
        if(data.dropItem != 0) {
             bool insert = inventoryData.SetInventory(data.dropItem , null);
        }
       

        CoinPooling.coinPooling.GetCoin(data.useItemType);
        CoinPooling.coinPooling.SettingCoin(null);
    }
}