using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUi : MonoBehaviour
{
    Socket socket;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject Status;
    [SerializeField] private GameObject Equip;
    [SerializeField] private GameObject Skill;

    void Start(){
        socket = Socket.Instance;
    }
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Escape) && socket.this_player == this.gameObject.transform.root.gameObject) {
            Inventory.SetActive(false);
            Status.SetActive(false);
            Equip.SetActive(false);
            Skill.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.I) && socket.this_player == this.gameObject.transform.root.gameObject) {Debug.Log("Open inventory"); Inventory.SetActive(!Inventory.activeSelf);} 
        if(Input.GetKeyDown(KeyCode.U) && socket.this_player == this.gameObject.transform.root.gameObject) Status.SetActive(!Status.activeSelf);
        if(Input.GetKeyDown(KeyCode.E) && socket.this_player == this.gameObject.transform.root.gameObject) Equip.SetActive(!Equip.activeSelf);
        if(Input.GetKeyDown(KeyCode.K) && socket.this_player == this.gameObject.transform.root.gameObject) Skill.SetActive(!Skill.activeSelf);
    }
    
}
