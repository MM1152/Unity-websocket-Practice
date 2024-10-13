using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    private Slider slider;
    private Text expValueText;
    // Start is called before the first frame update
    void Awake()
    {
        
        expValueText = GetComponentInChildren<Text>();
        slider = GetComponent<Slider>();
    }
    private void Start() {
        gameObject.SetActive(transform.root.name == Socket.Instance.this_player_MoveObject.name ? true : false);
    }
    public void setMaxExp(int value){
        slider.maxValue = value;
    }
    public void setcurrentExp(int value){
        expValueText.text = Math.Round((value / slider.maxValue) * 100 , 2).ToString() + "%";
        slider.value = value;
    }
}
