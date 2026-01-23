using System.Linq;
using System.Collections.Generic;
using UnityEngine;
public class ItemID_001 : Sinner
{
    private TimeLine timeLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 0.0f, 0.0f, 25.0f, 50.0f, 100.0f, 80.0f, 50.0f, 0.0f };
        sinnerID = "ItemID_001";
        sinnerName = "憎しみの堕星";
        LoadSprite("ID001");
        effect = effectObjectParent.transform.GetChild(0).gameObject;

        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        //神社が実装されたら書く
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains(0))
        {

        }
        if(timeLine.TimeStateAccess == TimeLine.TimeState.Morning)
        {
            //徒歩だったら
            if (deliveryProcessID == 2) IncreaseProbabilitys(25.0f);
        }
        if (timeLine.TimeStateAccess == TimeLine.TimeState.Night)
        {
            IncreaseProbabilitys(80.0f);
        }
        if (itemID==(int)Mood.Sadness)
        {
            IncreaseProbabilitys(100.0f);
        }
            base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
}
