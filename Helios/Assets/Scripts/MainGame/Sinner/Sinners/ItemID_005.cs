using UnityEngine;
public class ItemID_005 : Sinner
{
    private bool isRampage = false;
    private TimeLine timeLine;
    private float abnormalityDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 100.0f, 125.0f, 100.0f, 75.0f, 0.0f, 25.0f, 100.0f };
        sinnerID = "ItemID_005";
        sinnerName = "あなたの為の天象儀";
        LoadSprite("atokaraireru");
        effect = GameObject.Find("Effect").transform.Find("").gameObject;

        timeLine = GameObject.Find("Clock").GetComponent<TimeLine>();
        abnormalityDuration = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        abnormalityDuration -= Time.deltaTime;
        if (abnormalityDuration >= 0.0f)
        {

        }
    }
    public override void ReceiveDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        float increase;//各条件に対する確率増加量
        if (itemID == (int)Mood.Anticipation || itemID == (int)Mood.Trust)
        {
            isRampage = true;
        }
        else if (itemID == (int)Mood.Anger || itemID == (int)Mood.Disgust)
        {
            increase = 100.0f;
            IncreaseProbabilitys(increase);
        }
        const int processType_Truck = 0;
        if (deliveryProcessID == processType_Truck)
        {
            increase = 80.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiveDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
    }
}
