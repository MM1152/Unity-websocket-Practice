

using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "ItemInfo" , menuName = "MakeItemInfo")]
public class ItemInfo : ScriptableObject{
    public int type;
    public int damage;
    public string name;
}
