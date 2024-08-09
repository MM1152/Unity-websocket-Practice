using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillUnLock : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] GameObject succes; // 조건에 부합할 시 뛰어주는 창
    [SerializeField] GameObject fail;  // 조건에 부합하지 않을때 뛰어주는 창
    public int level;
    public int gold;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Socket.Instance.this_player_MoveObject.UserData.Level >= level) {
            succes.SetActive(true);
            if(/*골드가 충족될때*/ true){

            } else {
                /*골드가 부족할때*/
            }
        }   
        else {
            fail.SetActive(true);
        }       
    }
    void CheckGold(){
        //GetInventory의 Gold를 가져와 체크하는 부분
        
    }
}
