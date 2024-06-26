using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveInvenData
{
    public string Key;
    public int Value;
    public string id;
}
[Serializable]
public class InventoryData
{
    public Dictionary<string , int> item = new Dictionary<string, int>();
    public int gold;
}
//class GetData
public class InventoryUI : MonoBehaviour
{

}
