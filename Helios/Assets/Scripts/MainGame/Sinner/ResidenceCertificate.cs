using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(2)]
public class ResidenceCertificate : MonoBehaviour
{
    [SerializeField]
    private Text sinnerNameText;
    private string sinnerName;
    public string SetSinnerName { set { sinnerName = value; } }

    private string sinnerID;
    public string SetSinnerID { set { sinnerID = value; } }

    [SerializeField]
    private Image sinnerImage;
    public Sprite SetSinnerImage { set { sinnerImage.sprite = value; } }

    [SerializeField]
    private GameObject[] deliveryItemButton;
    private int[] deliveryItems;
    public int[] SetDeliveryItems { set { deliveryItems = value; } }


    private string secureClass;
    public string SetSecureClass {  set { secureClass = value; } }


    private string liskClass;
    public string SetLiskClass { set { liskClass = value; } }

    [SerializeField]
    private GameObject[] deliveryProcessButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deliveryItems = new int[deliveryItemButton.Length];
        for(int i = 0; i < deliveryItems.Length; i++)
        {
            DeliveryButton button = deliveryItemButton[i].AddComponent<DeliveryButton>();
            button.SetButtonID = i;
            button.SetButtonType=DeliveryButton.ButtonType.Item;
        }

        for(int i = 0; i < deliveryProcessButton.Length; i++)
        {
            DeliveryButton button = deliveryProcessButton[i].AddComponent<DeliveryButton>();
            button.SetButtonID = i;
            button.SetButtonType = DeliveryButton.ButtonType.Process;
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(!Input.GetMouseButtonDown(1));
    }
}
