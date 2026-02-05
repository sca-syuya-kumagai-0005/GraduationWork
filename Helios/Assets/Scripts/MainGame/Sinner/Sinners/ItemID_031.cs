using UnityEngine;
using UnityEngine.UIElements;
public class ItemID_031 : Sinner
{
    private GameObject mapObject;
    private SinnerDistribute distribute;
    bool isAbnormality;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 200.0f, 200.0f, 200.0f, 200.0f, 200.0f, 200.0f, 200.0f, 200.0f };
        sinnerID = "ItemID_031";
        sinnerName = "Unknownƒ¢";
        LoadSprite("ID031");
        LoadSinnerObject();
        effect = effectObjectParent.transform.GetChild(8).gameObject;

        distribute = GameObject.Find("Map").gameObject.GetComponent<SinnerDistribute>();
        isAbnormality = false;
    }
    // Update is called once per frame
    private void Update()
    {

    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    public override void AbnormalPhenomenon()
    {
        //‘S‚Ä‚ÌˆÙí‚É‚¨‚¢‚Ä‹¤’Ê‚Å‹N‚«‚é–‚ª‚ ‚ê‚Î«‚ğ•ÏX
        base.AbnormalPhenomenon();

        isAbnormality = true;
    }
}