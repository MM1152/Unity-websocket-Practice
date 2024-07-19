using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUi : MonoBehaviour
{
    Socket socket;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject Status;
    [SerializeField] private GameObject Equip;

    void Start(){
        socket = Socket.Instance;
    }
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.I) && socket.this_player == this.gameObject.transform.root.gameObject)
        {
            Inventory.SetActive(!Inventory.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.U) && socket.this_player == this.gameObject.transform.root.gameObject){
            Status.SetActive(!Status.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.E) && socket.this_player == this.gameObject.transform.root.gameObject){
            Equip.SetActive(!Equip.activeSelf);
        }
    }
    
}
