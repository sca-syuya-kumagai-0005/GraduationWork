using UnityEngine;


//　9/27 熊谷追加
public class DeliveryButton : EventSet
{
    private int myButtonID;
    public int SetButtonID { set { myButtonID = value; } }
    public enum ButtonType
    {
        Item,
        Process
    }
    SpecifyingDeliveryRoutes sDR;
    const string driverTag = "Drivers";
    private ButtonType myButtonType;
    public ButtonType SetButtonType { set {  myButtonType = value; } }

    void Awake()
    {
        GameObject obj = GameObject.Find(driverTag).gameObject;
        sDR = obj.GetComponent<SpecifyingDeliveryRoutes>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(down, OnClick);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        Debug.Log(myButtonType+"Button" + myButtonID + ":クリック");
        switch (myButtonType)
        {
            case ButtonType.Item:
                {
                    //配達物を渡すSetter呼び出し
                }
                break;

            case ButtonType.Process:
                {
                    sDR.DriverSetting(myButtonID);
                    //対応するトラックを起動
                }
                break;
        }
    }


}
