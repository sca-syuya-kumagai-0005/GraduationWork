using UnityEngine;


//�@9/27 �F�J�ǉ�
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
        Debug.Log(myButtonType+"Button" + myButtonID + ":�N���b�N");
        switch (myButtonType)
        {
            case ButtonType.Item:
                {
                    //�z�B����n��Setter�Ăяo��
                }
                break;

            case ButtonType.Process:
                {
                    sDR.DriverSetting(myButtonID);
                    //�Ή�����g���b�N���N��
                }
                break;
        }
    }


}
