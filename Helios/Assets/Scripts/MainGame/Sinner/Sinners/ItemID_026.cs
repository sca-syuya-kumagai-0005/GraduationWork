using System.Linq;
using UnityEngine;
public class ItemID_026 : Sinner
{
    private float t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 100.0f,100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f,100.0f };
        sinnerID = "ItemID_026";
        sinnerName = "双極剣・盈虚";
        LoadSprite("atokaraireru");
        effect = effectObjectParent.transform.GetChild(25).gameObject;
        t = 0.0f;
        for (int i = 0; i < specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
            specifyingDeliveryRoutes.SinnerDebuff[i].Add(sinnerName, false);
    }
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID][sinnerName] = true;
        specifyingDeliveryRoutes.IsConfison[deliveryLineID] = true;
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く

    }
}
