using System.Linq;
using UnityEngine;
public class ItemID_024 : Sinner
{
    private TimeLine timeLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 10.0f, 50.0f, 30.0f, 10.0f, 10.0f, 60.0f, 50.0f, 30.0f };
        sinnerID = "ItemID_024";
        sinnerName = "誓いと終焉の盤局";
        LoadSprite("atokaraireru");
        effect = effectObjectParent.transform.GetChild(23).gameObject;
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        bool isNotWalk = false;
        bool isNight = false;
        if (deliveryProcessID == 0 || deliveryProcessID == 1)
            isNotWalk = true;
        if (timeLine.TimeStateAccess == TimeLine.TimeState.Night)
            isNight = true;
        if (isNotWalk) IncreaseProbabilitys(100.0f);
        if (isNight) IncreaseProbabilitys(100.0f);
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
        if (isNotWalk) IncreaseProbabilitys(-100.0f);
        if (isNight) IncreaseProbabilitys(-100.0f);
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く

    }
}
