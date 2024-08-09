using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform center;
    public float moveSlotSpeed;

    private Transform Left {
        set {
            left = value;
            left.SetSiblingIndex(0);
        }
    }
    private Transform Right {
        set {
            right = value;
            right.SetSiblingIndex(1);
        }
    }
    private Transform Center {
        set {
            center = value;
            center.SetSiblingIndex(2);
        }
    }
    public int slotIndex;
    void Start(){
        moveSlotSpeed = 0.05f;
        slotIndex = 0;
    }
    public void OnClickLeft(){
        StartCoroutine(MoveSlot(right, left , center));
        slotIndex--;
        var temp = center;
        Center = right;
        Right = left;
        Left = temp;
        
    }
    public void OnClickRight(){
        StartCoroutine(MoveSlot(left , right , center));
        slotIndex++ ;
        var temp = center;
        Center = left;
        Left = right;
        Right = temp;
    }
    IEnumerator MoveSlot(Transform A , Transform B , Transform C){

        Vector3 Apos = A.position;
        Vector3 Bpos = B.position;
        Vector3 Cpos = C.position;
        
        RectTransform Arect = A.GetComponent<RectTransform>();
        Vector2 Asize = Arect.sizeDelta;
        Image AImage = A.GetComponent<Image>();

        RectTransform Crect = C.GetComponent<RectTransform>();
        Vector2 Csize = Crect.sizeDelta;
        Image CImage = C.GetComponent<Image>();

        Color CchangeColor = new Color32(180 , 180 , 180 , 255);
        Color AchangeColor = new Color32(255 , 255 ,255 ,255);

        for(float i = 0; i <= 1.2f; i += moveSlotSpeed){
            A.gameObject.transform.position = Vector3.Lerp(Apos , Cpos , i);

            Arect.sizeDelta = new Vector2(Asize.x + i * 25 , Asize.y + i * 25);
            AImage.color = Color.Lerp(AImage.color , AchangeColor , i);

            B.gameObject.transform.position = Vector3.Lerp(Bpos , Apos , i);
            C.gameObject.transform.position = Vector3.Lerp(Cpos , Bpos , i);   

            Crect.sizeDelta = new Vector2(Csize.x - i * 25 , Csize.y - i * 25);
            
            CImage.color = Color.Lerp(CImage.color , CchangeColor , i);
            Debug.Log(i);
            yield return null; 
        }



    }
}
