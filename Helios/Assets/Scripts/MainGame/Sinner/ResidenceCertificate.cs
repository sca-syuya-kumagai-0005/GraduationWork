using UnityEngine;
using UnityEngine.UI;

public class ResidenceCertificate : EventSet
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
    private GameObject[] deliveryItemObjects = new GameObject[8];
    private int[] deliveryItems = new int[8];
    public int[] SetDeliveryItems { set { deliveryItems = value; } }


    private string secureClass;
    public string SetSecureClass {  set { secureClass = value; } }


    private string liskClass;
    public string SetLiskClass { set { liskClass = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < deliveryItems.Length; i++)
        {
            DeliveryButton button = deliveryItemObjects[i].AddComponent<DeliveryButton>();
            button.SetButtonID = i;
        }
        sinnerImage = transform.GetComponent<Image>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
