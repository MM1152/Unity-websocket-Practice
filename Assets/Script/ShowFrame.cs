using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFrame : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        InvokeRepeating("changeFrame" , 1 , 1);
    }
    void changeFrame()
    {
        text.text = "Frame: " + (1f / Time.deltaTime).ToString();
    }
    // Update is called once per frame
}
