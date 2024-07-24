using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveInvenData
{
    public string title;
    public string Key;
    public int ItemSlotIndex;
    public int[] Value = new int[2];
    public string id;
}
[Serializable]
public class InventoryData
{
    public Dictionary<string , List<int>> item = new Dictionary<string, List<int>>();
    public Dictionary<string , int> equip = new Dictionary<string, int>();
    public Dictionary<string , int> equipItemTab = new Dictionary<string, int>();
    public int gold;
}
//class GetData
public class InventoryUI : MonoBehaviour
{

}
