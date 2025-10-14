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
    private Sinner sinner;
    void Awake()
    {
        specifyingDeliveryRoutes =
            GameObject.Find(driverTag).GetComponent<SpecifyingDeliveryRoutes>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(down, OnClick);
    }
    private void OnClick()
    {
        Debug.Log(myButtonType + "Button" + myButtonID + ":クリック");
        switch (myButtonType)
        {
            case ButtonType.Item:
                {
                    //選択された配達物をSet
                    specifyingDeliveryRoutes.DeliveryItemSetting(myButtonID);
                }
                break;

            case ButtonType.Process:
                {
                    //対応するレーンを起動、配達方法をSet
                    specifyingDeliveryRoutes.DeliveryProcessSetting(myButtonID);
                    gameState.GameState = GameStateSystem.State.Wait;
                    //対応するトラックを起動
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
