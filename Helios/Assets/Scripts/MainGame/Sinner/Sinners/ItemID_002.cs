using UnityEngine;
public class ItemID_002 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 0.0f, 0.0f, 200.0f, 200.0f, 100.0f, 0.0f, 350.0f, 25.0f };
        sinnerID = "ItemID_002";
        sinnerName = "g‚¢…";
        sinnerTypeList.Add(SinnerType.Humanoid);
        LoadSprite("ID002");
        effect = effectObjectParent.transform.GetChild(1).gameObject;
        KumagaiLibrary.Dictionary.Support.AddArray(specifyingDeliveryRoutes.SinnerDebuff, sinnerName, false);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        //‘‰Á—Ê
        float increase;
        const int processType_Truck = 0;
        if (deliveryProcessID == processType_Truck)
        {
            increase = 25.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //‘S‚Ä‚ÌˆÙí‚É‚¨‚¢‚Ä‹¤’Ê‚Å‹N‚«‚é–‚ª‚ ‚ê‚Î«‚ğ•ÏX
        base.AbnormalPhenomenon();

        //‚»‚ê‚¼‚ê‚Ìˆ—‚Í‚±‚±‚É‘‚­
        specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID][sinnerName] = true;
    }
}
