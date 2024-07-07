using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    [SerializeField] private GameObject settingTab;
    [SerializeField] private Slider HPrecoveryValue;
    [SerializeField] private Slider MPrecoveryValue;
    [SerializeField] private Slider SoundValue;
    
    public static GameManager Instance {
        get {
            if(Instance == null){
                return null;
            }

            return gameManager;
        }
    }
    void Awake()
    {
        gameManager = this;
        PlayerSettingLoad();
        DontDestroyOnLoad(gameObject);
    }
    private void Update() {
        PlayerSettingSave();
    }
    
    void PlayerSettingSave(){
        if(!settingTab.activeSelf){
            PlayerPrefs.SetFloat("HPrecovery" , HPrecoveryValue.value);
            PlayerPrefs.SetFloat("MPrecovery" , MPrecoveryValue.value);
            PlayerPrefs.SetFloat("Audio" , SoundValue.value);
            Debug.Log("Save Player Setting");
        }
    }

    void PlayerSettingLoad(){
        if(PlayerPrefs.HasKey("Audio")){
            HPrecoveryValue.value = PlayerPrefs.GetFloat("HPrecovery");
            MPrecoveryValue.value = PlayerPrefs.GetFloat("MPrecovery");
            SoundValue.value = PlayerPrefs.GetFloat("Audio");
        }

    }
}
