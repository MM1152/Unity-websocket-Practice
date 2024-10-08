using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffSkillController : MonoBehaviour
{
    public static Dictionary<string , float> buffType = new Dictionary<string, float>();

    void Update()
    {
        foreach(var buff in buffType.Keys.ToList()) {
            if(buffType[buff] > 0) buffType[buff] -= Time.deltaTime;
            if(buffType[buff] <= 0) {
                buffType.Remove(buff);
                Socket.Instance.this_player_MoveObject.attack -= 20;
            }
        }
        
    }

}
