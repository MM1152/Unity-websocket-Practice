using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   

    private static GameManager gameManager;
    public QuestDatas quests;
    [SerializeField] private Text HPrecoveryValueText;
    [SerializeField] private Text MPrecoveryValueText;
    [SerializeField] private GameObject settingTab;
    [SerializeField] private Slider HPrecoverySlider;
    [SerializeField] private Slider MPrecoverySlider;
    [SerializeField] private Slider SoundValue;
    


    public float HPrecoveryValue {
        get {
            if(HPrecoverySlider == null) {
                Debug.Log("HPrecoverySlider Not Setting");
                return 0;
            }
            return HPrecoverySlider.value;
        }
    }
    public float MPrecoveryValue {
        get {
            if(MPrecoverySlider == null) {
                Debug.Log("MPrecoverySlider Not Setting");
                return 0;
            }
            return MPrecoverySlider.value;
        }
    }

    public static GameManager Instance {
        get {
            if(gameManager == null){
                return null;
            }

            return gameManager;
        }
    }
    void Awake()
    {
        gameManager = this;
        PlayerSettingLoad();
        HttpRequest.HttpRequests.Request("getQuestData" , "null" , "null" , (value) => GetQuestData(value));
        DontDestroyOnLoad(gameObject);
    }
    private void Update() {
        PlayerSettingSave();
        HPrecoveryValueText.text = (int)(HPrecoverySlider.value * 100) + "%";
        MPrecoveryValueText.text = (int)(MPrecoverySlider.value * 100) + "%";
    }
    
    void PlayerSettingSave(){
        if(!settingTab.activeSelf){
            PlayerPrefs.SetFloat("HPrecovery" , HPrecoverySlider.value);
            PlayerPrefs.SetFloat("MPrecovery" , MPrecoverySlider.value);
            PlayerPrefs.SetFloat("Audio" , SoundValue.value); 
        }
    }

    void PlayerSettingLoad(){
        if(PlayerPrefs.HasKey("Audio")){
            HPrecoverySlider.value = PlayerPrefs.GetFloat("HPrecovery");
            MPrecoverySlider.value = PlayerPrefs.GetFloat("MPrecovery");
            SoundValue.value = PlayerPrefs.GetFloat("Audio");
        }

    }
    private void GetQuestData(string data){
        quests = JsonUtility.FromJson<QuestDatas>(data);
    }


}
