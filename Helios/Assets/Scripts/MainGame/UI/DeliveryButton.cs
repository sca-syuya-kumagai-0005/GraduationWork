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
                    //�Ή�����g���b�N���N��
                }
                break;
        }
    }


}
