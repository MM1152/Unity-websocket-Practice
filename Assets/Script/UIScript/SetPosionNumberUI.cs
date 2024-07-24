
using UnityEngine;

public class SetPosionNumberUI : MonoBehaviour
{
    [SerializeField]private GetInventoryData inventoryData;
    [SerializeField]private PostionUI[] postionGameObject;
    [SerializeField]private GameObject[] autoAnimationGameObject;
    /// <summary>
    /// bool[0] : HP , 
    /// bool[1] : MP
    /// </summary>
    public static bool[] autoRecovery;

    void Start(){
        autoRecovery = new bool[2];   
    }
    
    private void FixedUpdate() {
        if(postionGameObject[0].PosionNum !=  inventoryData.itemsNumber[3]) postionGameObject[0].PosionNum = inventoryData.itemsNumber[3];
        if(postionGameObject[1].PosionNum !=  inventoryData.itemsNumber[4]) postionGameObject[1].PosionNum = inventoryData.itemsNumber[4];
    }

    public void AutoUse(int index){
        autoAnimationGameObject[index].SetActive(!autoAnimationGameObject[index].activeSelf);
        autoRecovery[index] = autoAnimationGameObject[index].activeSelf;
    }
}
