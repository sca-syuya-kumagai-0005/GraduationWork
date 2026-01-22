using UnityEngine;
using UnityEngine.UIElements;
public class ItemID_009 : Sinner
{
    private TimeLine timeLine;
    bool isAbnormality;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 10.0f, 10.0f, 20.0f, 100.0f, 150.0f, 50.0f, 250.0f, 0.0f };
        sinnerID = "ItemID_009";
        sinnerName = "朽ちた天馬";
        LoadSprite("ID009");
        effect = effectObjectParent.transform.GetChild(8).gameObject;

        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        isAbnormality = false;
    }
    // Update is called once per frame
    private void Update()
    {
        if(!isAbnormality)
        {
            if (timeLine.TimeState == TimeLine.TimeStates.Night && deliveryCount == 0)
            {
                isAbnormality = true;
                AbnormalPhenomenon();
            }
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (timeLine.TimeState == TimeLine.TimeStates.Night || deliveryProcessID == 0) AbnormalPhenomenon();
        bool notPassedZoo = false;
        if (specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.ZOO))
        {
            IncreaseProbabilitys(90.0f);
            notPassedZoo = true;
        }
        if (notPassedZoo) IncreaseProbabilitys(-90.0f);
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        //「定期的に衝撃波をインスタンスする馬」をインスタンスする
    }
}
