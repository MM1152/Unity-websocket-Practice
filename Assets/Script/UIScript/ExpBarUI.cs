using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void setMaxExp(int value){
        slider.maxValue = value;
    }
    public void setcurrentExp(int value){
        slider.value = value;
    }
}
