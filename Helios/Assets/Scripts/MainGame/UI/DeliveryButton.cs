using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


//　9/27 熊谷追加
public class DeliveryButton : EventSetter
{
    private int myButtonID;
    public int SetButtonID { set { myButtonID = value; } }
    public enum ButtonType
    {
        Item,
        Process,
        Documents
    }
    const string driverTag = "Drivers";
    private SpecifyingDeliveryRoutes specifyingDeliveryRoutes;
    private ButtonType myButtonType;
    public ButtonType SetButtonType { set { myButtonType = value; } }
    private GameStateSystem gameState;

    private GameObject deliveryProcess;
    public GameObject SetDeliveryProcess { set { deliveryProcess = value; } }
    private Sprite[] buttonSprites = new Sprite[2];
    public Sprite OffButtonSprite { set { buttonSprites[0] = value; } }
    public Sprite OnButtonSprite { set { buttonSprites[1] = value; } }
    private Image myButton;
    private bool onClicked;
    void Awake()
    {
        specifyingDeliveryRoutes = GameObject.Find(driverTag).GetComponent<SpecifyingDeliveryRoutes>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
        myButton = gameObject.GetComponent<Image>();
    }
    private void OnEnable()
    {
        onClicked = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(down, OnClick);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClicked = false;
        }
        if (!buttonSprites.Contains(null))
        myButton.sprite = buttonSprites[onClicked ? 1 : 0];
    }

    private void OnClick()
    {
        switch (myButtonType)
        {
            case ButtonType.Item:
                {
                    gameState.GameState = GameStateSystem.State.DeliveryPreparation;
                    //選択された配達物をSet
                    specifyingDeliveryRoutes.DeliveryItemSetting(myButtonID);
                    StartCoroutine(ChangeMySprite());
                    //配達方法UIをActiveにする
                    deliveryProcess.SetActive(true);
                }
                break;

            case ButtonType.Process:
                {
                    //対応するレーンを起動、配達方法をSet
                    specifyingDeliveryRoutes.DeliveryProcessSetting(myButtonID);
                    gameState.GameState = GameStateSystem.State.Wait;
                }
                break;
            case ButtonType.Documents:
                {
                    //名前欄を押された時に資料にアクセスして関数を呼ぶ

                }
                break;
        }
    }

    private IEnumerator ChangeMySprite()
    {
        yield return new WaitForEndOfFrame();
        onClicked = true;
    }
}
