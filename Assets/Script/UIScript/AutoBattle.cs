
using UnityEngine;
using UnityEngine.UI;

public class AutoBattle : MonoBehaviour
{
    [SerializeField] private GameObject autoPlayExplain;

    public static bool autoBattle { get; private set; }

    private Image image;

    private void Awake() {
        image = GetComponent<Image>();    
    }

    private void FixedUpdate() {
        if(autoBattle) image.color = Color.red;
        else image.color = Color.white;
        
        if(Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal")) {
            autoBattle = false;
            autoPlayExplain.SetActive(autoBattle);
        }
    }

    public void PressButton(){
        autoBattle = !autoBattle;
        autoPlayExplain.SetActive(autoBattle);
    }
}
