using UnityEngine;

public class DeliveryButton : EventSet
{
    private int myButtonID;
    public int SetButtonID { set { myButtonID = value; } }
    public enum ButtonType
    {
        Item,
        Process
    }
    private ButtonType myButtonType;
    public ButtonType SetButtonType { set {  myButtonType = value; } }

    [SerializeField] GameObject drivers;
    private SpecifyingDeliveryRoutes specifyingDeliveryRoutes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        specifyingDeliveryRoutes = GameObject.Find(drivers.name).GetComponent<SpecifyingDeliveryRoutes>();
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
                    //選択された配達物をSet
                    specifyingDeliveryRoutes.DeliveryItem = myButtonID;
                }
                break;

            case ButtonType.Process:
                {
                    //対応するレーンを起動、配達方法をSet
                    specifyingDeliveryRoutes.DriverType = (int)myButtonType;
                }
                break;
        }
    }


}
