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
    private GameObject[] deliveryItemButtons;
    private int[] deliveryItems;
    public int[] SetDeliveryItems { set { deliveryItems = value; } }
    [SerializeField]
    private Sprite[] deliveryItemSprites_Off;
    public Sprite[] DeliveryItemSprites { get { return deliveryItemSprites_Off; } }
    [SerializeField]
    private Sprite[] deliveryItemSprites_On;

    private string secureClass;
    public string SetSecureClass {  set { secureClass = value; } }


    private string liskClass;
    public string SetLiskClass { set { liskClass = value; } }

    [SerializeField]
    private GameObject deliveryProcessParent;
    [SerializeField]
    private GameObject[] deliveryProcessButton;

    private GameStateSystem gameState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deliveryItems = new int[deliveryItemButtons.Length];
        for(int i = 0; i < deliveryItems.Length; i++)
        {
            DeliveryButton button = deliveryItemButtons[i].AddComponent<DeliveryButton>();
            button.SetButtonID = i;
            button.SetButtonType = DeliveryButton.ButtonType.Item;
        }

        for(int i = 0; i < deliveryProcessButton.Length; i++)
        {
            DeliveryButton button = deliveryProcessButton[i].AddComponent<DeliveryButton>();
            button.SetButtonID = i;
            button.SetButtonType = DeliveryButton.ButtonType.Process;
        }
        sinnerNameText.gameObject.AddComponent<DeliveryButton>().SetButtonType = DeliveryButton.ButtonType.Documents;
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
    }

    private void OnEnable()
    {
        deliveryProcessParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.GameState == GameStateSystem.State.DeliveryPreparation)
        {
            gameObject.SetActive(true);
            sinnerNameText.text = sinnerName;
            for (int i = 0; i < deliveryItemButtons.Length; i++)
            {
                DeliveryButton db = deliveryItemButtons[i].GetComponent<DeliveryButton>();
                db.OffButtonSprite = deliveryItemSprites_Off[deliveryItems[i]];
                db.OnButtonSprite = deliveryItemSprites_On[deliveryItems[i]];
                deliveryItemButtons[i].GetComponent<DeliveryButton>().SetDeliveryProcess = deliveryProcessParent;
            }
        }
        else
        {
            gameObject.SetActive(false);
            deliveryProcessParent.SetActive(false);
        }
    }
}
