using UnityEngine;
public class ItemID_027 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        sinnerID = "ItemID_027";
        sinnerName = "招かれザル";
        LoadSprite("ID027");
        effect = effectObjectParent.transform.GetChild(22).gameObject;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }

    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
    }

    public override void Release(string name)
    {
        base.Release(sinnerName);
    }
}
