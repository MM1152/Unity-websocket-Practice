using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class SetStatsUI : MonoBehaviour
{
    private UserData userData;
    [SerializeField] private MoveObject moveObject;
    [SerializeField] private Text[] stats;
    [SerializeField] private Button[] UpgradeButton;
    [SerializeField] private Text statsPoint;


    public void SetStats(UserData userData){
        stats[0].text = userData.strStats.ToString();
        stats[1].text = userData.intStats.ToString();
    }

    public void UpgradButtonPush(int value){
        if(int.Parse(statsPoint.text) > 0){
            stats[value].text = (int.Parse(stats[value].text) + 1).ToString();
            statsPoint.text = (int.Parse(statsPoint.text) - 1).ToString();
            StatsUpdate();
        }
    }

    public void StatsUpdate(){
        userData = moveObject.getUserData();
        userData.strStats = int.Parse(stats[0].text);
        userData.intStats = int.Parse(stats[1].text);
    }

    public void SetStatsPoint(UserData userData){
        int count = 0;
        for(int i = 0; i < stats.Length; i++){
            count += int.Parse(stats[i].text);
        }
        if(count - stats.Length != userData.Level){
            statsPoint.text = (userData.Level - (count - stats.Length)).ToString();
        }else {
            statsPoint.text = "0";
        }
    }
}
