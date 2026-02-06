using UnityEngine;
public class ItemID_013 : Sinner
{
    bool isAbnormality;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 5.0f, 5.0f, 100.0f, 100.0f, 40.0f, 10.0f, 50.0f, 5.0f };
        sinnerID = "ItemID_013";
        sinnerName = "自覚の道は己が夢";
        LoadSprite("ID013");
        effect = effectObjectParent.transform.GetChild(12).gameObject;
        isAbnormality = false;
        for(int i = 0; i < specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
        {
            if(specifyingDeliveryRoutes.SinnerDebuff[i] == null)
            specifyingDeliveryRoutes.SinnerDebuff[i].Add(sinnerName, false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isAbnormality)
        {
            deliveryItems[(int)Mood.Sadness] = Mood.Exception;
            for (int i = 0; i < specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
            {
                specifyingDeliveryRoutes.SinnerDebuff[i][sinnerName] = true;
            }

        }
        else
        {
            deliveryItems[(int)Mood.Sadness] = Mood.Sadness;
            for (int i = 0; i < specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
            {
                specifyingDeliveryRoutes.SinnerDebuff[i][sinnerName] = false;
            }
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (itemID == (int)Mood.Sadness && isAbnormality)
        {
            progressGraph.SinnerList.Remove(sinnerName);
            DeleteRanpage();
            spriteRenderer.color = new Color(0.25f, 1.0f, 0.15f);

            progressGraph.AddProgress();
            Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
            DeliveryProgressCheck();
        }
        int increase = 0;
        increase += specifyingDeliveryRoutes.AbnormalCount[deliveryLineID] * 35;
        increase += specifyingDeliveryRoutes.TotalAbnormal * 5;
        IncreaseProbabilitys(increase);

        base.ReceiptDeliveryInformation(itemID,deliveryProcessID, deliveryLineID);
        IncreaseProbabilitys(-increase);
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        isAbnormality = true;
    }

    public override void Release(string name)
    {
        isAbnormality = false;
        base.Release(sinnerName);
    }
}
