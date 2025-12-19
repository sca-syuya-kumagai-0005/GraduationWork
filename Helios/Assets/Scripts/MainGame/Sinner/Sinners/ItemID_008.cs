using System.Collections.Generic;
using UnityEngine;
public class ItemID_008 : Sinner
{
    float increase;
    float timer;
    const float timeLimit = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 200.0f, 150.0f, 0.0f, 0.0f, 25.0f, 50.0f, 50.0f, 100.0f };
        sinnerID = "ItemID_008";
        sinnerName = "輪唱の傘";
        LoadSprite("ID008");
        effect = GameObject.Find("Effect").transform.Find("").gameObject;
        GetEffectObject(true);
        effectTimer = 6.5f;
        KumagaiLibrary.Dictionary.Support.AddArray(specifyingDeliveryRoutes.SinnerDebuff, sinnerName, false);
    }
    // Update is called once per frame
    void Update()
    {
        if (timer >= timeLimit)
        {
            timer = 0.0f;
            List<int> normlLines = new List<int>();
            for (int i = 0; i > specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
            {
                if (!specifyingDeliveryRoutes.SinnerDebuff[i][sinnerName])
                    normlLines.Add(i);
            }
            int rand = Random.Range(0, normlLines.Count);
            AbnormalPhenomenon(normlLines[rand]);
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        //if (学校の前を通ってなかったら)
        {
            increase = 150.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID][sinnerName] = true;
    }
    private void AbnormalPhenomenon(int lineID)
    {
        //それぞれの処理はここに書く
        specifyingDeliveryRoutes.SinnerDebuff[lineID][sinnerName] = true;
    }
}
