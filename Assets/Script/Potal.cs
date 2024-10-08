using System;
using UnityEngine;

public class Potal : MonoBehaviour {
    string mapName = "";
    private void FixedUpdate() {
        if(Socket.Instance.this_player_MoveObject.getUserData().mapName != this.mapName) {
            this.mapName = Socket.Instance.this_player_MoveObject.getUserData().mapName;
            if(this.mapName != "상점") gameObject.SetActive(false);
        }
    }
}