using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;


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
