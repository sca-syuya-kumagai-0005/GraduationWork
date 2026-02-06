using UnityEngine;
public class ItemID_021 : Sinner
{
    TimeLine timeLine;
    MapMobilityManager mapMobilityManager;
    Sinner_021 effectManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Hazra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 200.0f, 85.0f, 30.0f, 100.0f, 25.0f, 70.0f, 150.0f, 20.0f };
        sinnerID = "ItemID_021";
        sinnerName = "Œã‰÷‚Ì—…j”Õ";
        LoadSprite("ID021");
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        mapMobilityManager = GameObject.Find("Map").GetComponent<MapMobilityManager>();
        effect = effectObjectParent.transform.GetChild(20).gameObject;
        effectManager = effect.GetComponent<Sinner_021>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        bool isNight = false;
        if (timeLine.TimeStateAccess == TimeLine.TimeState.Night)
        {
            isNight = true;
            IncreaseProbabilitys(165.0f);
        }

        bool isLongRoute = false;
        if (specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Count >= 65)
        {
            isLongRoute = true;
            IncreaseProbabilitys(70.0f);
        }

        bool isAroundSae = false;
        if (specifyingDeliveryRoutes.PassArea[deliveryLineID].Contains(3))
        {
            isAroundSae = true;
            IncreaseProbabilitys(65.0f);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
        if (isAroundSae)
        {
            IncreaseProbabilitys(-65.0f);
        }
        if (isLongRoute)
        {
            IncreaseProbabilitys(-70.0f);
        }
        if (isNight)
        {
            IncreaseProbabilitys(-165.0f);
        }
    }
    public override void AbnormalPhenomenon()
    {
        //‚»‚ê‚¼‚ê‚Ìˆ—‚Í‚±‚±‚É‘‚­
        string north = null;
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                north = "U";
                break;
            case 1:
                north = "D";
                break;
            case 2:
                north = "S";
                break;
            case 3:
                north = "A";
                break;
        }
        effectManager.Direction = (Sinner_021.WENS)rand;
        mapMobilityManager.NorthKey = north;
        //‘S‚Ä‚ÌˆÙí‚É‚¨‚¢‚Ä‹¤’Ê‚Å‹N‚«‚é–‚ª‚ ‚ê‚Î«‚ğ•ÏX
        base.AbnormalPhenomenon();

    }

    public override void Release(string name)
    {
        mapMobilityManager.NorthKey = "U";
        base.Release(sinnerName);
    }
}
