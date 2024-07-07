using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPosionNumberUI : MonoBehaviour
{
    [SerializeField]private GetInventoryData inventoryData;
    [SerializeField]private Text hpPosionNumText;
    [SerializeField]private Text mpPosionNumText;
    private int hp_posion;
    private int mp_postion;
    private int HP_Posion {
        get {
            return hp_posion;
        }
        set {
            hp_posion = value;
            hpPosionNumText.text = value.ToString();
        }
    }
    private int MP_Posion {
        get {
            return mp_postion;
        }
        set {
            mp_postion = value;
            mpPosionNumText.text = value.ToString();
        }
    }
    private void LateUpdate() {
        if(HP_Posion !=  inventoryData.itemsNumber[3]) HP_Posion = inventoryData.itemsNumber[3];
        if(MP_Posion !=  inventoryData.itemsNumber[4]) MP_Posion = inventoryData.itemsNumber[4];
    }

    public void UsePostion(int index){
        inventoryData.itemsNumber[index]--;
    }
}
