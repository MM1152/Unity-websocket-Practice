using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    MoveObject moveObject;
    Text text;
    int level;
    private void Awake() {
        text = GetComponent<Text>();
    }
    private void Start() {
        moveObject = Socket.Instance.this_player_MoveObject;
        level = moveObject.getUserData().Level;
    }

    private void FixedUpdate() {
        if(level != moveObject.getUserData().Level) {
            level = moveObject.getUserData().Level;
            text.text = "Level : " + level;
        }    
    }
}
