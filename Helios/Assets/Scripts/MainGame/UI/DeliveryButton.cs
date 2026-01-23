using UnityEngine;


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
    private bool isPuhsed;
    public bool IsPuhsed {  get { return isPuhsed; } }
    void Awake()
    {
        specifyingDeliveryRoutes =
            GameObject.Find(driverTag).GetComponent<SpecifyingDeliveryRoutes>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
    }
    private void OnEnable()
    {
        isPuhsed = false;
    }
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(down, OnClick);
    }
    private void OnClick()
    {
        switch (myButtonType)
        {
            case ButtonType.Item:
                {
                    isPuhsed = true;
                    gameState.GameState = GameStateSystem.State.DeliveryPreparation;
                    //選択された配達物をSet
                    specifyingDeliveryRoutes.DeliveryItemSetting(myButtonID);
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
}
