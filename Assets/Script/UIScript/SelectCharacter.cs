using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    public int leftType;
    public int rightType;
    public int centerType;
    public int CenterType {
        get { return centerType; }
        set {
            centerType = math.abs(value) % images.Length;
            leftType = math.abs(centerType - 1) ;
            rightType = math.abs(centerType + 1) % images.Length;
            
            center.transform.GetChild(0).GetComponent<Image>().sprite = images[centerType];
            left.transform.GetChild(0).GetComponent<Image>().sprite = leftType < images.Length ? images[leftType] : null;
            right.transform.GetChild(0).GetComponent<Image>().sprite =  rightType < images.Length ? images[rightType] : null;

        }
    }
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
    void Awake(){
        moveSlotSpeed = 0.05f;
        CenterType = 1;
    }
    public void OnClickLeft(){
        StartCoroutine(MoveSlot(left, right , center));
        var temp = center;
        Center = left;
        Left = right;
        Right = temp;
        CenterType = leftType;

    }
    public void OnClickRight(){
        StartCoroutine(MoveSlot( right , left , center));
        var temp = center;
        Center = right;
        Right = left;
        Left = temp;
        CenterType = rightType ;
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
