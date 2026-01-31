using UnityEngine;
public class ItemID_023 : Sinner
{
    private bool isAbnormality;
    private bool isWalk;
    private bool isNight;

    MouseNoise mouseNoise;
    TimeLine timeLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 20.0f, 200.0f, 50.0f, 70.0f, 40.0f, 0.0f, 30.0f, 0.0f };
        sinnerID = "ItemID_023";
        sinnerName = "ひとりぼっちのブランコ";
        LoadSprite("ID023");
        effect = effectObjectParent.transform.GetChild(21).gameObject;

        isAbnormality = false;
        isWalk = false;
        isNight = false;
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        mouseNoise = GameObject.Find("PlayerObject").GetComponent<MouseNoise>();
    }
    // Update is called once per frame
    void Update()
    {
        mouseNoise.IsNoise = isAbnormality;
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (deliveryProcessID == 2)
        {
            if (isAbnormality)
            {
                isAbnormality = false;
                IncreaseProbabilitys(-999.0f);
                base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
                IncreaseProbabilitys(999.0f);
                announceManager.MakeAnnounce(sinnerName + "の異常は解消されました。");
                return;
            }
            else
            {
                isWalk = true;
                IncreaseProbabilitys(150.0f);
            }
        }
        if (timeLine.TimeStateAccess == TimeLine.TimeState.Night)
        {
            isNight = true;
            IncreaseProbabilitys(200.0f);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);

        if (isWalk)
        {
            IncreaseProbabilitys(-150.0f);
            isWalk = false;
        }
        if(isNight)
        {
            IncreaseProbabilitys(-200.0f);
            isNight = false;
        }
    }

    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        isAbnormality = true;
    }
}
