using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowMapUI : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IPointerClickHandler
{
    [SerializeField] Text mapName;
    [SerializeField] ShowDropItemUI dropItemPrefeb;
    [SerializeField] GameObject MapUI;
    ChangeMap changeMap;
    RectTransform size;
    GameObject parent;

    private void Awake() {
        changeMap = GetComponent<ChangeMap>();
        size = MapUI.GetComponent<RectTransform>();
        
    }
    private void Start() {
        parent ??= transform.parent.gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        MapUI.transform.position = (Vector3)eventData.position + new Vector3(size.sizeDelta.x / 1.5f , size.sizeDelta.y / 1.5f);
        mapName.text = changeMap.currentMapName;
        dropItemPrefeb.SetDropItem(changeMap.currentMapName);
        MapUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MapUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && other.gameObject == Socket.Instance.this_player){
            MapUI.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parent.SetActive(false);
        MapUI.SetActive(false);
    }
}
