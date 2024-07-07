using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SoundClip{
    NONE, 첫번째맵, 상점 
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; 
    public AudioSource audioSource;
    public Slider bgmVolum;

    [Header("#BGM")]
    public AudioClip[] bgmClip;
    
    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        audioSource =  GetComponent<AudioSource>();
        Init();
    }

    public void Init(){
        audioSource.clip = bgmClip[0];
        audioSource.loop = true;
        audioSource.Play(); 
    }
    public void SetBgmSound(SoundClip sound){
        audioSource.clip = bgmClip[(int)sound];
        audioSource.Play();
    }
    private void Update() {
       audioSource.volume = bgmVolum.value;
    }
}
