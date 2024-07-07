using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingButton : MonoBehaviour
{
    [SerializeField] private GameObject settingWindow;
    private GameObject pastGameObject;
    public void ShowSetting(){
        settingWindow.SetActive(!settingWindow.activeSelf);
    }
    public void ClickSettingTab(GameObject settingTab){
        if(pastGameObject != null) pastGameObject.SetActive(false);
        settingTab.SetActive(true);
        pastGameObject = settingTab;
    }
    
}
