using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField] private IMoveObj player;
    [SerializeField] private Slider hpValue;
    void Start()
    {
        player = transform.root.GetComponent<IMoveObj>();
        hpValue = GetComponent<Slider>();
        hpValue.maxValue = player.UserData.hp;
    }

    // Update is called once per frame
    void Update()
    {
        hpValue.value = player.UserData.hp;
    }
}
