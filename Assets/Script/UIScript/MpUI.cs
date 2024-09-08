using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpUI : MonoBehaviour
{
    [SerializeField] private IMoveObj player;
    [SerializeField] private Slider mpValue;
    void Start()
    {
        player = transform.root.GetComponent<IMoveObj>();
        mpValue = GetComponent<Slider>();
        mpValue.maxValue = player.UserData.hp;
    }

    // Update is called once per frame
    void Update()
    {
        mpValue.value = player.UserData.mp;
    }
}
