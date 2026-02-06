using UnityEngine;
public class ItemID_006 : Sinner
{
    float timer;
    const float timeLimit = 60.0f;
    float increase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        sinnerID = "ItemID_006";
        sinnerName = "君と歩んだクラゲ";
        LoadSprite("ID006");
        effect = effectObjectParent.transform.GetChild(5).gameObject;
        specifyingDeliveryRoutes.SinnerDebuff[0][sinnerName] = false;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeLimit)
        {
            timer -= timeLimit;
            increase = 10.0f;
            IncreaseProbabilitys(increase);
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        //水族館を通ってなかったら
        if (!specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.AQUARIUM))
        {
            increase = Random.Range(0, 101);
            IncreaseProbabilitys(increase);
            base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
            increase *= -1;
            IncreaseProbabilitys(increase);
        }
        else
        {
            if (specifyingDeliveryRoutes.SinnerDebuff[0][sinnerName])
            {
                Release(sinnerName);
            }
        }
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        specifyingDeliveryRoutes.SinnerDebuff[0][sinnerName] = true;
    }

    public override void Release(string name)
    {
        specifyingDeliveryRoutes.SinnerDebuff[0][sinnerName] = false;
        base.Release(sinnerName);
    }
}
